using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportsViewModel : PropertyChangeNotifier
    {
        // Fields
        private ObservableCollection<ReportModel> reports;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ReportsView ReportsView { get; private set; }

        public ReportsSelectionViewModel ReportsSelectionViewModel { get; private set; }
        public ReportsInspectionViewModel ReportsInspectionViewModel { get; private set; }

        public ObservableCollection<ReportModel> Reports
        {
            get { return reports; }
            set
            {
                reports = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ReportsViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportsView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        /// <param name="user">The user who authenticated to the system</param>
        public void Init(ReportsView reportsView, MainViewModel mainViewModel, UserModel user)
        {
            ReportsSelectionView reportsSelectionView = null;
            ReportsInspectionView reportsInspectionView = null;

            ReportsView = reportsView;
            MainViewModel = mainViewModel;

            Reports = DATABASE.LoadReports(user);

            reportsSelectionView = (ReportsSelectionView)ReportsView.FindName("ReportsSelectionView");
            ReportsSelectionViewModel = (ReportsSelectionViewModel)reportsSelectionView.DataContext;
            ReportsSelectionViewModel.Init(reportsSelectionView, this);

            reportsInspectionView = (ReportsInspectionView)ReportsView.FindName("ReportsInspectionView");
            ReportsInspectionViewModel = (ReportsInspectionViewModel)reportsInspectionView.DataContext;
            ReportsInspectionViewModel.Init(reportsInspectionView, this);
        }

        /// <summary>
        /// Retrieves a report by its screenplay's title.
        /// </summary>
        /// <param name="screenplayTitle">The screenplay's title</param>
        /// <returns>The matching report (if exists), Null otherwise</returns>
        public ReportModel FindReportByTitle(string screenplayTitle)
        {
            foreach (ReportModel report in Reports)
                if (string.Equals(report.Screenplay.Title, screenplayTitle))
                    return report;

            return null;
        }
    }
}