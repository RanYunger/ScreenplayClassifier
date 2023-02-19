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
        private ObservableCollection<ReportModel> classifiedScreenplays;
        private bool browseComplete, progressComplete, classificationComplete;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        public BrowseViewModel BrowseViewModel { get; private set; }
        public ProgressViewModel ProgressViewModel { get; private set; }
        public FeedbackViewModel FeedbackViewModel { get; private set; }

        public ObservableCollection<ReportModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
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
                    BrowseViewModel.HideView();
                    ProgressViewModel.RefreshView(BrowseViewModel.CheckedScreenplays);
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

        public bool ClassificationComplete
        {
            get { return classificationComplete; }
            set
            {
                classificationComplete = value;

                if (classificationComplete)
                {
                    FeedbackViewModel.HideView();
                    BrowseViewModel.RefreshView();
                    BrowseViewModel.ShowView();

                    BrowseComplete = false;
                    ProgressComplete = false;
                }
                else if (FeedbackViewModel != null)
                {
                    FeedbackViewModel.HideView();

                    BrowseComplete = true;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationComplete"));
            }
        }

        // Constructors
        public ClassificationViewModel()
        {
            ClassifiedScreenplays = new ObservableCollection<ReportModel>();
            BrowseComplete = false;
            ProgressComplete = false;
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
        }
    }
}