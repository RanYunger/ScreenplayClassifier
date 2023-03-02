﻿using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationOverviewViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ScreenplayModel> classifiedScreenplays;
        private string titleText, accuracyColor;
        private double accuracyPercent;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ClassificationOverviewView ClassificationOverviewView { get; private set; }

        public ObservableCollection<ScreenplayModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (classifiedScreenplays.Count > 0)
                    TitleText = string.Format("{0} Review", ClassifiedScreenplays.Count == 1 ? ClassifiedScreenplays[0].Title : "Batch");

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TitleText"));
            }
        }

        public string AccuracyColor
        {
            get { return accuracyColor; }
            set
            {
                accuracyColor = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AccuracyColor"));
            }
        }

        public double AccuracyPercent
        {
            get { return accuracyPercent; }
            set
            {
                accuracyPercent = value;

                if ((accuracyPercent >= 0) && (AccuracyPercent < 33.3))
                    AccuracyColor = "Red";
                else if ((accuracyPercent >= 33.3) && (AccuracyPercent < 66.6))
                    AccuracyColor = "Orange";
                else if ((accuracyPercent >= 66.6) && (AccuracyPercent <= 100))
                    AccuracyColor = "Green";

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AccuracyPercent"));
            }
        }

        // Constructors
        public ClassificationOverviewViewModel() { }

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ClassificationOverviewView == null)
                        return;

                    HideView();
                    ClassificationViewModel.ClassificationInspectionViewModel.ShowView();
                });
            }
        }

        public Command RerunClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.ClassificationComplete = false); }
        }

        public Command CompleteClassificationCommand
        {
            get
            {
                return new Command(() =>
                {
                    UpdateModules();
                    ClassificationViewModel.ClassificationComplete = true;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="classificationOverviewView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(ClassificationOverviewView classificationOverviewView, ClassificationViewModel classificationViewModel)
        {
            ClassificationOverviewView = classificationOverviewView;
            ClassificationViewModel = classificationViewModel;

            ClassifiedScreenplays = new ObservableCollection<ScreenplayModel>();
            TitleText = string.Empty;
            AccuracyColor = string.Empty;
            AccuracyPercent = 0.0;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ClassificationOverviewView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationOverviewView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ClassificationInspectionView inspectionView = ClassificationViewModel.ClassificationInspectionViewModel.ClassificationInspectionView;
            int correctClassifications;

            ClassifiedScreenplays = ClassificationViewModel.ClassificationFeedbackViewModel.FeedbackedScreenplays;

            correctClassifications = new List<ScreenplayModel>(ClassifiedScreenplays).FindAll(s => s.IsClassifiedCorrectly).Count;
            AccuracyPercent = (100.0 * correctClassifications) / ClassifiedScreenplays.Count;

            ClassificationViewModel.ClassificationInspectionViewModel.Init(inspectionView, ClassificationViewModel);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationOverviewView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationOverviewView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Notifies other modules of the completed classification process.
        /// </summary>
        private void UpdateModules()
        {
            UserModel owner = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User;
            ClassificationBrowseViewModel classificationBrowseViewModel = ClassificationViewModel.ClassificationBrowseViewModel;
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesViewModel archivesViewModel = (ArchivesViewModel)ClassificationViewModel.MainViewModel.ArchivesView.DataContext;

            foreach (BrowseModel checkedScreenplay in classificationBrowseViewModel.CheckedScreenplays)
                classificationBrowseViewModel.BrowsedScreenplays.Remove(checkedScreenplay);

            foreach (ScreenplayModel screenplay in ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(new ReportModel(owner, screenplay));
                archivesViewModel.Screenplays.Add(screenplay);
            }

            // Triggers PropertyChanged events
            reportsViewModel.Reports = reportsViewModel.Reports;
            archivesViewModel.Screenplays = archivesViewModel.Screenplays;
        }
    }
}