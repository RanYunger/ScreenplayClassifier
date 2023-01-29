using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource firstImage, previousImage, nextImage, lastImage;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresViewModel PredictedGenresViewModel { get; private set; }
        public GenresViewModel ActualGenresViewModel { get; private set; }

        public ImageSource FirstImage
        {
            get { return firstImage; }
            set
            {
                firstImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstImage"));
            }
        }

        public ImageSource PreviousImage
        {
            get { return previousImage; }
            set
            {
                previousImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PreviousImage"));
            }
        }

        public ImageSource NextImage
        {
            get { return nextImage; }
            set
            {
                nextImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NextImage"));
            }
        }

        public ImageSource LastImage
        {
            get { return lastImage; }
            set
            {
                lastImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LastImage"));
            }
        }

        // Constructors
        public FeedbackViewModel() { }

        // Methods
        #region Commands
        public Command PressFirstCommand
        {
            get
            {
                return new Command(() =>
                {
                    ClassificationViewModel.BrowseViewModel.SelectedScreenplay = 0;

                    FirstImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "FirstPressed.png"));
                });
            }
        }

        public Command UnpressFirstCommand
        {
            get
            {
                return new Command(() =>
                {
                    FirstImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "FirstUnpressed.png"));
                });
            }
        }

        public Command PressPreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (ClassificationViewModel.BrowseViewModel.SelectedScreenplay > 0)
                        ClassificationViewModel.BrowseViewModel.SelectedScreenplay--;

                    PreviousImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PreviousPressed.png"));
                });
            }
        }

        public Command UnpressPreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    PreviousImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PreviousUnpressed.png"));
                });
            }
        }

        public Command SubmitFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult startOver;

                    if (!CanSubmit())
                        MessageBoxHandler.Show("Complete feedback for all screenplays", "Error", 3, MessageBoxImage.Error);
                    else
                    {
                        UpdateOtherModules();

                        startOver = MessageBox.Show("Would you like to start over?", "Classification Complete",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                        ClassificationViewModel.ClassificationComplete = startOver == MessageBoxResult.No;
                    }
                });
            }
        }

        public Command PressNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (ClassificationViewModel.BrowseViewModel.SelectedScreenplay < ClassificationViewModel.ClassifiedScreenplays.Count - 1)
                        ClassificationViewModel.BrowseViewModel.SelectedScreenplay++;

                    NextImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "NextPressed.png"));
                });
            }
        }

        public Command UnpressNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    NextImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "NextUnpressed.png"));
                });
            }
        }

        public Command PressLastCommand
        {
            get
            {
                return new Command(() =>
                {
                    ClassificationViewModel.BrowseViewModel.SelectedScreenplay = ClassificationViewModel.ClassifiedScreenplays.Count - 1;

                    LastImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LastPressed.png"));
                });
            }
        }

        public Command UnpressLastCommand
        {
            get
            {
                return new Command(() =>
                {
                    LastImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LastUnpressed.png"));
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        /// <param name="feedbackView">The view to obtain controls from</param>
        public void Init(ClassificationViewModel classificationViewModel, FeedbackView feedbackView)
        {
            GenresView predictedGenresView, actualGenresView;

            ClassificationViewModel = classificationViewModel;
            FeedbackView = feedbackView;

            predictedGenresView = (GenresView)FeedbackView.FindName("PredictedGenresView");
            PredictedGenresViewModel = (GenresViewModel)predictedGenresView.DataContext;
            PredictedGenresViewModel.Init(predictedGenresView);

            actualGenresView = (GenresView)FeedbackView.FindName("ActualGenresView");
            ActualGenresViewModel = (GenresViewModel)actualGenresView.DataContext;
            ActualGenresViewModel.Init(actualGenresView);

            FirstImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "FirstUnpressed.png"));
            PreviousImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PreviousUnpressed.png"));
            NextImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "NextUnpressed.png"));
            LastImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LastUnpressed.png"));

        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            ScreenplayModel screenplay = ClassificationViewModel.ClassifiedScreenplays[ClassificationViewModel.SelectedScreenplay].Screenplay;

            RefreshView(screenplay);
            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        /// <param name="selectedScreenplay">The screenplay to be shown in the view</param>
        public void RefreshView(ScreenplayModel selectedScreenplay)
        {
            try
            {
                PredictedGenresViewModel.RefreshView(selectedScreenplay, "Predicted");
                ActualGenresViewModel.RefreshView(selectedScreenplay, "Actual");
            }
            catch { }
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Indicates wheather the user can submit his feedback.
        /// </summary>
        /// <returns>True if feedback can be submited, False otherwise</returns>
        private bool CanSubmit()
        {
            ScreenplayModel currentScreenplay = null;

            for (int i = 0; i < ClassificationViewModel.ClassifiedScreenplays.Count; i++)
            {
                currentScreenplay = ClassificationViewModel.ClassifiedScreenplays[i].Screenplay;
                if ((currentScreenplay.ActualGenre == "Unknown") || (currentScreenplay.ActualSubGenre1 == "Unknown")
                    || (currentScreenplay.ActualSubGenre2 == "Unknown"))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Notifies other modules of the completed classification process.
        /// </summary>
        private void UpdateOtherModules()
        {
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesViewModel archivesViewModel = (ArchivesViewModel)ClassificationViewModel.MainViewModel.ArchivesView.DataContext;

            foreach (ClassificationModel report in ClassificationViewModel.ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(report);
                archivesViewModel.Screenplays.Add(report.Screenplay);
            }

            archivesViewModel.Screenplays = archivesViewModel.Screenplays; // Triggers PropertyChanged event
        }
    }
}