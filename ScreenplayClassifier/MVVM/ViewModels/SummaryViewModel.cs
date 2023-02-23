using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SummaryViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> classifiedScreenplays;
        private string titleText, accuracyColor;
        private double accuracyPercent;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public SummaryView SummaryView { get; private set; }
        public ClassificationViewModel ClassificationViewModel { get; private set; }

        public ObservableCollection<ReportModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (classifiedScreenplays.Count > 0)
                    TitleText = string.Format("{0} Review", ClassifiedScreenplays.Count == 1 ? ClassifiedScreenplays[0].Screenplay.Title : "Batch");

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
        public SummaryViewModel() { }

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                InspectionView inspectionView = null;

                return new Command(() =>
                {
                    // Validation
                    if (SummaryView == null)
                        return;

                    inspectionView = (InspectionView)SummaryView.FindName("InspectionView");
                    inspectionView.Visibility = Visibility.Visible;
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

        public void Init(SummaryView summaryView, ClassificationViewModel classificationViewModel)
        {
            SummaryView = summaryView;
            ClassificationViewModel = classificationViewModel;

            ClassifiedScreenplays = new ObservableCollection<ReportModel>();
            AccuracyPercent = 0.0;
            TitleText = string.Empty;
            AccuracyColor = string.Empty;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (SummaryView != null)
                App.Current.Dispatcher.Invoke(() => SummaryView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            InspectionView inspectionView = null;
            List<ScreenplayModel> screenplays = new List<ScreenplayModel>();
            int correctClassifications;

            ClassifiedScreenplays = ClassificationViewModel.FeedbackViewModel.FeedbackedScreenplays;
            foreach (ReportModel reportModel in ClassifiedScreenplays)
                screenplays.Add(reportModel.Screenplay);

            correctClassifications = screenplays.FindAll(s => s.IsClassifiedCorrectly).Count;
            AccuracyPercent = (100.0 * correctClassifications) / screenplays.Count;

            inspectionView = (InspectionView)SummaryView.FindName("InspectionView");
            ((InspectionViewModel)inspectionView.DataContext).Init(inspectionView, screenplays);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (SummaryView != null)
                App.Current.Dispatcher.Invoke(() => SummaryView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Notifies other modules of the completed classification process.
        /// </summary>
        private void UpdateModules()
        {
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesViewModel archivesViewModel = (ArchivesViewModel)ClassificationViewModel.MainViewModel.ArchivesView.DataContext;

            foreach (BrowseModel checkedScreenplay in browseViewModel.CheckedScreenplays)
                browseViewModel.BrowsedScreenplays.Remove(checkedScreenplay);

            foreach (ReportModel report in ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(report);
                archivesViewModel.Screenplays.Add(report.Screenplay);
            }

            // Triggers PropertyChanged events
            reportsViewModel.Reports = reportsViewModel.Reports;
            archivesViewModel.Screenplays = archivesViewModel.Screenplays;
        }
    }
}