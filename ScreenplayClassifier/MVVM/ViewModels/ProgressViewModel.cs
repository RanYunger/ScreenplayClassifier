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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> activeClassifications, inactiveClassifications;
        private ObservableCollection<ProgressModel> classificationsProgresses;
        private ImageSource stopImage, pauseImage, selectedGifImage;
        private System.Timers.Timer durationTimer;
        private TimeSpan totalDuration, selectedDuration;
        private bool allClassificationsPaused, canStop, canPause;
        private int classificationsRequired, classificationsComplete, classificationsProgress, selectedClassification, selectedPercentage;
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

        public ObservableCollection<ProgressModel> ClassificationsProgresses
        {
            get { return classificationsProgresses; }
            set
            {
                classificationsProgresses = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsProgresses"));
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

        public ImageSource SelectedGifImage
        {
            get { return selectedGifImage; }
            set
            {
                selectedGifImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedGifImage"));
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
                DurationTimer.Enabled = CanPause = !allClassificationsPaused;

                for (int i = 0; i < ClassificationsProgresses.Count; i++)
                    ClassificationsProgresses[i].IsPaused = allClassificationsPaused;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AllClassificationsPaused"));
            }
        }

        public bool CanStop
        {
            get { return canStop; }
            set
            {
                canStop = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanStop"));
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

        public int ClassificationsProgress
        {
            get { return classificationsProgress; }
            set
            {
                classificationsProgress = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsProgress"));
            }
        }

        public int SelectedClassification
        {
            get { return selectedClassification; }
            set
            {
                string imagePath = string.Empty;

                selectedClassification = value;
                if ((ClassificationsProgresses.Count > 0) && (selectedClassification != -1))
                {
                    SelectedDuration = ClassificationsProgresses[selectedClassification].Duration;
                    SelectedPercentage = ClassificationsProgresses[selectedClassification].Percentage;
                    SelectedDescription = ClassificationsProgresses[selectedClassification].Description;
                    SelectedGifImage = ClassificationsProgresses[selectedClassification].GifImage;

                    imagePath = string.Format("{0}Pause{1}.png", FolderPaths.IMAGES, ClassificationsProgresses[selectedClassification].IsPaused
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

            ClassificationsProgresses = new ObservableCollection<ProgressModel>();

            CanStop = true;
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

                    if ((ActiveClassifications.Count > 0) && (SelectedClassification != -1))
                    {
                        ClassificationsProgresses[SelectedClassification].IsPaused = !ClassificationsProgresses[SelectedClassification].IsPaused;

                        imagePath = string.Format("{0}Pause{1}.png", FolderPaths.IMAGES,
                            ClassificationsProgresses[SelectedClassification].IsPaused ? "Pressed" : "Unpressed");
                        PauseImage = new BitmapImage(new Uri(imagePath));
                    }
                });
            }
        }
        #endregion

        private void DurationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TotalDuration = TotalDuration.Add(new TimeSpan(0, 0, 1));

            if (ClassificationsComplete == ClassificationsRequired)
                lock (this)
                {
                    DurationTimer.Stop();
                    CanStop = CanPause = false;

                    App.Current.Dispatcher.Invoke(() => ClassificationViewModel.ProgressComplete = true);

                    MessageBoxHandler.Show("You can now view the classifications at the Results tab", "Classifications Complete", 3,
                        MessageBoxImage.Information);
                }
        }

        private void ProgressUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // TODO: FIX (stabilize the code - throws random exceptions from time to time)

            // Validations
            try
            {
                if ((App.Current == null) || (ClassificationsProgresses.Count == 0))
                    return;

                App.Current.Dispatcher.Invoke(() => SelectedClassification = SelectedClassification); // Triggers PropertyChange events
                for (int i = 0; i < ClassificationsProgresses.Count; i++)
                {
                    if (ClassificationsProgresses[i].IsPaused)
                        continue;

                    if (ClassificationsProgresses[i].IsComplete)
                    {
                        lock (this)
                        {
                            try
                            {
                                ClassificationsProgresses[i].DurationTimer.Stop();
                                ActiveClassifications[i].Duration = ClassificationsProgresses[i].Duration;
                                App.Current.Dispatcher.Invoke(() => ClassificationViewModel.ResultsViewModel.ClassifiedScreenplays.Add(ActiveClassifications[i]));
                                //App.Current.Dispatcher.Invoke(() => ClassificationViewModel.FeedbackViewModel.ClassifiedScreenplays.Add(ActiveClassifications[i]));

                                App.Current.Dispatcher.Invoke(() => ActiveClassifications.RemoveAt(i));
                                App.Current.Dispatcher.Invoke(() => ClassificationsProgresses.RemoveAt(i));

                                if (InactiveClassifications.Count > 0)
                                {
                                    App.Current.Dispatcher.Invoke(() => ActiveClassifications.Add(InactiveClassifications[0]));
                                    App.Current.Dispatcher.Invoke(() => ClassificationsProgresses.Add(new ProgressModel()));
                                    ClassificationsProgresses[ClassificationsProgresses.Count - 1].BackgroundWorker.RunWorkerAsync();
                                    ClassificationsProgresses[ClassificationsProgresses.Count - 1].DurationTimer.Start();

                                    App.Current.Dispatcher.Invoke(() => InactiveClassifications.RemoveAt(0));
                                }

                                ClassificationsComplete++;
                                ClassificationsProgress = (int)(((float)ClassificationsComplete / ClassificationsRequired) * 100);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
        }

        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }
    }
}
