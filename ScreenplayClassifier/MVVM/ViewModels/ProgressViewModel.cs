using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> activeClassifications, inactiveClassifications;
        private ImageSource stopImage, pauseImage;
        private ProgressModel[] classificationsProgress;
        private System.Timers.Timer durationTimer;
        private TimeSpan totalDuration, selectedDuration;
        private bool allClassificationsPaused, canPause;
        private int classificationsLeft, classificationsComplete, selectedClassification, selectedPercentage;
        private string classificationsText, selectedDescription;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ProgressView ProgressView { get; private set; }

        public ObservableCollection<ClassificationModel> ActiveClassifications
        {
            get { return activeClassifications; }
            set
            {
                activeClassifications = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActiveClassifications"));
            }
        }

        public ObservableCollection<ClassificationModel> InactiveClassifications
        {
            get { return inactiveClassifications; }
            set
            {
                inactiveClassifications = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("InactiveClassifications"));
            }
        }

        public ImageSource StopImage
        {
            get { return stopImage; }
            set
            {
                stopImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("StopImage"));
            }
        }

        public ImageSource PauseImage
        {
            get { return pauseImage; }
            set
            {
                pauseImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PauseImage"));
            }
        }
        public ProgressModel[] ClassificationsProgress
        {
            get { return classificationsProgress; }
            set
            {
                classificationsProgress = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsProgress"));
            }
        }

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

        public TimeSpan TotalDuration
        {
            get { return totalDuration; }
            set
            {
                totalDuration = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalDuration"));
            }
        }

        public TimeSpan SelectedDuration
        {
            get { return selectedDuration; }
            set
            {
                selectedDuration = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDuration"));
            }
        }

        public bool AllClassificationsPaused
        {
            get { return allClassificationsPaused; }
            set
            {
                allClassificationsPaused = value;
                CanPause = !allClassificationsPaused;
                for (int i = 0; i < ClassificationsProgress.Length; i++)
                    ClassificationsProgress[i].IsPaused = allClassificationsPaused;
                SelectedClassification = SelectedClassification; // Triggers PropertyChanged event

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AllClassificationsPaused"));
            }
        }

        public bool CanPause
        {
            get { return canPause; }
            set
            {
                canPause = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanPause"));
            }
        }

        public int ClassificationsLeft
        {
            get { return classificationsLeft; }
            set
            {
                classificationsLeft = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsLeft"));
            }
        }

        public int ClassificationsComplete
        {
            get { return classificationsComplete; }
            set
            {
                classificationsComplete = value;
                ClassificationsText = string.Format("Classified: {0}/{1}", classificationsComplete, ClassificationsLeft);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsComplete"));
            }
        }

        public int SelectedClassification
        {
            get { return selectedClassification; }
            set
            {
                string imagePath = string.Empty;

                selectedClassification = value;
                if (selectedClassification != -1)
                {
                    SelectedDuration = ClassificationsProgress[selectedClassification].Duration;
                    SelectedPercentage = ClassificationsProgress[selectedClassification].Percentage;
                    SelectedDescription = ClassificationsProgress[selectedClassification].Description;

                    imagePath = string.Format("{0}Pause{1}.png", FolderPaths.IMAGES, ClassificationsProgress[selectedClassification].IsPaused
                        ? "Pressed" : "Unpressed");
                    PauseImage = new BitmapImage(new Uri(imagePath));
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassification"));
            }
        }

        public int SelectedPercentage
        {
            get { return selectedPercentage; }
            set
            {
                selectedPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPercentage"));
            }
        }

        public string SelectedDescription
        {
            get { return selectedDescription; }
            set
            {
                selectedDescription = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDescription"));
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

        // Constructors
        public ProgressViewModel()
        {
            System.Timers.Timer progressUpdateTimer = new System.Timers.Timer(10);
            progressUpdateTimer.Elapsed += ProgressUpdateTimer_Elapsed;
            progressUpdateTimer.Start();

            DurationTimer = new System.Timers.Timer(1000);
            DurationTimer.Elapsed += DurationTimer_Elapsed;

            ActiveClassifications = new ObservableCollection<ClassificationModel>();
            InactiveClassifications = new ObservableCollection<ClassificationModel>();

            StopImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "StopUnpressed.png"));
            PauseImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PauseUnpressed.png"));

            ClassificationsProgress = new ProgressModel[5];
            for (int i = 0; i < 5; i++)
                ClassificationsProgress[i] = new ProgressModel();

            AllClassificationsPaused = false;
            SelectedClassification = 0;
        }

        // Methods

        #region Commands
        public Command ToggleStopCommand
        {
            get
            {
                return new Command(() =>
                {
                    string imagePath = string.Empty;

                    AllClassificationsPaused = !AllClassificationsPaused;

                    imagePath = string.Format("{0}Stop{1}.png", FolderPaths.IMAGES, AllClassificationsPaused ? "Pressed" : "Unpressed");
                    StopImage = new BitmapImage(new Uri(imagePath));
                });
            }
        }
        public Command TogglePauseCommand
        {
            get
            {
                return new Command(() =>
                {
                    string imagePath = string.Empty;

                    if (ActiveClassifications.Count > 0)
                    {
                        ClassificationsProgress[SelectedClassification].IsPaused = !ClassificationsProgress[SelectedClassification].IsPaused;
                        if (ClassificationsProgress[SelectedClassification].IsPaused)
                            ClassificationsProgress[SelectedClassification].BackgroundWorker.CancelAsync();

                        imagePath = string.Format("{0}Pause{1}.png", FolderPaths.IMAGES, ClassificationsProgress[SelectedClassification].IsPaused
                            ? "Pressed" : "Unpressed");
                        PauseImage = new BitmapImage(new Uri(imagePath));
                    }
                });
            }
        }
        #endregion
        private void DurationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TotalDuration = TotalDuration.Add(new TimeSpan(0, 0, 1));
        }

        private void ProgressUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Triggers PropertyChange events
            if (App.Current != null)
            {
                App.Current.Dispatcher.Invoke(() => SelectedClassification = SelectedClassification);
                /*if (ClassificationsProgress[0].IsComplete)
                {
                    App.Current.Dispatcher.Invoke(() => ClassificationsComplete++);

                    App.Current.Dispatcher.Invoke(() => ActiveClassifications.RemoveAt(0));
                    if (InactiveClassifications.Count > 0)
                    {
                        App.Current.Dispatcher.Invoke(() => activeClassifications.Insert(0, InactiveClassifications[0]));
                        App.Current.Dispatcher.Invoke(() => InactiveClassifications.RemoveAt(0));
                    }
                }*/
            }
        }

        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }
    }
}
