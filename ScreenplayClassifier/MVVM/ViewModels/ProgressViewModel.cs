using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> activeClassifications, inactiveClassifications;
        private ImageSource stopImage, pauseImage;
        private bool allClassificationsStopped, canPause;
        private bool[] classificationsPaused;
        private int selectedActiveClassification;
        private string stepDescription;

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

        public bool AllClassificationsStopped
        {
            get { return allClassificationsStopped; }
            set
            {
                allClassificationsStopped = value;
                CanPause = !allClassificationsStopped;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AllClassificationsStopped"));
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

        public bool[] ClassificationsPaused
        {
            get { return classificationsPaused; }
            set
            {
                classificationsPaused = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsPaused"));
            }
        }

        public int SelectedActiveClassification
        {
            get { return selectedActiveClassification; }
            set
            {
                string imagePath = string.Empty;

                selectedActiveClassification = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedActiveClassification"));

                imagePath = string.Format("{0}Pause{1}.png", FolderPaths.IMAGES, ClassificationsPaused[selectedActiveClassification]
                    ? "Pressed" : "Unpressed");
                PauseImage = new BitmapImage(new Uri(imagePath));
            }
        }

        public string StepDescription
        {
            get { return stepDescription; }
            set
            {
                stepDescription = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("StepDescription"));
            }
        }

        // Constructors
        public ProgressViewModel()
        {
            ActiveClassifications = new ObservableCollection<ClassificationModel>();
            InactiveClassifications = new ObservableCollection<ClassificationModel>();

            StopImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "StopUnpressed.png"));
            PauseImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PauseUnpressed.png"));

            AllClassificationsStopped = false;
            ClassificationsPaused = new bool[5];
            SelectedActiveClassification = 0;
            StepDescription = "Reading screenplay...";
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

                    AllClassificationsStopped = !AllClassificationsStopped;

                    imagePath = string.Format("{0}Stop{1}.png", FolderPaths.IMAGES, AllClassificationsStopped ? "Pressed" : "Unpressed");
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

                    ClassificationsPaused[SelectedActiveClassification] = !ClassificationsPaused[SelectedActiveClassification];

                    imagePath = string.Format("{0}Pause{1}.png", FolderPaths.IMAGES, ClassificationsPaused[SelectedActiveClassification]
                        ? "Pressed" : "Unpressed");
                    PauseImage = new BitmapImage(new Uri(imagePath));
                });
            }
        }
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }
    }
}
