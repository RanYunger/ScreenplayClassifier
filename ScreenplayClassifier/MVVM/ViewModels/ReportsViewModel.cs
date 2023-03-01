using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> reports;

        public event PropertyChangedEventHandler PropertyChanged;

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

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationReports"));
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

            InitReports(user);

            reportsSelectionView = (ReportsSelectionView)ReportsView.FindName("ReportsSelectionView");
            ReportsSelectionViewModel = (ReportsSelectionViewModel)reportsSelectionView.DataContext;
            ReportsSelectionViewModel.Init(reportsSelectionView, this);

            reportsInspectionView = (ReportsInspectionView)ReportsView.FindName("ReportsInspectionView");
            ReportsInspectionViewModel = (ReportsInspectionViewModel)reportsInspectionView.DataContext;
            ReportsInspectionViewModel.Init(reportsInspectionView, this);
        }

        /// <summary>
        /// Initiates the reports.
        /// </summary>
        /// <param name="user">The user authenticated to the system</param>
        private void InitReports(UserModel user)
        {
            List<ReportModel> memberReports;

            if (user.Role == UserModel.UserRole.GUEST)
                Reports = new ObservableCollection<ReportModel>();
            else
            {
                Reports = new ObservableCollection<ReportModel>(JSON.LoadedReports);

                // Members can only view the reports they own
                if (user.Role == UserModel.UserRole.MEMBER)
                {
                    memberReports = new List<ReportModel>(Reports).FindAll(report => report.Owner.Username.Equals(user.Username));
                    Reports = new ObservableCollection<ReportModel>(memberReports);
                }
            }
        }
    }
}