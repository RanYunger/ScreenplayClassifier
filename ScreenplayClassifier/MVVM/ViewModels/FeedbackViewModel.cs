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
        private List<int> checkedOffsets;
        private int currentOffset;
        private bool canGoToFirst, canGoToPrevious, canGoToNext, canGoToLast;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresViewModel PredictedGenresViewModel { get; private set; }
        public GenresViewModel ActualGenresViewModel { get; private set; }

        public List<int> CheckedOffsets
        {
            get { return checkedOffsets; }
            set
            {
                checkedOffsets = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedOffsets"));
            }
        }

        public int CurrentOffset
        {
            get { return currentOffset; }
            set
            {
                BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;
                ScreenplayModel shownScreenplay = null;

                currentOffset = value;

                if (CheckedOffsets.Count > 0)
                {
                    browseViewModel.SelectedScreenplay = CheckedOffsets[currentOffset];

                    shownScreenplay = ClassificationViewModel.ClassifiedScreenplays[currentOffset].Screenplay;
                    RefreshView(shownScreenplay);
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentOffset"));
            }
        }

        public bool CanGoToFirst
        {
            get { return canGoToFirst; }
            set
            {
                canGoToFirst = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToFirst"));
            }
        }

        public bool CanGoToPrevious
        {
            get { return canGoToPrevious; }
            set
            {
                canGoToPrevious = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToPrevious"));
            }
        }

        public bool CanGoToNext
        {
            get { return canGoToNext; }
            set
            {
                canGoToNext = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToNext"));
            }
        }

        public bool CanGoToLast
        {
            get { return canGoToLast; }
            set
            {
                canGoToLast = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToLast"));
            }
        }

        // Constructors
        public FeedbackViewModel() { }

        // Methods
        #region Commands
        public Command GoToFirstCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentOffset = 0;

                    CanGoToFirst = false;
                    CanGoToPrevious = false;
                    CanGoToNext = true;
                    CanGoToLast = true;
                });
            }
        }

        public Command GoToPreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentOffset = CurrentOffset - 1 <= 0 ? 0 : CurrentOffset - 1;

                    if (CurrentOffset == 0)
                    {
                        CanGoToFirst = false;
                        CanGoToPrevious = false;
                    }
                    CanGoToNext = true;
                    CanGoToLast = true;
                });
            }
        }

        public Command SubmitAllFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    ScreenplayModel screenplay = null;

                    // Validation
                    foreach (ClassificationModel classification in ClassificationViewModel.ClassifiedScreenplays)
                    {
                        screenplay = classification.Screenplay;
                        if (!screenplay.Isfeedbacked)
                        {
                            MessageBoxHandler.Show("Complete feedback for " + screenplay.Title, "Error", 3, MessageBoxImage.Error);
                            return;
                        }
                    }

                    // TODO: COMPLETE

                    //MessageBoxResult startOver;
                    //else
                    //{
                    //  UpdateOtherModules();
                    //    startOver = MessageBox.Show("Would you like to start over?", "Classification Complete",
                    //        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //    ClassificationViewModel.ClassificationComplete = startOver == MessageBoxResult.No;
                    //}
                });
            }
        }

        public Command GoToNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentOffset = CurrentOffset + 1 >= CheckedOffsets.Count - 1 ? CheckedOffsets.Count - 1 : CurrentOffset + 1;

                    CanGoToFirst = true;
                    CanGoToPrevious = true;
                    if (CurrentOffset == CheckedOffsets.Count - 1)
                    {
                        CanGoToNext = false;
                        CanGoToLast = false;
                    }
                });
            }
        }

        public Command GoToLastCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentOffset = CheckedOffsets.Count - 1;

                    CanGoToFirst = true;
                    CanGoToPrevious = true;
                    CanGoToNext = false;
                    CanGoToLast = false;
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

            CheckedOffsets = new List<int>();
            CurrentOffset = -1;
            CanGoToFirst = false;
            CanGoToPrevious = false;
            CanGoToNext = true;
            CanGoToLast = true;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;

            CheckedOffsets.Clear();
            foreach (BrowseModel checkedScreenplay in browseViewModel.CheckedScreenplays)
                CheckedOffsets.Add(browseViewModel.BrowsedScreenplays.IndexOf(checkedScreenplay));

            GoToFirstCommand.Execute(null);

            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        /// <param name="shownScreenplay">The screenplay to be shown in the view</param>
        public void RefreshView(ScreenplayModel shownScreenplay)
        {
            try
            {
                PredictedGenresViewModel.RefreshView(shownScreenplay, "Predicted");
                ActualGenresViewModel.RefreshView(shownScreenplay, "Actual");
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