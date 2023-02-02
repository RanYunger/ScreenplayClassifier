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
        private ObservableCollection<ScreenplayModel> feedbackedScreenplays;
        private List<BrowseModel> checkedScreenplays;
        private List<bool> canAnswerSurveys;
        private int currentOffset;
        private bool canAnswerSurvey, canGoToFirst, canGoToPrevious, canSubmit, canGoToNext, canGoToLast;
        private string classificationsText, genreClassificationsText, subGenre1ClassificationsText, subGenre2ClassificationsText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresViewModel ModelClassificationGenresViewModel { get; private set; }
        public GenresViewModel UserClassificationGenresViewModel { get; private set; }

        public ObservableCollection<ScreenplayModel> FeedbackedScreenplays
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

        public List<bool> CanAnswerSurveys
        {
            get { return canAnswerSurveys; }
            set
            {
                canAnswerSurveys = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanAnswerSurveys"));
            }
        }

        public int CurrentOffset
        {
            get { return currentOffset; }
            set
            {
                BrowseViewModel browseViewModel = null;
                ScreenplayModel feedbackedScreenplay = null;

                currentOffset = value;

                if (currentOffset != -1)
                {
                    browseViewModel = ClassificationViewModel.BrowseViewModel;
                    feedbackedScreenplay = FeedbackedScreenplays[currentOffset];

                    browseViewModel.SelectedScreenplay = browseViewModel.BrowsedScreenplays.IndexOf(CheckedScreenplays[currentOffset]);

                    ModelClassificationGenresViewModel.RefreshView(feedbackedScreenplay, "Model");
                    UserClassificationGenresViewModel.RefreshView(feedbackedScreenplay, "User");

                    if (CanAnswerSurvey = CanAnswerSurveys[currentOffset])
                        PrepareSurveyCommand.Execute(null);
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentOffset"));
            }
        }

        public bool CanAnswerSurvey
        {
            get { return canAnswerSurvey; }
            set
            {
                canAnswerSurvey = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanAnswerSurvey"));
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
        public Command PrepareSurveyCommand
        {
            get
            {
                RadioButton yesCorrectRadioButton = null, noCorrectRadioButton = null, yesReadRadioButton = null, noReadRadioButton = null;

                return new Command(() =>
                {
                    // Validation
                    if (FeedbackView == null)
                        return;

                    yesCorrectRadioButton = (RadioButton)FeedbackView.FindName("YesCorrectRadioButton");
                    noCorrectRadioButton = (RadioButton)FeedbackView.FindName("NoCorrectRadioButton");
                    yesReadRadioButton = (RadioButton)FeedbackView.FindName("YesReadRadioButton");
                    noReadRadioButton = (RadioButton)FeedbackView.FindName("NoReadRadioButton");

                    yesCorrectRadioButton.IsChecked = false;
                    noCorrectRadioButton.IsChecked = false;
                    yesReadRadioButton.IsChecked = false;
                    noReadRadioButton.IsChecked = false;
                });
            }
        }

        public Command SubmitSurveyCommand
        {
            get
            {
                RadioButton yesCorrectRadioButton = null, noCorrectRadioButton = null, yesReadRadioButton = null, noReadRadioButton = null;
                ScreenplayView screenplayView = null;
                ScreenplayModel feedbackedScreenplay = null;

                return new Command(() =>
                {
                    // Validation
                    if (FeedbackView == null)
                        return;

                    yesCorrectRadioButton = (RadioButton)FeedbackView.FindName("YesCorrectRadioButton");
                    noCorrectRadioButton = (RadioButton)FeedbackView.FindName("NoCorrectRadioButton");
                    yesReadRadioButton = (RadioButton)FeedbackView.FindName("YesReadRadioButton");
                    noReadRadioButton = (RadioButton)FeedbackView.FindName("NoReadRadioButton");

                    if (yesCorrectRadioButton.IsChecked == noCorrectRadioButton.IsChecked)
                    {
                        MessageBoxHandler.Show("Choose whether the model classification is correct.", string.Empty, 3, MessageBoxImage.Exclamation);
                        return;
                    }
                    if (yesReadRadioButton.IsChecked == noReadRadioButton.IsChecked)
                    {
                        MessageBoxHandler.Show("Choose whether you'd like to read the screenplay.", string.Empty, 3, MessageBoxImage.Exclamation);
                        return;
                    }

                    feedbackedScreenplay = FeedbackedScreenplays[CurrentOffset];

                    if (yesCorrectRadioButton.IsChecked.Value)
                    {
                        feedbackedScreenplay.UserGenre = feedbackedScreenplay.ModelGenre;
                        feedbackedScreenplay.UserSubGenre1 = feedbackedScreenplay.ModelSubGenre1;
                        feedbackedScreenplay.UserSubGenre2 = feedbackedScreenplay.ModelSubGenre2;
                    }

                    if (yesReadRadioButton.IsChecked.Value)
                    {
                        screenplayView = new ScreenplayView();
                        ((ScreenplayViewModel)screenplayView.DataContext).Init(feedbackedScreenplay.FilePath);

                        screenplayView.Show();
                    }

                    CanAnswerSurveys[CurrentOffset] = false;
                    
                    CurrentOffset = CurrentOffset; // Triggers PropertyChanged event
                });
            }
        }

        public Command PrepareReviewCommand
        {
            get
            {
                return new Command(() =>
                {
                    List<ScreenplayModel> screenplays = new List<ScreenplayModel>(FeedbackedScreenplays);
                    Predicate<ScreenplayModel>[] queries = { s => s.IsClassifiedCorrectly, s => s.ModelGenre == s.UserGenre,
                s => s.ModelSubGenre1 == s.UserSubGenre1, s => s.ModelSubGenre2 == s.UserSubGenre2 };
                    int correctClassifications;
                    string formattedText;
                    double percentage;

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
                });
            }
        }

        public Command GoToFirstCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentOffset = 0;

                    CanGoToFirst = false;
                    CanGoToPrevious = false;
                    CanGoToNext = FeedbackedScreenplays.Count > 1;
                    CanGoToLast = FeedbackedScreenplays.Count > 1;
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

        public Command SubmitFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult reclassifyDecision;

                    // Validation
                    foreach (ClassificationModel classification in ClassificationViewModel.ClassifiedScreenplays)
                        if (!classification.Screenplay.Isfeedbacked)
                        {
                            MessageBoxHandler.Show("Complete feedback for " + classification.Screenplay.Title, string.Empty, 3,
                                MessageBoxImage.Exclamation);
                            return;
                        }

                    PrepareReviewCommand.Execute(null);

                    CanSubmit = true;

                    // Closes an existing ScreenplayView (if there's one)
                    foreach (Window view in App.Current.Windows)
                        if (view is ScreenplayView)
                        {
                            view.Close();
                            break;
                        }

                    reclassifyDecision = MessageBox.Show("Would you like to re-classify this batch?", "Classification Complete",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (reclassifyDecision == MessageBoxResult.No)
                        UpdateOtherModules();

                    ClassificationViewModel.ClassificationComplete = false;
                });
            }
        }

        public Command GoToNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentOffset = CurrentOffset + 1 >= FeedbackedScreenplays.Count - 1 ? FeedbackedScreenplays.Count - 1 : CurrentOffset + 1;

                    CanGoToFirst = true;
                    CanGoToPrevious = true;
                    if (CurrentOffset == FeedbackedScreenplays.Count - 1)
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
                    CurrentOffset = FeedbackedScreenplays.Count - 1;

                    CanGoToFirst = FeedbackedScreenplays.Count > 1;
                    CanGoToPrevious = FeedbackedScreenplays.Count > 1;
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
            GenresView modelClassificationGenresView, userClassificationGenresView;

            ClassificationViewModel = classificationViewModel;
            FeedbackView = feedbackView;

            modelClassificationGenresView = (GenresView)FeedbackView.FindName("ModelClassificationGenresView");
            ModelClassificationGenresViewModel = (GenresViewModel)modelClassificationGenresView.DataContext;
            ModelClassificationGenresViewModel.Init(modelClassificationGenresView);

            userClassificationGenresView = (GenresView)FeedbackView.FindName("UserClassificationGenresView");
            UserClassificationGenresViewModel = (GenresViewModel)userClassificationGenresView.DataContext;
            UserClassificationGenresViewModel.Init(userClassificationGenresView);

            FeedbackedScreenplays = new ObservableCollection<ScreenplayModel>();
            CheckedScreenplays = new List<BrowseModel>();
            CanAnswerSurveys = new List<bool>();
            CurrentOffset = -1;
            CanAnswerSurvey = false;
            CanGoToFirst = false;
            CanGoToPrevious = false;
            CanSubmit = false;
            CanGoToNext = true;
            CanGoToLast = true;
            ClassificationsText = string.Empty;
            GenreClassificationsText = string.Empty;
            SubGenre1ClassificationsText = string.Empty;
            SubGenre2ClassificationsText = string.Empty;
        }

        /// <summary>
        /// Refreshes and shows the view.
        /// </summary>
        public void RefreshView()
        {
            //ScreenplayModel feedbackedScreenplay = FeedbackedScreenplays[CurrentOffset];
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;

            // Obtains the collection of checked offsets
            CheckedScreenplays.Clear();
            foreach (BrowseModel browsedScreenplay in browseViewModel.BrowsedScreenplays)
                if (browsedScreenplay.IsChecked)
                    CheckedScreenplays.Add(browsedScreenplay);

            // Obtains the collection of feedbacked screenplays
            FeedbackedScreenplays.Clear();
            CanAnswerSurveys.Clear();
            foreach (ClassificationModel classificationReport in ClassificationViewModel.ClassifiedScreenplays)
            {
                FeedbackedScreenplays.Add(classificationReport.Screenplay);
                CanAnswerSurveys.Add(true);
            }

            GoToFirstCommand.Execute(null);

            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Visible);
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
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesViewModel archivesViewModel = (ArchivesViewModel)ClassificationViewModel.MainViewModel.ArchivesView.DataContext;

            foreach (BrowseModel checkedScreenplay in CheckedScreenplays)
                browseViewModel.BrowsedScreenplays.Remove(checkedScreenplay);

            foreach (ClassificationModel report in ClassificationViewModel.ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(report);
                archivesViewModel.Screenplays.Add(report.Screenplay);
            }

            archivesViewModel.Screenplays = archivesViewModel.Screenplays; // Triggers PropertyChanged event

            CanSubmit = false;
        }
    }
}