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
                browseComplete = value;

                if (browseComplete)
                {
                    ProgressViewModel.ProgressView.Visibility = Visibility.Visible;
                    ProgressViewModel.Set(BrowseViewModel.BrowsedScreenplays);
                }
                else
                {
                    BrowseViewModel.Reset();
                    ProgressViewModel.Reset();
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

                //if (progerssComplete)
                //{
                //    ResultsViewModel.Set();
                //    FeedbackViewModel.Set(ResultsViewModel.ClassifiedScreenplays);
                //}
                //else
                //{
                //    ProgressViewModel.Reset();
                //    ResultsViewModel.Reset();
                //    FeedbackViewModel.Reset();
                //}

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

                //if (!feedbackComplete)
                //{
                //    BrowseComplete = false;
                //    ProgressComplete = false;
                //}

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
            //ResultsView resultsView = null;
            //FeedbackView feedbackView = null;

            MainViewModel = mainViewModel;
            ClassificationView = classificationView;

            browseView = (BrowseView)ClassificationView.FindName("BrowseView");
            BrowseViewModel = (BrowseViewModel)browseView.DataContext;
            BrowseViewModel.Init(this, browseView);

            progressView = (ProgressView)ClassificationView.FindName("ProgressView");
            ProgressViewModel = (ProgressViewModel)progressView.DataContext;
            ProgressViewModel.Init(this, progressView);

            //resultsView = (ResultsView)ClassificationView.FindName("ResultsView");
            //ResultsViewModel = (ResultsViewModel)resultsView.DataContext;
            //ResultsViewModel.Init(this, resultsView);

            //feedbackView = (FeedbackView)ClassificationView.FindName("FeedbackView");
            //FeedbackViewModel = (FeedbackViewModel)feedbackView.DataContext;
            //FeedbackViewModel.Init(this, feedbackView);
        }
    }
}
