using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationViewModel : INotifyPropertyChanged
    {
        // Fields
        private bool browseComplete, progressComplete, feedbackComplete, classificationComplete;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        public BrowseViewModel BrowseViewModel { get; private set; }
        public ProgressViewModel ProgressViewModel { get; private set; }
        public FeedbackViewModel FeedbackViewModel { get; private set; }
        public OverviewViewModel OverviewViewModel { get; private set; }
        public InspectionViewModel InspectionViewModel { get; private set; }


        public bool BrowseComplete
        {
            get { return browseComplete; }
            set
            {
                browseComplete = value;

                if (browseComplete)
                {
                    BrowseViewModel.HideView();
                    ProgressViewModel.RefreshView();
                    ProgressViewModel.ShowView();
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowseComplete"));
            }
        }

        public bool ProgressComplete
        {
            get { return progressComplete; }
            set
            {
                progressComplete = value;

                if (progressComplete)
                {
                    ProgressViewModel.HideView();
                    FeedbackViewModel.RefreshView();
                    FeedbackViewModel.ShowView();
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

                if (feedbackComplete)
                {
                    FeedbackViewModel.HideView();
                    OverviewViewModel.RefreshView();
                    InspectionViewModel.RefreshView();
                    OverviewViewModel.ShowView();
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FeedbackComplete"));
            }
        }

        public bool ClassificationComplete
        {
            get { return classificationComplete; }
            set
            {
                classificationComplete = value;

                if (classificationComplete)
                {
                    OverviewViewModel.HideView();
                    InspectionViewModel.HideView();
                    BrowseViewModel.RefreshView();
                    BrowseViewModel.ShowView();

                    BrowseComplete = false;
                    ProgressComplete = false;
                    FeedbackComplete = false;
                }
                else if (OverviewViewModel != null)
                {
                    OverviewViewModel.HideView();
                    InspectionViewModel.HideView();

                    BrowseComplete = true;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationComplete"));
            }
        }

        // Constructors
        public ClassificationViewModel()
        {
            BrowseComplete = false;
            ProgressComplete = false;
            FeedbackComplete = false;
            ClassificationComplete = false;
        }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(ClassificationView classificationView, MainViewModel mainViewModel)
        {
            BrowseView browseView = null;
            ProgressView progressView = null;
            FeedbackView feedbackView = null;
            OverviewView overviewView = null;
            InspectionView inspectionView = null;

            ClassificationView = classificationView;
            MainViewModel = mainViewModel;

            browseView = (BrowseView)ClassificationView.FindName("BrowseView");
            BrowseViewModel = (BrowseViewModel)browseView.DataContext;
            BrowseViewModel.Init(browseView, this);

            progressView = (ProgressView)ClassificationView.FindName("ProgressView");
            ProgressViewModel = (ProgressViewModel)progressView.DataContext;
            ProgressViewModel.Init(progressView, this);

            feedbackView = (FeedbackView)ClassificationView.FindName("FeedbackView");
            FeedbackViewModel = (FeedbackViewModel)feedbackView.DataContext;
            FeedbackViewModel.Init(feedbackView, this);

            overviewView = (OverviewView)ClassificationView.FindName("OverviewView");
            OverviewViewModel = (OverviewViewModel)overviewView.DataContext;
            OverviewViewModel.Init(overviewView, this);

            inspectionView = (InspectionView)ClassificationView.FindName("InspectionView");
            InspectionViewModel = (InspectionViewModel)inspectionView.DataContext;
            InspectionViewModel.Init(inspectionView, this);
        }
    }
}