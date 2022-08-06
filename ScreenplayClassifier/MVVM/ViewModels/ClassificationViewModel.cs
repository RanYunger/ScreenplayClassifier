using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationViewModel : INotifyPropertyChanged
    {
        // Fields
        private BrowseViewModel browseViewModel;
        private ProgressViewModel progressViewModel;
        private ResultsViewModel resultsViewModel;
        private FeedbackViewModel feedbackViewModel;
        private bool browseComplete, progerssComplete, feedbackComplete;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        public BrowseViewModel BrowseViewModel
        {
            get { return browseViewModel; }
            set
            {
                browseViewModel = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowseViewModel"));
            }
        }
        public ProgressViewModel ProgressViewModel
        {
            get { return progressViewModel; }
            set
            {
                progressViewModel = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProgressViewModel"));
            }
        }
        public ResultsViewModel ResultsViewModel
        {
            get { return resultsViewModel; }
            set
            {
                resultsViewModel = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ResultsViewModel"));
            }
        }

        public FeedbackViewModel FeedbackViewModel
        {
            get { return feedbackViewModel; }
            set
            {
                feedbackViewModel = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FeedbackViewModel"));
            }
        }

        public bool BrowseComplete
        {
            get { return browseComplete; }
            set
            {
                int batchSize = BrowseViewModel.BrowsedScreenplays.Count < 5 ? BrowseViewModel.BrowsedScreenplays.Count : 5;

                browseComplete = value;

                if (browseComplete)
                {
                    ProgressViewModel.DurationTimer.Start();
                    ProgressViewModel.ClassificationsRequired = BrowseViewModel.BrowsedScreenplays.Count;
                    ProgressViewModel.ClassificationsComplete = 0;

                    foreach (ScreenplayModel screenplay in BrowseViewModel.BrowsedScreenplays)
                        ProgressViewModel.InactiveClassifications.Add(new ClassificationModel(screenplay));
                    for (int i = 0; i < batchSize; i++)
                    {
                        ProgressViewModel.ActiveClassifications.Add(progressViewModel.InactiveClassifications[0]);
                        ProgressViewModel.ClassificationsProgresses.Add(new ProgressModel());
                        ProgressViewModel.InactiveClassifications.RemoveAt(0);

                        ProgressViewModel.ClassificationsProgresses[i].BackgroundWorker.RunWorkerAsync();
                        ProgressViewModel.ClassificationsProgresses[i].DurationTimer.Start();
                    }

                    ProgressViewModel.SelectedClassification = 0;

                    ChangeTabVisibility("ProgressTabItem", Visibility.Visible, true);
                }
                else
                {
                    BrowseViewModel.BrowsedScreenplays.Clear();
                    BrowseViewModel.CanBrowse = BrowseViewModel.CanClear = true;

                    ProgressViewModel.SelectedClassification = -1;

                    ChangeTabVisibility("BrowseTabItem", Visibility.Visible, true);
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowseComplete"));
            }
        }
        public bool ProgressComplete
        {
            get { return progerssComplete; }
            set
            {
                progerssComplete = value;

                if (progerssComplete)
                {
                    ResultsViewModel.SelectedClassifiedScreenplay = 0;

                    foreach (ClassificationModel classification in ResultsViewModel.ClassifiedScreenplays)
                        FeedbackViewModel.ClassifiedScreenplays.Add(classification);
                    FeedbackViewModel.SelectedClassifiedScreenplay = 0;

                    ChangeTabVisibility("ResultsTabItem", Visibility.Visible, false);
                    ChangeTabVisibility("FeedbackTabItem", Visibility.Visible, false);
                }
                else
                {
                    ProgressViewModel.ClassificationsProgresses.Clear();
                    ProgressViewModel.TotalDuration = TimeSpan.Zero;
                    ProgressViewModel.ClassificationsRequired = ProgressViewModel.ClassificationsComplete
                        = ProgressViewModel.ClassificationsProgress = 0;
                    ProgressViewModel.CanStop = ProgressViewModel.CanPause = true;

                    ResultsViewModel.ClassifiedScreenplays.Clear();
                    ResultsViewModel.SelectedClassifiedScreenplay = -1;

                    FeedbackViewModel.ClassifiedScreenplays.Clear();
                    FeedbackViewModel.SelectedClassifiedScreenplay = -1;

                    ChangeTabVisibility("ProgressTabItem", Visibility.Collapsed, false);
                    ChangeTabVisibility("ResultsTabItem", Visibility.Collapsed, false);
                    ChangeTabVisibility("FeedbackTabItem", Visibility.Collapsed, false);
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProgressComplete"));
            }
        }
        public bool FeedbackComplete
        {
            get { return feedbackComplete; }
            set
            {
                feedbackComplete = value;

                if (!feedbackComplete)
                {
                    FeedbackViewModel.SelectedClassifiedScreenplay = -1;

                    BrowseComplete = false;
                    ProgressComplete = false;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FeedbackComplete"));
            }
        }

        // Constructors
        public ClassificationViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ClassificationView classificationView, MainViewModel mainViewModel)
        {
            BrowseView browseView = null;
            ProgressView progressView = null;
            ResultsView resultsView = null;
            FeedbackView feedbackView = null;

            MainViewModel = mainViewModel;
            ClassificationView = classificationView;

            browseView = (BrowseView)ClassificationView.FindName("BrowseView");
            BrowseViewModel = (BrowseViewModel)browseView.DataContext;
            BrowseViewModel.Init(this, browseView);

            progressView = (ProgressView)ClassificationView.FindName("ProgressView");
            ProgressViewModel = (ProgressViewModel)progressView.DataContext;
            ProgressViewModel.Init(this, progressView);

            resultsView = (ResultsView)ClassificationView.FindName("ResultsView");
            ResultsViewModel = (ResultsViewModel)resultsView.DataContext;
            ResultsViewModel.Init(this, resultsView);

            feedbackView = (FeedbackView)ClassificationView.FindName("FeedbackView");
            FeedbackViewModel = (FeedbackViewModel)feedbackView.DataContext;
            FeedbackViewModel.Init(this, feedbackView);
        }

        private void ChangeTabVisibility(string tabName, Visibility visibility, bool focus)
        {
            TabControl tabControl = (TabControl)ClassificationView.FindName("TabControl");
            TabItem tabItem = (TabItem)tabControl.FindName(tabName);

            tabItem.IsSelected = focus;
            tabItem.Visibility = visibility;
        }
    }
}
