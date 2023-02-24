﻿using LiveCharts;
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
    class ReportsViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> reports;
        private SeriesCollection percentageSeries;
        private string screenplayTitle;
        private int selectedReport;
        private bool canGoToFirst, canGoToPrevious, canGoToNext, canGoToLast;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }

        public ReportsView ReportsView { get; private set; }

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

        public SeriesCollection PercentageSeries
        {
            get { return percentageSeries; }
            set
            {
                percentageSeries = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PercentageSeries"));
            }
        }

        public string ScreenplayTitle
        {
            get { return screenplayTitle; }
            set
            {
                screenplayTitle = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayTitle"));
            }
        }

        public int SelectedReport
        {
            get { return selectedReport; }
            set
            {
                selectedReport = value;

                if (selectedReport != -1)
                    RefreshReportCommand.Execute(null);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedReport"));
            }
        }

        public bool CanGoToFirst
        {
            get { return canGoToFirst; }
            set
            {
                canGoToFirst = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToFirst"));
            }
        }

        public bool CanGoToPrevious
        {
            get { return canGoToPrevious; }
            set
            {
                canGoToPrevious = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToPrevious"));
            }
        }

        public bool CanGoToNext
        {
            get { return canGoToNext; }
            set
            {
                canGoToNext = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToNext"));
            }
        }

        public bool CanGoToLast
        {
            get { return canGoToLast; }
            set
            {
                canGoToLast = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToLast"));
            }
        }

        // Constructors
        public ReportsViewModel() { }

        // Methods
        #region Commands
        public Command RefreshReportCommand
        {
            get
            {
                ReportView reportView = (ReportView)ReportsView.FindName("ReportView");

                return new Command(() =>
                {
                    if (Reports.Count > 0)
                    {
                        ((ReportViewModel)reportView.DataContext).Init(reportView, Reports[SelectedReport].Screenplay, false);
                        ScreenplayTitle = Reports[SelectedReport].Screenplay.Title;
                        RefreshPieChart();
                    }
                });
            }
        }

        public Command GoToFirstCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedReport = 0;

                    CanGoToFirst = false;
                    CanGoToPrevious = false;
                    CanGoToNext = Reports.Count > 1;
                    CanGoToLast = Reports.Count > 1;
                });
            }
        }

        public Command GoToPreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedReport = SelectedReport - 1 <= 0 ? 0 : SelectedReport - 1;

                    if (SelectedReport == 0)
                    {
                        CanGoToFirst = false;
                        CanGoToPrevious = false;
                    }
                    CanGoToNext = true;
                    CanGoToLast = true;
                });
            }
        }

        public Command GoToNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedReport = SelectedReport + 1 >= Reports.Count - 1 ? Reports.Count - 1 : SelectedReport + 1;

                    CanGoToFirst = true;
                    CanGoToPrevious = true;
                    if (SelectedReport == Reports.Count - 1)
                    {
                        CanGoToNext = false;
                        CanGoToLast = false;
                    }
                });
            }
        }

        public Command GoToLastCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedReport = Reports.Count - 1;

                    CanGoToFirst = Reports.Count > 1;
                    CanGoToPrevious = Reports.Count > 1;
                    CanGoToNext = false;
                    CanGoToLast = false;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportsView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        /// <param name="user">The user who authenticated to the system</param>
        public void Init(ReportsView reportsView, MainViewModel mainViewModel, UserModel user)
        {
            MainViewModel = mainViewModel;
            ReportsView = reportsView;

            InitReports(user);
            ScreenplayTitle = string.Empty;
            SelectedReport = -1;

            CanGoToFirst = false;
            CanGoToPrevious = false;
            CanGoToNext = true;
            CanGoToLast = true;
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

        /// <summary>
        /// Refreshes the pie chart.
        /// </summary>
        private void RefreshPieChart()
        {
            float decimalPercentage;
            string textualPercentage;

            PercentageSeries = new SeriesCollection();

            // Creates slice for each genre
            foreach (string genreName in JSON.LoadedGenres)
            {
                decimalPercentage = Reports[SelectedReport].Screenplay.GenrePercentages[genreName];
                if (decimalPercentage > 0)
                {
                    textualPercentage = decimalPercentage.ToString("0.00");
                    PercentageSeries.Add(new PieSeries()
                    {
                        Title = genreName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(double.Parse(textualPercentage)) },
                        FontSize = 20,
                        DataLabels = true
                    });
                }
            }
        }
    }
}