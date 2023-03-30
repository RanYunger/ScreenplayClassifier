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

        public ClassificationBrowseViewModel ClassificationBrowseViewModel { get; private set; }
        public ClassificationProgressViewModel ClassificationProgressViewModel { get; private set; }
        public ClassificationFeedbackViewModel ClassificationFeedbackViewModel { get; private set; }
        public ClassificationOverviewViewModel ClassificationOverviewViewModel { get; private set; }
        public ClassificationInspectionViewModel ClassificationInspectionViewModel { get; private set; }

        public bool BrowseComplete
        {
            get { return browseComplete; }
            set
            {
                browseComplete = value;

                if (browseComplete)
                {
                    ClassificationBrowseViewModel.HideView();
                    ClassificationProgressViewModel.RefreshView();
                    ClassificationProgressViewModel.ShowView();
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
                    ClassificationProgressViewModel.HideView();
                    ClassificationFeedbackViewModel.RefreshView();
                    ClassificationFeedbackViewModel.ShowView();
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
                    ClassificationFeedbackViewModel.HideView();
                    ClassificationOverviewViewModel.RefreshView();
                    ClassificationInspectionViewModel.RefreshView();
                    ClassificationOverviewViewModel.ShowView();
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
                    ClassificationOverviewViewModel.HideView();
                    ClassificationInspectionViewModel.HideView();
                    ClassificationBrowseViewModel.RefreshView();
                    ClassificationBrowseViewModel.ShowView();

                    BrowseComplete = false;
                    ProgressComplete = false;
                    FeedbackComplete = false;
                }
                else if (ClassificationOverviewViewModel != null)
                {
                    ClassificationOverviewViewModel.HideView();
                    ClassificationInspectionViewModel.HideView();

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
        public Command ShowHomeViewCommand
        {
            get { return new Command(() => MainViewModel.UserToolbarViewModel.ShowHomeViewCommand.Execute(null)); }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(ClassificationView classificationView, MainViewModel mainViewModel)
        {
            ClassificationBrowseView classificationBrowseView = null;
            ClassificationProgressView classificationProgressView = null;
            ClassificationFeedbackView classificationFeedbackView = null;
            ClassificationOverviewView classificationOverviewView = null;
            ClassificationInspectionView classificationInspectionView = null;

            ClassificationView = classificationView;
            MainViewModel = mainViewModel;

            classificationBrowseView = (ClassificationBrowseView)ClassificationView.FindName("ClassificationBrowseView");
            ClassificationBrowseViewModel = (ClassificationBrowseViewModel)classificationBrowseView.DataContext;
            ClassificationBrowseViewModel.Init(classificationBrowseView, this);

            classificationProgressView = (ClassificationProgressView)ClassificationView.FindName("ClassificationProgressView");
            ClassificationProgressViewModel = (ClassificationProgressViewModel)classificationProgressView.DataContext;
            ClassificationProgressViewModel.Init(classificationProgressView, this);

            classificationFeedbackView = (ClassificationFeedbackView)ClassificationView.FindName("ClassificationFeedbackView");
            ClassificationFeedbackViewModel = (ClassificationFeedbackViewModel)classificationFeedbackView.DataContext;
            ClassificationFeedbackViewModel.Init(classificationFeedbackView, this);

            classificationOverviewView = (ClassificationOverviewView)ClassificationView.FindName("ClassificationOverviewView");
            ClassificationOverviewViewModel = (ClassificationOverviewViewModel)classificationOverviewView.DataContext;
            ClassificationOverviewViewModel.Init(classificationOverviewView, this);

            classificationInspectionView = (ClassificationInspectionView)ClassificationView.FindName("ClassificationInspectionView");
            ClassificationInspectionViewModel = (ClassificationInspectionViewModel)classificationInspectionView.DataContext;
            ClassificationInspectionViewModel.Init(classificationInspectionView, this);
        }
    }
}