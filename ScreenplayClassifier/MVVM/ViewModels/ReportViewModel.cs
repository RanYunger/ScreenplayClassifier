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
        private ReportModel classificationReport;
        private SeriesCollection percentageSeries;
        private string screenplayText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportView ReportView { get; set; }
        public GenresViewModel ModelGenresViewModel { get; private set; }
        public GenresViewModel UserGenresViewModel { get; private set; }

        public ReportModel ClassificationReport
        {
            get { return classificationReport; }
            set
            {
                classificationReport = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationReport"));
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
        /// <param name="classificationReport">The report to represent in the ReportView</param>
        /// <param name="reportView">The view to obtain controls from</param>
        public void Init(ReportModel classificationReport, ReportView reportView)
        {
            GenresView modelGenresView, userGenresView;

            ClassificationReport = classificationReport;
            ScreenplayText = File.ReadAllText(classificationReport.Screenplay.FilePath);

            ReportView = reportView;

            modelGenresView = (GenresView)ReportView.FindName("ModelGenresView");
            ModelGenresViewModel = (GenresViewModel)modelGenresView.DataContext;
            ModelGenresViewModel.Init(modelGenresView, ClassificationReport.Screenplay, "Model");

            userGenresView = (GenresView)ReportView.FindName("UserGenresView");
            UserGenresViewModel = (GenresViewModel)userGenresView.DataContext;
            UserGenresViewModel.Init(userGenresView, ClassificationReport.Screenplay, "User");

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
                decimalPercentage = ClassificationReport.Screenplay.GenrePercentages[genreName];
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
