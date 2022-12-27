using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FeedbackViewModel
    {
        // Fields

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresViewModel PredictedGenresViewModel { get; private set; }
        public GenresViewModel ActualGenresViewModel { get; private set; }

        // Constructors
        public FeedbackViewModel() { }

        // Methods
        #region Commands
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