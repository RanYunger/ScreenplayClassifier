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
        private List<int> checkedOffsets;
        private List<bool> hasAnsweredSurveys;
        private int currentOffset;
        private bool hasAnsweredSurvey, canGoToFirst, canGoToPrevious, canSubmitAll, canGoToNext, canGoToLast;

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

        public List<bool> HasAnsweredSurveys
        {
            get { return hasAnsweredSurveys; }
            set
            {
                hasAnsweredSurveys = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HasAnsweredSurveys"));
            }
        }

        public int CurrentOffset
        {
            get { return currentOffset; }
            set
            {
                BrowseViewModel browseViewModel = null;

                currentOffset = value;

                if (currentOffset != -1)
                {
                    browseViewModel = ClassificationViewModel.BrowseViewModel;
                    browseViewModel.SelectedScreenplay = CheckedOffsets[currentOffset];

                    RefreshView();
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentOffset"));
            }
        }

        public bool HasAnsweredSurvey
        {
            get { return hasAnsweredSurvey; }
            set
            {
                hasAnsweredSurvey = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HasAnsweredSurvey"));
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

        public bool CanSubmitAll
        {
            get { return canSubmitAll; }
            set
            {
                canSubmitAll = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanSubmitAll"));
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

        public Command AnswerSurveyCommand
        {
            get
            {
                RadioButton yesCorrectRadioButton = null, noCorrectRadioButton = null, yesReadRadioButton = null, noReadRadioButton = null;
                ScreenplayView screenplayView = null;
                ScreenplayViewModel screenplayViewModel = null;
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
                        MessageBoxHandler.Show("Choose whether the classification is correct.", string.Empty, 3, MessageBoxImage.Exclamation);
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
                        // Finds an existing ScreenplayView (if there's one)
                        foreach (Window view in App.Current.Windows)
                            if (view is ScreenplayView)
                            {
                                screenplayViewModel = (ScreenplayViewModel)view.DataContext;

                                if (string.Equals(screenplayViewModel.FilePath, feedbackedScreenplay.FilePath))
                                    view.Focus();
                                else
                                {
                                    // Shows the screenplay in a new ScreenplayView
                                    view.Close();

                                    screenplayView = new ScreenplayView();
                                    ((ScreenplayViewModel)screenplayView.DataContext).Init(feedbackedScreenplay.FilePath);

                                    screenplayView.Show();
                                }

                                break;
                            }
                    }

                    HasAnsweredSurveys[CurrentOffset] = true;

                    CurrentOffset = CurrentOffset; // Triggers PropertyChanged event
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

        public Command SubmitAllFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult reClassifyDecision;

                    // Validation
                    foreach (ClassificationModel classification in ClassificationViewModel.ClassifiedScreenplays)
                        if (!classification.Screenplay.Isfeedbacked)
                        {
                            MessageBoxHandler.Show("Complete feedback for " + classification.Screenplay.Title, string.Empty, 3,
                                MessageBoxImage.Exclamation);
                            return;
                        }

                    CanSubmitAll = true;

                    reClassifyDecision = MessageBox.Show("Would you like to re-classify this batch?", "Classification Complete",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (reClassifyDecision == MessageBoxResult.No)
                        UpdateOtherModules();

                    ClassificationViewModel.ClassificationComplete = false;

                    // TODO: COMPLETE

                    //else
                    //{
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
            CheckedOffsets = new List<int>();
            HasAnsweredSurveys = new List<bool>();
            CurrentOffset = -1;
            HasAnsweredSurvey = false;
            CanGoToFirst = false;
            CanGoToPrevious = false;
            CanSubmitAll = false;
            CanGoToNext = true;
            CanGoToLast = true;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;

            // Obtains the collection of checked offsets
            CheckedOffsets.Clear();
            for (int i = 0; i < browseViewModel.BrowsedScreenplays.Count; i++)
                if (browseViewModel.BrowsedScreenplays[i].IsChecked)
                    CheckedOffsets.Add(i);

            // Obtains the collection of feedbacked screenplays
            FeedbackedScreenplays.Clear();
            HasAnsweredSurveys.Clear();
            foreach (ClassificationModel classificationReport in ClassificationViewModel.ClassifiedScreenplays)
            {
                FeedbackedScreenplays.Add(classificationReport.Screenplay);
                HasAnsweredSurveys.Add(false);
            }

            GoToFirstCommand.Execute(null);

            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ScreenplayModel feedbackedScreenplay = FeedbackedScreenplays[CurrentOffset];

            ModelClassificationGenresViewModel.RefreshView(feedbackedScreenplay, "Model");
            UserClassificationGenresViewModel.RefreshView(feedbackedScreenplay, "User");

            HasAnsweredSurvey = HasAnsweredSurveys[CurrentOffset];
            if (!HasAnsweredSurvey)
                PrepareSurveyCommand.Execute(null);
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

            foreach (int checkedOffset in CheckedOffsets)
                browseViewModel.BrowsedScreenplays.RemoveAt(checkedOffset);

            foreach (ClassificationModel report in ClassificationViewModel.ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(report);
                archivesViewModel.Screenplays.Add(report.Screenplay);
            }

            archivesViewModel.Screenplays = archivesViewModel.Screenplays; // Triggers PropertyChanged event
        }
    }
}