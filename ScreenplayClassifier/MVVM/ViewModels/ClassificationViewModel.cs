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
        private bool browseComplete, progerssComplete, resultsComplete;
        private BrowseViewModel browseViewModel;
        private ProgressViewModel progressViewModel;
        private ResultsViewModel resultsViewModel;
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

        public bool BrowseComplete
        {
            get { return browseComplete; }
            set
            {
                browseComplete = value;

                if (browseComplete)
                {
                    int batchSize = BrowseViewModel.BrowsedScreenplays.Count < 5 ? BrowseViewModel.BrowsedScreenplays.Count : 5;

                    progressViewModel.DurationTimer.Start();
                    progressViewModel.ClassificationsRequired = BrowseViewModel.BrowsedScreenplays.Count;
                    progressViewModel.ClassificationsComplete = 0;

                    foreach (ScreenplayModel screenplay in BrowseViewModel.BrowsedScreenplays)
                        ProgressViewModel.InactiveClassifications.Add(new ClassificationModel(screenplay));
                    for (int i = 0; i < batchSize; i++)
                    {
                        ProgressViewModel.ActiveClassifications.Add(progressViewModel.InactiveClassifications[0]);
                        ProgressViewModel.InactiveClassifications.RemoveAt(0);

                        ProgressViewModel.ClassificationsProgresses[i].BackgroundWorker.RunWorkerAsync();
                        ProgressViewModel.ClassificationsProgresses[i].DurationTimer.Start();
                    }

                    ChangeTabVisibility("ProgressTabItem", Visibility.Visible, true);
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
                BrowseComplete = !progerssComplete;

                if (progerssComplete)
                    ChangeTabVisibility("ResultsTabItem", Visibility.Visible, false);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProgressComplete"));
            }
        }
        public bool ResultsComplete
        {
            get { return resultsComplete; }
            set
            {
                resultsComplete = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ResultsComplete"));
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
