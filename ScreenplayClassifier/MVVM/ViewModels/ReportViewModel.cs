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
        private ClassificationModel classificationReport;
        private SeriesCollection percentageSeries;
        private string screenplayText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportView ReportView { get; set; }
        public GenresViewModel PredictedGenresViewModel { get; private set; }
        public GenresViewModel ActualGenresViewModel { get; private set; }

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

        public string ScreenplayText
        {
            get { return screenplayText; }
            set
            {
                screenplayText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreebplayText"));
            }
        }

        // Constructors
        public ReportViewModel() { }

        // Methods
        #region Commands
        public Command ReadScreenplayCommand
        {
            get
            {
                return new Command(() =>
                {
                    ScreenplayView screenplayView = null;
                    ScreenplayViewModel screenplayViewModel = null;

                    // Finds an existing ScreenplayView (if there's one)
                    foreach (Window view in App.Current.Windows)
                        if (view is ScreenplayView)
                        {
                            screenplayViewModel = (ScreenplayViewModel)view.DataContext;

                            if (string.Equals(screenplayViewModel.FilePath, ClassificationReport.Screenplay.FilePath))
                            {
                                view.Focus();
                                return;
                            }
                            else
                            {
                                view.Close();
                                break;
                            }
                        }

                    // Shows the screenplay in a new ScreenplayView
                    screenplayView = new ScreenplayView();
                    ((ScreenplayViewModel)screenplayView.DataContext).Init(ClassificationReport.Screenplay.FilePath);

                    screenplayView.Show();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationReport">The report to represent in the ReportView</param>
        /// <param name="reportView">The view to obtain controls from</param>
        public void Init(ClassificationModel classificationReport, ReportView reportView)
        {
            GenresView predictedGenresView, actualGenresView;

            ClassificationReport = classificationReport;
            ScreenplayText = File.ReadAllText(classificationReport.Screenplay.FilePath);

            ReportView = reportView;

            predictedGenresView = (GenresView)ReportView.FindName("PredictedGenresView");
            PredictedGenresViewModel = (GenresViewModel)predictedGenresView.DataContext;
            PredictedGenresViewModel.Init(predictedGenresView);
            PredictedGenresViewModel.RefreshView(ClassificationReport.Screenplay, "Predicted");

            actualGenresView = (GenresView)ReportView.FindName("ActualGenresView");
            ActualGenresViewModel = (GenresViewModel)actualGenresView.DataContext;
            ActualGenresViewModel.Init(actualGenresView);
            ActualGenresViewModel.RefreshView(ClassificationReport.Screenplay, "Actual");

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
