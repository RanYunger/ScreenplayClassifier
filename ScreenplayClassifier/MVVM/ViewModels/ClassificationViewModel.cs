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
        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private bool browseComplete, progressComplete, classificationComplete;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        public BrowseViewModel BrowseViewModel { get; private set; }
        public ProgressViewModel ProgressViewModel { get; private set; }
        public FeedbackViewModel FeedbackViewModel { get; private set; }

        public ObservableCollection<ClassificationModel> ClassifiedScreenplays
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
                    ProgressViewModel.RefreshView();
                    ProgressViewModel.StartClassification(BrowseViewModel.CheckedScreenplays);
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

                    //BrowseViewModel.CanChoose = true;
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
                    BrowseViewModel.RefreshView();
                    ProgressViewModel.RefreshView();
                    FeedbackViewModel.HideView();

                    BrowseComplete = false;
                    ProgressComplete = false;
                }
                else if (FeedbackViewModel != null)
                {
                    FeedbackViewModel.RefreshView();
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
            ClassifiedScreenplays = new ObservableCollection<ClassificationModel>();
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

            MainViewModel = mainViewModel;
            ClassificationView = classificationView;

            browseView = (BrowseView)ClassificationView.FindName("BrowseView");
            BrowseViewModel = (BrowseViewModel)browseView.DataContext;
            BrowseViewModel.Init(this, browseView);

            progressView = (ProgressView)ClassificationView.FindName("ProgressView");
            ProgressViewModel = (ProgressViewModel)progressView.DataContext;
            ProgressViewModel.Init(this, progressView);

            feedbackView = (FeedbackView)ClassificationView.FindName("FeedbackView");
            FeedbackViewModel = (FeedbackViewModel)feedbackView.DataContext;
            FeedbackViewModel.Init(this, feedbackView);
        }
    }
}