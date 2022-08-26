using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private System.Timers.Timer durationTimer;

        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private TimeSpan duration;
        private int classificationsRequired, classificationsComplete;
        private string classificationsText, durationText;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ProgressView ProgressView { get; private set; }

        public ObservableCollection<ClassificationModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
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
            Duration = TimeSpan.Zero;

            durationTimer = new System.Timers.Timer();
            durationTimer.Interval = 1000;
            durationTimer.Elapsed += DurationTimer_Elapsed;
        }

        // Methods
        #region Commands
        private void DurationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Duration = Duration.Add(new TimeSpan(0, 0, 1));

            if (Duration.Seconds == 5)
                ClassificationViewModel.ProgressComplete = true;
        }
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }

        public void SetView(ObservableCollection<string> browsedScreenplays)
        {
            durationTimer.Start();

            ClassificationsRequired = browsedScreenplays.Count;
            ClassificationsComplete = 0;
            ClassifiedScreenplays = new ObservableCollection<ClassificationModel>();

            App.Current.Dispatcher.Invoke(() => ProgressView.Visibility = Visibility.Visible);

            ClassifiedScreenplays = ClassifyScreenplays(browsedScreenplays);
        }

        public void ResetView()
        {
            durationTimer.Stop();

            ClassificationsRequired = 0;
            ClassificationsComplete = 0;
            ClassifiedScreenplays.Clear();

            App.Current.Dispatcher.Invoke(() => ProgressView.Visibility = Visibility.Collapsed);

            App.Current.Dispatcher.Invoke(() => ClassificationViewModel.ClassifiedScreenplays = ClassifiedScreenplays);
        }

        public ObservableCollection<ClassificationModel> ClassifyScreenplays(ObservableCollection<string> screenplaysToClassify)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource source = engine.CreateScriptSourceFromFile(FolderPaths.PYTHON + "Setup.py");
            MemoryStream errorsMemoryStream = new MemoryStream(), resultsMemoryStream = new MemoryStream();
            List<string> argv = new List<string>() { string.Empty };
            string classifierResultsStr = string.Empty;
            string[] classificationStrings;

            argv.AddRange(string.Join(" ", screenplaysToClassify).Split(" ", StringSplitOptions.RemoveEmptyEntries));

            engine.GetSysModule().SetVariable("argv", argv);
            engine.Runtime.IO.SetErrorOutput(errorsMemoryStream, Encoding.Default);
            engine.Runtime.IO.SetOutput(resultsMemoryStream, Encoding.Default);

            source.Execute(engine.CreateScope());

            classifierResultsStr = Encoding.Default.GetString(resultsMemoryStream.ToArray());
            classificationStrings = classifierResultsStr.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            
            return ProcessResults(classificationStrings);
        }

        public ObservableCollection<ClassificationModel> ProcessResults(string[] resultStrings)
        {
            ObservableCollection<ClassificationModel> classifications = new ObservableCollection<ClassificationModel>();

            // TODO: COMPLETE (convert each result string into ClassificationModel to add to ClassifiedScreenplays)

            return classifications;
        }
    }
}
