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
    public class GenresInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private SeriesCollection percentageSeries;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public GenresViewModel GenresViewModel { get; private set; }
        public GenresInspectionView GenresInspectionView { get; private set; }

        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplay"));
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
        public GenresInspectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowOverviewViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (GenresInspectionView == null)
                        return;

                    HideView();
                    GenresViewModel.GenresOverviewViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Inits the model.
        /// </summary>
        /// <param name="genresInspectionView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show the genres of</param>
        /// <param name="genresViewModel">The view model which manages the genres view</param>
        public void Init(GenresInspectionView genresInspectionView, ScreenplayModel screenplay, GenresViewModel genresViewModel)
        {
            GenresInspectionView = genresInspectionView;
            GenresViewModel = genresViewModel;

            Screenplay = screenplay;

            RefreshBarChart();
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (GenresInspectionView != null)
                App.Current.Dispatcher.Invoke(() => GenresInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (GenresInspectionView != null)
                App.Current.Dispatcher.Invoke(() => GenresInspectionView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Refreshes the pie chart.
        /// </summary>
        private void RefreshBarChart()
        {
            float decimalPercentage;
            string textualPercentage;

            PercentageSeries = new SeriesCollection();

            // Creates a slice for each genre
            foreach (string genreName in Screenplay.GenrePercentages.Keys)
            {
                decimalPercentage = Screenplay.GenrePercentages[genreName];
                textualPercentage = decimalPercentage.ToString("0.00");

                PercentageSeries.Add(new ColumnSeries()
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