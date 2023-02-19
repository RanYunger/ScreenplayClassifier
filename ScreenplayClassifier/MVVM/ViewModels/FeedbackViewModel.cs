using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> feedbackedScreenplays;
        private List<BrowseModel> checkedScreenplays;
        private int screenplayOffset;
        private bool canSubmit;
        private string classificationsText, genreClassificationsText, subGenre1ClassificationsText, subGenre2ClassificationsText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public ReportViewModel ReportViewModel { get; private set; }

        public ObservableCollection<ReportModel> FeedbackedScreenplays
        {
            get { return feedbackedScreenplays; }
            set
            {
                feedbackedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("feedbackedScreenplays"));
            }
        }

        public List<BrowseModel> CheckedScreenplays
        {
            get { return checkedScreenplays; }
            set
            {
                checkedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedScreenplays"));
            }
        }

        public int ScreenplayOffset
        {
            get { return screenplayOffset; }
            set
            {
                screenplayOffset = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayOffset"));
            }
        }

        public bool CanSubmit
        {
            get { return canSubmit; }
            set
            {
                canSubmit = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanSubmit"));
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

        public string GenreClassificationsText
        {
            get { return genreClassificationsText; }
            set
            {
                genreClassificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreClassificationsText"));
            }
        }

        public string SubGenre1ClassificationsText
        {
            get { return subGenre1ClassificationsText; }
            set
            {
                subGenre1ClassificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1ClassificationsText"));
            }
        }

        public string SubGenre2ClassificationsText
        {
            get { return subGenre2ClassificationsText; }
            set
            {
                subGenre2ClassificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2ClassificationsText"));
            }
        }

        // Constructors
        public FeedbackViewModel() { }

        // Methods
        #region Commands
        public Command ProceedCommand
        {
            get
            {
                ReportView reportView = null;

                return new Command(() =>
                {
                    // Validation
                    if (FeedbackView == null)
                        return;

                    if ((ScreenplayOffset >= 0) && (!FeedbackedScreenplays[ScreenplayOffset].Screenplay.Isfeedbacked))
                    {
                        MessageBoxHandler.Show("Complete feedback before proceeding", string.Empty, 5, MessageBoxImage.Error);
                        return;
                    }

                    reportView = (ReportView)FeedbackView.FindName("ReportView");

                    CanSubmit = ++ScreenplayOffset == FeedbackedScreenplays.Count;

                    if (CanSubmit)
                    {
                        reportView.Visibility = Visibility.Collapsed;
                        PrepareReviewCommand.Execute(null);
                    }
                    else
                        ReportViewModel.Init(reportView, FeedbackedScreenplays[ScreenplayOffset], true);
                });
            }
        }

        public Command PrepareReviewCommand
        {
            get
            {
                StackPanel reviewStackPanel = (StackPanel)FeedbackView.FindName("ReviewStackPanel");

                return new Command(() =>
                {
                    List<ScreenplayModel> screenplays = new List<ScreenplayModel>();
                    Predicate<ScreenplayModel>[] queries = { s => s.IsClassifiedCorrectly, s => s.ModelGenre == s.OwnerGenre,
                        s => s.ModelSubGenre1 == s.OwnerSubGenre1, s => s.ModelSubGenre2 == s.OwnerSubGenre2 };
                    int correctClassifications;
                    string formattedText;
                    double percentage;

                    foreach (ReportModel reportModel in FeedbackedScreenplays)
                        screenplays.Add(reportModel.Screenplay);

                    for (int i = 0; i < queries.Length; i++)
                    {
                        correctClassifications = screenplays.FindAll(queries[i]).Count;
                        percentage = (100.0 * correctClassifications) / screenplays.Count;
                        formattedText = string.Format("{0}/{1} ({2}%)", correctClassifications, screenplays.Count, percentage.ToString("0.00"));

                        switch (i)
                        {
                            case 0: ClassificationsText = formattedText; break;
                            case 1: GenreClassificationsText = formattedText; break;
                            case 2: SubGenre1ClassificationsText = formattedText; break;
                            case 3: SubGenre2ClassificationsText = formattedText; break;
                        }
                    }

                    reviewStackPanel.Visibility = Visibility.Visible;
                });
            }
        }

        public Command SubmitFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult reclassifyDecision = MessageBox.Show("Would you like to re-classify this batch?", "Classification Complete",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (reclassifyDecision == MessageBoxResult.Yes)
                        ClassificationViewModel.ClassificationComplete = false;
                    else
                    {
                        UpdateOtherModules();
                        ClassificationViewModel.ClassificationComplete = true;
                    }

                    CanSubmit = false;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="feedbackView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(FeedbackView feedbackView, ClassificationViewModel classificationViewModel)
        {
            ReportView reportView = null;

            FeedbackView = feedbackView;
            ClassificationViewModel = classificationViewModel;

            FeedbackedScreenplays = new ObservableCollection<ReportModel>();
            CheckedScreenplays = new List<BrowseModel>();

            reportView = (ReportView)FeedbackView.FindName("ReportView");
            ReportViewModel = (ReportViewModel)reportView.DataContext;

            ClassificationsText = string.Empty;
            GenreClassificationsText = string.Empty;
            SubGenre1ClassificationsText = string.Empty;
            SubGenre2ClassificationsText = string.Empty;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (FeedbackView != null)
                App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes and shows the view.
        /// </summary>
        public void RefreshView()
        {
            StackPanel reviewStackPanel = (StackPanel)FeedbackView.FindName("ReviewStackPanel");
            ReportView reportView = (ReportView)FeedbackView.FindName("ReportView");
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;

            // Obtains the collection of feedbacked screenplays
            FeedbackedScreenplays = ClassificationViewModel.ClassifiedScreenplays;

            // Obtains the collection of checked offsets
            CheckedScreenplays.Clear();
            foreach (BrowseModel browsedScreenplay in browseViewModel.BrowsedScreenplays)
                if (browsedScreenplay.IsChecked)
                    CheckedScreenplays.Add(browsedScreenplay);

            CanSubmit = false;
            ScreenplayOffset = -1;

            ProceedCommand.Execute(null);

            reviewStackPanel.Visibility = Visibility.Collapsed;
            reportView.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (FeedbackView != null)
                App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Notifies other modules of the completed classification process.
        /// </summary>
        private void UpdateOtherModules()
        {
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesViewModel archivesViewModel = (ArchivesViewModel)ClassificationViewModel.MainViewModel.ArchivesView.DataContext;

            foreach (BrowseModel checkedScreenplay in CheckedScreenplays)
                browseViewModel.BrowsedScreenplays.Remove(checkedScreenplay);

            foreach (ReportModel report in ClassificationViewModel.ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(report);
                archivesViewModel.Screenplays.Add(report.Screenplay);
            }

            // Triggers PropertyChanged events
            reportsViewModel.Reports = reportsViewModel.Reports;
            archivesViewModel.Screenplays = archivesViewModel.Screenplays;
        }
    }
}