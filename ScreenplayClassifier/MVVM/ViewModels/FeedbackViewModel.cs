﻿using ScreenplayClassifier.MVVM.Models;
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
        private ObservableCollection<string> screenplayTitles;
        private int screenplayOffset;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public ReportViewModel ReportViewModel { get; private set; }

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

        public ObservableCollection<string> ScreenplayTitles
        {
            get { return screenplayTitles; }
            set
            {
                screenplayTitles = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayTitles"));
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

        // Constructors
        public FeedbackViewModel() { }

        // Methods
        #region Commands
        public Command GoToNextCommand
        {
            get
            {
                ReportView reportView = null;

                return new Command(() =>
                {
                    MessageBoxResult proceedDecision;

                    // Validation
                    if (FeedbackView == null)
                        return;

                    if ((ScreenplayOffset >= 0) && (!FeedbackedScreenplays[ScreenplayOffset].Isfeedbacked))
                    {
                        MessageBoxHandler.Show("Complete feedback before proceeding", string.Empty, 5, MessageBoxImage.Error);
                        return;
                    }

                    proceedDecision = MessageBox.Show("Once confirmed, your classification cannot be changed.", "Are you sure?",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (proceedDecision == MessageBoxResult.Yes)
                    {
                        reportView = (ReportView)FeedbackView.FindName("ReportView");

                        ClassificationViewModel.FeedbackComplete = ++ScreenplayOffset == FeedbackedScreenplays.Count;
                        if (!ClassificationViewModel.FeedbackComplete)
                            ReportViewModel.Init(reportView, FeedbackedScreenplays[ScreenplayOffset], true);
                    }
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

            FeedbackedScreenplays = new ObservableCollection<ScreenplayModel>();
            ScreenplayTitles = new ObservableCollection<string>();

            reportView = (ReportView)FeedbackView.FindName("ReportView");
            ReportViewModel = (ReportViewModel)reportView.DataContext;
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
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ReportView reportView = (ReportView)FeedbackView.FindName("ReportView");

            ScreenplayTitles.Clear();
            foreach (ScreenplayModel feedbackedScreenplay in FeedbackedScreenplays)
                ScreenplayTitles.Add(feedbackedScreenplay.Title);

            ScreenplayOffset = 0;

            ((ReportViewModel)reportView.DataContext).Init(reportView, FeedbackedScreenplays[ScreenplayOffset], true);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (FeedbackView != null)
                App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Collapsed);
        }
    }
}