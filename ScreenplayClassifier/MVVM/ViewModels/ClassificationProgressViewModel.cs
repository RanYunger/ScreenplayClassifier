using Microsoft.Win32;
using Newtonsoft.Json;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private System.Timers.Timer durationTimer;
        private ImageSource phaseGif;
        private TimeSpan duration;
        private bool isThreadAlive;
        private int classificationsRequired, classificationsComplete, percent, currentPhase;
        private string classificationsText, durationText, phaseText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ClassificationProgressView ClassificationProgressView { get; private set; }
        public Thread ClassificationThread { get; private set; }

        public System.Timers.Timer DurationTimer
        {
            get { return durationTimer; }
            set
            {
                durationTimer = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DurationTimer"));
            }
        }

        public ImageSource PhaseGif
        {
            get { return phaseGif; }
            set
            {
                phaseGif = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PhaseGif"));
            }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                DurationText = duration.ToString();

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
            }
        }

        public bool IsThreadAlive
        {
            get { return isThreadAlive; }
            set
            {
                isThreadAlive = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsThreadAlive"));
            }
        }

        public int ClassificationsRequired
        {
            get { return classificationsRequired; }
            set
            {
                classificationsRequired = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsRequired"));
            }
        }

        public int ClassificationsComplete
        {
            get { return classificationsComplete; }
            set
            {
                classificationsComplete = value;
                ClassificationsText = string.Format("Classified: {0}/{1}", classificationsComplete, ClassificationsRequired);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsComplete"));
            }
        }

        public int Percent
        {
            get { return percent; }
            set
            {
                percent = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Percent"));
            }
        }

        public int CurrentPhase
        {
            get { return currentPhase; }
            set
            {
                currentPhase = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentPhase"));
            }
        }

        public string ClassificationsText
        {
            get { return classificationsText; }
            set
            {
                classificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsText"));
            }
        }

        public string DurationText
        {
            get { return durationText; }
            set
            {
                durationText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DurationText"));
            }
        }

        public string PhaseText
        {
            get { return phaseText; }
            set
            {
                phaseText = value;

                if ((phaseText != string.Empty) && (phaseText != "Classifying"))
                    App.Current.Dispatcher.Invoke(() => PhaseGif = new BitmapImage(new Uri(string.Format("{0}{1}.gif", FolderPaths.GIFS, phaseText))));

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PhaseText"));
            }
        }

        // Constructors
        public ClassificationProgressViewModel()
        {
            DurationTimer = new System.Timers.Timer();
            DurationTimer.Interval = 1000;
            DurationTimer.Elapsed += DurationTimer_Elapsed;
        }

        // Methods
        #region Commands
        private void DurationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Duration = Duration.Add(TimeSpan.FromSeconds(1));
        }

        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationProgressView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model who manages the classification module</param>
        public void Init(ClassificationProgressView classificationProgressView, ClassificationViewModel classificationViewModel)
        {
            ClassificationProgressView = classificationProgressView;
            ClassificationViewModel = classificationViewModel;
            ClassificationThread = null;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ClassificationProgressView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationProgressView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            DurationTimer.Stop();

            Duration = TimeSpan.Zero;

            IsThreadAlive = false;
            ClassificationsRequired = 0;
            ClassificationsComplete = 0;
            Percent = 0;
            CurrentPhase = 0;

            PhaseText = string.Empty;

            StartClassification(ClassificationViewModel.ClassificationBrowseViewModel.CheckedScreenplays);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationProgressView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationProgressView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Starts the classification process.
        /// </summary>
        /// <param name="screenplaysToClassify">The screenplays to be processed</param>
        public void StartClassification(ObservableCollection<SelectionModel> screenplaysToClassify)
        {
            ObservableCollection<string> screenplayFilePaths = new ObservableCollection<string>();

            foreach (SelectionModel browsedScreenplay in screenplaysToClassify)
                screenplayFilePaths.Add(browsedScreenplay.ScreenplayFilePath);

            DurationTimer.Start();

            ClassificationsRequired = screenplaysToClassify.Count;
            ClassificationsComplete = 0;

            PhaseText = "Processing";
            
            IsThreadAlive = true;
            ClassificationThread = new Thread(() => ClassifyScreenplays(screenplayFilePaths));
            ClassificationThread.Start();
        }

        /// <summary>
        /// Retrieves the installation path of Python.exe on the local machine.
        /// </summary>
        /// <returns>The installation path of Python.exe on the local machine</returns>
        private string GetPythonPath()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Python\PythonCore\3.9\PythonPath");
            string registryValue = registryKey.GetValue("").ToString();
            string pythonPath = registryValue.Substring(0, registryValue.IndexOf("Lib"));

            return pythonPath;
        }

        /// <summary>
        /// Classification thread: sends the screenplays to the python classifier for processing.
        /// </summary>
        /// <param name="screenplayFilePaths">The file paths of the screenplays to be processed by the thread</param>
        private void ClassifyScreenplays(ObservableCollection<string> screenplayFilePaths)
        {
            int progressOutput;
            string scriptPath = FolderPaths.CLASSIFIER + "Main.py", scriptArgs = string.Join(" ", screenplayFilePaths);
            string outputLine = string.Empty, screenplaysJson = string.Empty;
            UserModel owner = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User;
            List<ScreenplayModel> deserializedScreenplays;
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = GetPythonPath() + "python.exe",
                Arguments = string.Format("\"{0}\" {1}", scriptPath, scriptArgs),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    while (Percent < 100)
                    {
                        // Validation
                        if (!IsThreadAlive)
                            return;

                        // Reads progress values printed by the python classifier
                        outputLine = reader.ReadLine();
                        if ((!string.IsNullOrEmpty(outputLine)) && (int.TryParse(outputLine, out progressOutput)))
                        {
                            CurrentPhase = 1;
                            PhaseText = "Classifying";

                            ClassificationsComplete = progressOutput;
                            Percent = (ClassificationsComplete * 100) / classificationsRequired;
                        }

                        Thread.Sleep(500);
                    }

                    // Reads the rest of the process output, which contains the json representation of the classifications
                    screenplaysJson = reader.ReadToEnd();
                }
            }

            Thread.Sleep(500);
            CurrentPhase = 2;
            PhaseText = "Done!";
            Thread.Sleep(1700);

            // Generates classification report for each screenplay
            deserializedScreenplays = JsonConvert.DeserializeObject<List<ScreenplayModel>>(screenplaysJson);
            ClassificationViewModel.ClassificationFeedbackViewModel.FeedbackedScreenplays
                = new ObservableCollection<ScreenplayModel>(deserializedScreenplays);
            App.Current.Dispatcher.Invoke(() => ClassificationViewModel.ProgressComplete = true);
        }
    }
}