using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ScreenplayOverviewViewModel : PropertyChangeNotifier
    {
        // Fields
        private ScreenplayModel screenplay;
        private SeriesCollection percentageSeries;
        private ObservableCollection<string> ownerGenreOptions, ownerSubGenre1Options, ownerSubGenre2Options;
        private ImageSource ownerGenreImage, ownerSubGenre1Image, ownerSubGenre2Image;
        private string screenplayContent, selectedOwnerGenre, selectedOwnerSubGenre1, selectedOwnerSubGenre2;
        private bool canGiveFeedback;

        // Properties
        public ScreenplayViewModel ScreenplayViewModel { get; private set; }
        public ScreenplayOverviewView ScreenplayOverviewView { get; private set; }

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

        public ObservableCollection<string> OwnerGenreOptions
        {
            get { return ownerGenreOptions; }
            set
            {
                ownerGenreOptions = value;

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<string> OwnerSubGenre1Options
        {
            get { return ownerSubGenre1Options; }
            set
            {
                ownerSubGenre1Options = value;

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<string> OwnerSubGenre2Options
        {
            get { return ownerSubGenre2Options; }
            set
            {
                ownerSubGenre2Options = value;

                NotifyPropertyChange();
            }
        }

        public ImageSource OwnerGenreImage
        {
            get { return ownerGenreImage; }
            set
            {
                ownerGenreImage = value;

                NotifyPropertyChange();
            }
        }

        public ImageSource OwnerSubGenre1Image
        {
            get { return ownerSubGenre1Image; }
            set
            {
                ownerSubGenre1Image = value;

                NotifyPropertyChange();
            }
        }

        public ImageSource OwnerSubGenre2Image
        {
            get { return ownerSubGenre2Image; }
            set
            {
                ownerSubGenre2Image = value;

                NotifyPropertyChange();
            }
        }

        public string ScreenplayContent
        {
            get { return screenplayContent; }
            set
            {
                screenplayContent = value;

                NotifyPropertyChange();
            }
        }

        public string SelectedOwnerGenre
        {
            get { return selectedOwnerGenre; }
            set
            {
                selectedOwnerGenre = value;

                if (!string.Equals(selectedOwnerGenre, string.Empty))
                {
                    Screenplay.OwnerGenre = selectedOwnerGenre;

                    OwnerGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerGenre)));

                    // Restores options to their default collection
                    OwnerSubGenre1Options = new ObservableCollection<string>(CONFIGURATIONS.Genres);
                    OwnerSubGenre2Options = new ObservableCollection<string>(CONFIGURATIONS.Genres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerSubGenre1Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre1Options.Remove(selectedOwnerSubGenre2);
                    OwnerSubGenre1Options = OwnerSubGenre1Options;

                    OwnerSubGenre2Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre2Options.Remove(selectedOwnerSubGenre1);
                    OwnerSubGenre2Options = OwnerSubGenre2Options;
                }

                NotifyPropertyChange();
            }
        }

        public string SelectedOwnerSubGenre1
        {
            get { return selectedOwnerSubGenre1; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(CONFIGURATIONS.Genres);

                selectedOwnerSubGenre1 = value;

                if (!string.Equals(selectedOwnerSubGenre1, string.Empty))
                {
                    Screenplay.OwnerSubGenre1 = selectedOwnerSubGenre1;

                    OwnerSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerSubGenre1)));

                    // Restores options to their default collection
                    OwnerGenreOptions = new ObservableCollection<string>(CONFIGURATIONS.Genres);
                    OwnerSubGenre2Options = new ObservableCollection<string>(CONFIGURATIONS.Genres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre1);
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre2);
                    OwnerGenreOptions = OwnerGenreOptions;

                    OwnerSubGenre2Options.Remove(selectedOwnerSubGenre1);
                    OwnerSubGenre2Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre2Options = OwnerSubGenre2Options;
                }

                NotifyPropertyChange();
            }
        }

        public string SelectedOwnerSubGenre2
        {
            get { return selectedOwnerSubGenre2; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(CONFIGURATIONS.Genres);

                selectedOwnerSubGenre2 = value;

                if (!string.Equals(selectedOwnerSubGenre2, string.Empty))
                {
                    Screenplay.OwnerSubGenre2 = selectedOwnerSubGenre2;

                    OwnerSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerSubGenre2)));

                    // Restores options to their default collection
                    OwnerGenreOptions = new ObservableCollection<string>(CONFIGURATIONS.Genres);
                    OwnerSubGenre1Options = new ObservableCollection<string>(CONFIGURATIONS.Genres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre2);
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre1);
                    OwnerGenreOptions = OwnerGenreOptions;

                    OwnerSubGenre1Options.Remove(selectedOwnerSubGenre2);
                    OwnerSubGenre1Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre1Options = OwnerSubGenre1Options;
                }

                NotifyPropertyChange();
            }
        }

        public bool CanGiveFeedback
        {
            get { return canGiveFeedback; }
            set
            {
                canGiveFeedback = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ScreenplayOverviewViewModel() { }

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ScreenplayOverviewView == null)
                        return;

                    HideView();
                    ScreenplayViewModel.ScreenplayInspectionViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="screenplayOverviewView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show the genres of</param>
        /// <param name="canFeedback">The indication whether the user can give feedback</param>
        /// <param name="screenplayViewModel">The view model which manages the screenplay view</param>
        public void Init(ScreenplayOverviewView screenplayOverviewView, ScreenplayModel screenplay, bool canFeedback,
            ScreenplayViewModel screenplayViewModel)
        {
            ScreenplayOverviewView = screenplayOverviewView;
            ScreenplayViewModel = screenplayViewModel;

            Screenplay = screenplay;

            CanGiveFeedback = canFeedback;

            RefreshPieChart();

            OwnerGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanGiveFeedback ?
                "Unknown" : Screenplay.OwnerGenre)));
            OwnerSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanGiveFeedback ?
                "Unknown" : Screenplay.OwnerSubGenre1)));
            OwnerSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanGiveFeedback ?
                "Unknown" : Screenplay.OwnerSubGenre2)));

            ScreenplayContent = File.ReadAllText(Screenplay.FilePath);

            SelectedOwnerGenre = CanGiveFeedback ? screenplay.OwnerGenre : string.Empty;
            SelectedOwnerSubGenre1 = CanGiveFeedback ? screenplay.OwnerSubGenre1 : string.Empty;
            SelectedOwnerSubGenre2 = CanGiveFeedback ? screenplay.OwnerSubGenre2 : string.Empty;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ScreenplayOverviewView != null)
                App.Current.Dispatcher.Invoke(() => ScreenplayOverviewView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ScreenplayOverviewView != null)
                App.Current.Dispatcher.Invoke(() => ScreenplayOverviewView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Refreshes the pie chart.
        /// </summary>
        private void RefreshPieChart()
        {
            string[] genres = { Screenplay.ModelGenre, Screenplay.ModelSubGenre1, Screenplay.ModelSubGenre2 };
            float decimalPercentage;
            string textualPercentage;

            PercentageSeries = new SeriesCollection();

            // Creates a slice for each genre
            foreach (string genreName in genres)
            {
                decimalPercentage = Screenplay.GenrePercentages[genreName];
                textualPercentage = decimalPercentage.ToString("0.00");

                PercentageSeries.Add(new PieSeries()
                {
                    Fill = new ImageBrush(new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, genreName)))),
                    Title = genreName,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(double.Parse(textualPercentage)) },
                    FontSize = 30,
                    Foreground = Brushes.Black,
                    DataLabels = true
                });
            }
        }
    }
}