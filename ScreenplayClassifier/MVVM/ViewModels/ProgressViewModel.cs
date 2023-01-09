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
    public class ProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private System.Timers.Timer durationTimer;
        private TimeSpan duration;
        private int classificationsRequired, classificationsComplete, percent, currentPhase;
        private string classificationsText, durationText, phaseText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ProgressView ProgressView { get; private set; }

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

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PhaseText"));
            }
        }

        // Constructors
        public ProgressViewModel()
        {
            DurationTimer = new System.Timers.Timer();
            DurationTimer.Interval = 1000;
            DurationTimer.Elapsed += DurationTimer_Elapsed;

            RefreshView();
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
        /// <param name="classificationViewModel">The view model who manages the classification module</param>
        /// <param name="progressView">The view to obtain controls from</param>
        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        /// <param name="browsedScreenplays">The screenplays to be processed</param>
        public void ShowView(ObservableCollection<string> browsedScreenplays)
        {
            DurationTimer.Start();

            ClassificationsRequired = browsedScreenplays.Count;
            ClassificationsComplete = 0;
            Percent = 0;

            App.Current.Dispatcher.Invoke(() => ProgressView.Visibility = Visibility.Visible);

            new Thread(() => ClassificationThread(browsedScreenplays)).Start();
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            DurationTimer.Stop();
            Duration = TimeSpan.Zero;

            ClassificationsRequired = 0;
            ClassificationsComplete = 0;

            CurrentPhase = 0;
            PhaseText = "Reading";
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            RefreshView();

            App.Current.Dispatcher.Invoke(() => ProgressView.Visibility = Visibility.Collapsed);
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
        /// <param name="screenplaysToClassify">The screenplays to be processed by the thread</param>
        private void ClassificationThread(ObservableCollection<string> screenplaysToClassify)
        {
            int progressOutput;
            string scriptPath = FolderPaths.CLASSIFIER + "Setup.py", scriptArgs = string.Join(" ", screenplaysToClassify);
            string outputLine = string.Empty, screenplaysJson = string.Empty;
            UserModel owner = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User;
            List<ClassificationModel> classifications = new List<ClassificationModel>();
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
            Thread.Sleep(500);

            // Generates classification report for each screenplay
            deserializedScreenplays = JsonConvert.DeserializeObject<List<ScreenplayModel>>(screenplaysJson);
            foreach (ScreenplayModel screenplay in deserializedScreenplays)
                classifications.Add(new ClassificationModel(owner, screenplay));

            // Stores the classification results
            ClassificationViewModel.ClassifiedScreenplays = new ObservableCollection<ClassificationModel>(classifications);
            App.Current.Dispatcher.Invoke(() => ClassificationViewModel.ProgressComplete = true);
        }
    }
}