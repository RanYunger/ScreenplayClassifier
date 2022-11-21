using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        // Fields
        private ClassificationModel classificationReport;
        private SeriesCollection percentageSeries;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportView ReportView { get; set; }
        public GenresViewModel GenresViewModel { get; private set; }

        public ClassificationModel ClassificationReport
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

        // Constructors
        public ReportViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ClassificationModel classificationReport, ReportView reportView)
        {
            GenresView genresView;

            ClassificationReport = classificationReport;
            ReportView = reportView;

            genresView = (GenresView)ReportView.FindName("GenresView");
            GenresViewModel = (GenresViewModel)genresView.DataContext;
            GenresViewModel.Init(genresView);

            GenresViewModel.RefreshView(ClassificationReport.Screenplay, "Actual");

            RefreshPieChart();
        }

        private void RefreshPieChart()
        {
            float genrePercentage;

            PercentageSeries = new SeriesCollection();

            foreach (string genreName in JSON.LoadedGenres)
            {
                genrePercentage = ClassificationReport.Screenplay.MatchingPercentages[genreName];
                if (genrePercentage > 0)
                    PercentageSeries.Add(new PieSeries()
                    {
                        Title = genreName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(genrePercentage) },
                        DataLabels = false
                    });
            }
        }
    }
}
