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
        private int classificationsRequired, classificationsComplete, percent;
        private string classificationsText, durationText;
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

            if (Percent > 100)
                App.Current.Dispatcher.Invoke(() => ClassificationViewModel.ProgressComplete = true);
        }
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }

        public void ShowView(ObservableCollection<string> browsedScreenplays)
        {
            DurationTimer.Start();

            ClassificationsRequired = browsedScreenplays.Count;
            ClassificationsComplete = 0;
            Percent = 0;

            App.Current.Dispatcher.Invoke(() => ProgressView.Visibility = Visibility.Visible);

            ClassificationViewModel.ClassifiedScreenplays = ClassifyScreenplays(browsedScreenplays);
        }

        public void RefreshView()
        {
            DurationTimer.Stop();
            Duration = TimeSpan.Zero;

            ClassificationsRequired = 0;
            ClassificationsComplete = 0;
        }

        public void HideView()
        {
            RefreshView();

            App.Current.Dispatcher.Invoke(() => ProgressView.Visibility = Visibility.Collapsed);
        }

        public ObservableCollection<ClassificationModel> ClassifyScreenplays(ObservableCollection<string> screenplaysToClassify)
        {
            int progressOutput;
            string scriptPath = FolderPaths.CLASSIFIER + "Setup.py", scriptArgs = string.Join(" ", screenplaysToClassify);
            string outputLine = string.Empty, classificationsJson = string.Empty;
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = @"C:\Users\Admin\AppData\Local\Programs\Python\Python39\python.exe",
                Arguments = string.Format("\"{0}\" \"{1}\"", scriptPath, scriptArgs),
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
                        // TODO: FIX (reads nothing?)
                        outputLine = reader.ReadLine();
                        if ((!string.IsNullOrEmpty(outputLine)) && (int.TryParse(outputLine, out progressOutput)))
                        {
                            ClassificationsComplete = progressOutput;
                            Percent = (ClassificationsComplete * 100) / classificationsRequired;
                        }
                    }

                    classificationsJson = reader.ReadToEnd();
                }
            }

            return new ObservableCollection<ClassificationModel>(JsonConvert.DeserializeObject<List<ClassificationModel>>(classificationsJson));
        }

        // TODO: REMOVE (AFTER CLASSIFIER IS READY IN PYTHON) 
        private ObservableCollection<ClassificationModel> Mockup(ObservableCollection<string> screenplaysToClassify)
        {
            ObservableCollection<ClassificationModel> x = new ObservableCollection<ClassificationModel>();
            UserModel owner = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User;
            ScreenplayModel screenplay;
            Dictionary<string, float> matchingPercentages = new Dictionary<string, float>();

            matchingPercentages["Action"] = 20;
            matchingPercentages["Adventure"] = 10;
            matchingPercentages["Comedy"] = 10;
            matchingPercentages["Crime"] = 10;
            matchingPercentages["Drama"] = 10;
            matchingPercentages["Family"] = 10;
            matchingPercentages["Fantasy"] = 10;
            matchingPercentages["Horror"] = 10;
            matchingPercentages["Romance"] = 2.5f;
            matchingPercentages["SciFi"] = 2.5f;
            matchingPercentages["Thriller"] = 2.5f;
            matchingPercentages["War"] = 2.5f;

            foreach (string screenplayName in screenplaysToClassify)
            {
                screenplay = new ScreenplayModel(Path.GetFileNameWithoutExtension(screenplayName), matchingPercentages);

                x.Add(new ClassificationModel(owner, screenplay));
            }

            return x;
        }
    }
}
