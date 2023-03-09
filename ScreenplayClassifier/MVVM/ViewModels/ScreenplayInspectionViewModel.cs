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
    public class ScreenplayInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private SeriesCollection percentageSeries;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ScreenplayViewModel ScreenplayViewModel { get; private set; }
        public ScreenplayInspectionView ScreenplayInspectionView { get; private set; }

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
        public ScreenplayInspectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowOverviewViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ScreenplayInspectionView == null)
                        return;

                    HideView();
                    ScreenplayViewModel.ScreenplayOverviewViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Inits the model.
        /// </summary>
        /// <param name="screenplayInspectionView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show the genres of</param>
        /// <param name="screenplayViewModel">The view model which manages the screenplay view</param>
        public void Init(ScreenplayInspectionView screenplayInspectionView, ScreenplayModel screenplay, ScreenplayViewModel screenplayViewModel)
        {
            ScreenplayInspectionView = screenplayInspectionView;
            ScreenplayViewModel = screenplayViewModel;

            Screenplay = screenplay;

            RefreshBarChart();
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ScreenplayInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ScreenplayInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ScreenplayInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ScreenplayInspectionView.Visibility = Visibility.Collapsed);
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
                    FontSize = 15,
                    ColumnPadding = 30,
                    DataLabels = true
                });
            }
        }
    }
}