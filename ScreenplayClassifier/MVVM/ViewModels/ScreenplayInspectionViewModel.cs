using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ScreenplayInspectionViewModel : PropertyChangeNotifier
    {
        // Fields
        private ScreenplayModel screenplay;
        private SeriesCollection percentageSeries;

        // Properties
        public ScreenplayViewModel ScreenplayViewModel { get; private set; }
        public ScreenplayInspectionView ScreenplayInspectionView { get; private set; }

        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;

                NotifyPropertyChange();
            }
        }

        public SeriesCollection PercentageSeries
        {
            get { return percentageSeries; }
            set
            {
                percentageSeries = value;

                NotifyPropertyChange();
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
                textualPercentage = decimalPercentage.ToString("0.0");

                PercentageSeries.Add(new ColumnSeries()
                {
                    Fill = new ImageBrush(new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, genreName)))),
                    Title = genreName,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(double.Parse(textualPercentage)) },
                    FontSize = 15,
                    ColumnPadding = 10,
                    DataLabels = true
                });
            }
        }
    }
}