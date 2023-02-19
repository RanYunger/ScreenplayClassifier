using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        // Fields
        private ReportModel report;
        private SeriesCollection percentageSeries;
        private string screenplayText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportView ReportView { get; set; }
        public GenresViewModel GenresViewModel { get; private set; }

        public ReportModel Report
        {
            get { return report; }
            set
            {
                report = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Report"));
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

        public string ScreenplayText
        {
            get { return screenplayText; }
            set
            {
                screenplayText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("screenplayText"));
            }
        }

        // Constructors
        public ReportViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportView">The view to obtain controls from</param>
        /// <param name="report">The report to represent in the ReportView</param>
        /// <param name="canGiveFeedback">The indication whether the user can give feedback</param>
        public void Init(ReportView reportView, ReportModel report, bool canGiveFeedback)
        {
            GenresView genresView;

            ReportView = reportView;

            Report = report;
            ScreenplayText = File.ReadAllText(report.Screenplay.FilePath);

            genresView = (GenresView)ReportView.FindName("GenresView");
            GenresViewModel = (GenresViewModel)genresView.DataContext;
            GenresViewModel.Init(genresView, Report.Screenplay, canGiveFeedback);

            RefreshPieChart();
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
                decimalPercentage = Report.Screenplay.GenrePercentages[genreName];
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
