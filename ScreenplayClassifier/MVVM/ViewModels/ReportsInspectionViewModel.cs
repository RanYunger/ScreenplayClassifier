using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportsInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private SeriesCollection percentageSeries;
        private string screenplayTitle;
        private int selectedReport;
        private bool canGoToFirst, canGoToPrevious, canGoToNext, canGoToLast;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportsViewModel ReportsViewModel { get; private set; }
        public ReportsInspectionView ReportsInspectionView { get; private set; }


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
        public ReportsInspectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowSelectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ReportsInspectionView == null)
                        return;

                    HideView();
                    ReportsViewModel.ReportsSelectionViewModel.ShowView();
                });
            }
        }

        public Command RefreshReportCommand
        {
            get
            {
                ReportView reportView = (ReportView)ReportsViewModel.ReportsView.FindName("ReportView");

                return new Command(() =>
                {
                    if (ReportsViewModel.Reports.Count > 0)
                    {
                        ((ReportViewModel)reportView.DataContext).Init(reportView, ReportsViewModel.Reports[SelectedReport].Screenplay, false);
                        ScreenplayTitle = ReportsViewModel.Reports[SelectedReport].Screenplay.Title;
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
                    CanGoToNext = ReportsViewModel.Reports.Count > 1;
                    CanGoToLast = ReportsViewModel.Reports.Count > 1;
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
                    SelectedReport = SelectedReport + 1 >= ReportsViewModel.Reports.Count - 1
                        ? ReportsViewModel.Reports.Count - 1 : SelectedReport + 1;

                    CanGoToFirst = true;
                    CanGoToPrevious = true;
                    if (SelectedReport == ReportsViewModel.Reports.Count - 1)
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
                    SelectedReport = ReportsViewModel.Reports.Count - 1;

                    CanGoToFirst = ReportsViewModel.Reports.Count > 1;
                    CanGoToPrevious = ReportsViewModel.Reports.Count > 1;
                    CanGoToNext = false;
                    CanGoToLast = false;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportsInspectionView">The view to obtain controls from</param>
        /// <param name="reportsViewModel">The view model who manages the reports module</param>
        public void Init(ReportsInspectionView reportsInspectionView, ReportsViewModel reportsViewModel)
        {
            ReportsInspectionView = reportsInspectionView;
            ReportsViewModel = reportsViewModel;

            ScreenplayTitle = string.Empty;
            SelectedReport = -1;

            CanGoToFirst = false;
            CanGoToPrevious = false;
            CanGoToNext = true;
            CanGoToLast = true;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ReportsInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ReportsInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ReportsInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ReportsInspectionView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Refreshes the pie chart.
        /// </summary>
        private void RefreshPieChart()
        {
            ScreenplayModel selectedScreenplay = ReportsViewModel.Reports[SelectedReport].Screenplay;
            string[] genres = { selectedScreenplay.ModelGenre, selectedScreenplay.ModelSubGenre1, selectedScreenplay.ModelSubGenre2 };
            float decimalPercentage;
            string textualPercentage;

            PercentageSeries = new SeriesCollection();

            // Creates slice for each genre
            foreach (string genreName in genres)
            {
                decimalPercentage = selectedScreenplay.GenrePercentages[genreName];
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