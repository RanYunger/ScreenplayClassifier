using LiveCharts;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenresOverviewViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private SeriesCollection percentageSeries;
        private ObservableCollection<string> ownerGenreOptions, ownerSubGenre1Options, ownerSubGenre2Options;
        private ImageSource ownerGenreImage, ownerSubGenre1Image, ownerSubGenre2Image;
        private string selectedOwnerGenre, selectedOwnerSubGenre1, selectedOwnerSubGenre2;
        private bool canGiveFeedback;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public GenresViewModel GenresViewModel { get; private set; }
        public GenresOverviewView GenresOverviewView { get; private set; }

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

        public ObservableCollection<string> OwnerGenreOptions
        {
            get { return ownerGenreOptions; }
            set
            {
                ownerGenreOptions = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerGenreOptions"));
            }
        }

        public ObservableCollection<string> OwnerSubGenre1Options
        {
            get { return ownerSubGenre1Options; }
            set
            {
                ownerSubGenre1Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre1Options"));
            }
        }

        public ObservableCollection<string> OwnerSubGenre2Options
        {
            get { return ownerSubGenre2Options; }
            set
            {
                ownerSubGenre2Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre2Options"));
            }
        }

        public ImageSource OwnerGenreImage
        {
            get { return ownerGenreImage; }
            set
            {
                ownerGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerGenreImage"));
            }
        }

        public ImageSource OwnerSubGenre1Image
        {
            get { return ownerSubGenre1Image; }
            set
            {
                ownerSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre1Image"));
            }
        }

        public ImageSource OwnerSubGenre2Image
        {
            get { return ownerSubGenre2Image; }
            set
            {
                ownerSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ownerSubGenre2Image"));
            }
        }

        public string SelectedOwnerGenre
        {
            get { return selectedOwnerGenre; }
            set
            {
                selectedOwnerGenre = value;

                if (selectedOwnerGenre != null)
                {
                    Screenplay.OwnerGenre = selectedOwnerGenre;

                    // Refreshes the genre image
                    OwnerGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerGenre)));

                    // Restores options to their default collection
                    OwnerSubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                    OwnerSubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerSubGenre1Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre1Options.Remove(selectedOwnerSubGenre2);
                    OwnerSubGenre1Options = OwnerSubGenre1Options;

                    OwnerSubGenre2Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre2Options.Remove(selectedOwnerSubGenre1);
                    OwnerSubGenre2Options = OwnerSubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOwnerGenre"));
            }
        }

        public string SelectedOwnerSubGenre1
        {
            get { return selectedOwnerSubGenre1; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                selectedOwnerSubGenre1 = value;

                if (selectedOwnerSubGenre1 != null)
                {
                    Screenplay.OwnerSubGenre1 = selectedOwnerSubGenre1;

                    // Refreshes the genre image
                    OwnerSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerSubGenre1)));

                    // Restores options to their default collection
                    OwnerGenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    OwnerSubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre1);
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre2);
                    OwnerGenreOptions = OwnerGenreOptions;

                    OwnerSubGenre2Options.Remove(selectedOwnerSubGenre1);
                    OwnerSubGenre2Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre2Options = OwnerSubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOwnerSubGenre1"));
            }
        }

        public string SelectedOwnerSubGenre2
        {
            get { return selectedOwnerSubGenre2; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                selectedOwnerSubGenre2 = value;

                if (selectedOwnerSubGenre2 != null)
                {
                    Screenplay.OwnerSubGenre2 = selectedOwnerSubGenre2;

                    // Refreshes the genre image
                    OwnerSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerSubGenre2)));

                    // Restores options to their default collection
                    OwnerGenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    OwnerSubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre2);
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre1);
                    OwnerGenreOptions = OwnerGenreOptions;

                    OwnerSubGenre1Options.Remove(selectedOwnerSubGenre2);
                    OwnerSubGenre1Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre1Options = OwnerSubGenre1Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOwnerSubGenre2"));
            }
        }

        public bool CanGiveFeedback
        {
            get { return canGiveFeedback; }
            set
            {
                canGiveFeedback = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGiveFeedback"));
            }
        }

        // Constructors
        public GenresOverviewViewModel() { }

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (GenresOverviewView == null)
                        return;

                    HideView();
                    GenresViewModel.GenresInspectionViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="genresOverviewView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show the genres of</param>
        /// <param name="canGiveFeedback">The indication whether the user can give feedback</param>
        /// <param name="genresViewModel">The view model which manages the genres view</param>
        public void Init(GenresOverviewView genresOverviewView, ScreenplayModel screenplay, bool canGiveFeedback, GenresViewModel genresViewModel)
        {
            GenresOverviewView = genresOverviewView;
            GenresViewModel = genresViewModel;

            Screenplay = screenplay;
            CanGiveFeedback = canGiveFeedback;

            OwnerGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanGiveFeedback ? "Unknown"
                : Screenplay.OwnerGenre)));
            OwnerSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanGiveFeedback ? "Unknown"
                : Screenplay.OwnerSubGenre1)));
            OwnerSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanGiveFeedback ? "Unknown"
                : Screenplay.OwnerSubGenre2)));

            RefreshPieChart();

            if (CanGiveFeedback)
            {
                SelectedOwnerGenre = screenplay.OwnerGenre;
                SelectedOwnerSubGenre1 = screenplay.OwnerSubGenre1;
                SelectedOwnerSubGenre2 = screenplay.OwnerSubGenre2;
            }
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (GenresOverviewView != null)
                App.Current.Dispatcher.Invoke(() => GenresOverviewView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (GenresOverviewView != null)
                App.Current.Dispatcher.Invoke(() => GenresOverviewView.Visibility = Visibility.Collapsed);
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
                    Title = genreName,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(double.Parse(textualPercentage)) },
                    FontSize = 20,
                    DataLabels = true
                });
            }
        }
    }
}