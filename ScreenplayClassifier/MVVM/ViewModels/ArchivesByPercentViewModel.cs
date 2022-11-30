using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using NumericUpDownLib;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesByPercentViewModel : INotifyPropertyChanged
    {
        // Fields
        private Predicate<object> genreFilter, genrePercentageFilter;
        private Predicate<object> subGenre1Filter, subGenre1PercentageFilter;
        private Predicate<object> subGenre2Filter, subGenre2PercentageFilter;

        private string filteredGenre, filteredSubGenre1, filteredSubGenre2;
        private int filteredGenreMinPercentage, filteredSubGenre1MinPercentage, filteredSubGenre2MinPercentage;
        private int filteredGenreMaxPercentage, filteredSubGenre1MaxPercentage, filteredSubGenre2MaxPercentage;
        private ScreenplayModel selectedScreenplay;
        private ObservableCollection<ScreenplayModel> archives;
        private ObservableCollection<string> genreOptions, subGenre1Options, subGenre2Options;
        private SeriesCollection percentageSeries;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesByPercentView ArchivesByPercentView { get; private set; }

        public string FilteredGenre
        {
            get { return filteredGenre; }
            set
            {
                // TODO: FIX (selection isn't shown)
                filteredGenre = value;

                if (filteredGenre != null)
                {
                    SubGenre1Options = JSON.LoadedGenres;
                    SubGenre2Options = JSON.LoadedGenres;

                    SubGenre1Options.Remove(filteredGenre);
                    SubGenre1Options.Remove(FilteredSubGenre2);
                    SubGenre1Options = SubGenre1Options;

                    SubGenre2Options.Remove(filteredGenre);
                    SubGenre2Options.Remove(FilteredSubGenre1);
                    SubGenre2Options = SubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredGenre"));
            }
        }

        public string FilteredSubGenre1
        {
            get { return filteredSubGenre1; }
            set
            {
                // TODO: FIX (selection isn't shown)
                filteredSubGenre1 = value;

                if (filteredSubGenre1 != null)
                {
                    GenreOptions = JSON.LoadedGenres;
                    SubGenre2Options = JSON.LoadedGenres;

                    GenreOptions.Remove(filteredSubGenre1);
                    GenreOptions.Remove(FilteredSubGenre2);
                    GenreOptions = GenreOptions;

                    SubGenre2Options.Remove(filteredSubGenre1);
                    SubGenre2Options.Remove(FilteredGenre);
                    SubGenre2Options = SubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredSubGenre1"));
            }
        }

        public string FilteredSubGenre2
        {
            get { return filteredSubGenre2; }
            set
            {
                // TODO: FIX (selection isn't shown)
                filteredSubGenre2 = value;

                if (filteredSubGenre2 != null)
                {
                    GenreOptions = JSON.LoadedGenres;
                    SubGenre1Options = JSON.LoadedGenres;

                    GenreOptions.Remove(filteredSubGenre2);
                    GenreOptions.Remove(FilteredSubGenre1);
                    GenreOptions = GenreOptions;

                    SubGenre1Options.Remove(filteredSubGenre2);
                    SubGenre1Options.Remove(FilteredGenre);
                    SubGenre1Options = SubGenre1Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredSubGenre2"));
            }
        }

        public int FilteredGenreMinPercentage
        {
            get { return filteredGenreMinPercentage; }
            set
            {
                filteredGenreMinPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredGenreMinPercentage"));
            }
        }

        public int FilteredSubGenre1MinPercentage
        {
            get { return filteredSubGenre1MinPercentage; }
            set
            {
                filteredSubGenre1MinPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredSubGenre1MinPercentage"));
            }
        }

        public int FilteredSubGenre2MinPercentage
        {
            get { return filteredSubGenre2MinPercentage; }
            set
            {
                filteredSubGenre2MinPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredSubGenre2MinPercentage"));
            }
        }

        public int FilteredGenreMaxPercentage
        {
            get { return filteredGenreMaxPercentage; }
            set
            {
                filteredGenreMaxPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredGenreMaxPercentage"));
            }
        }

        public int FilteredSubGenre1MaxPercentage
        {
            get { return filteredSubGenre1MaxPercentage; }
            set
            {
                filteredSubGenre1MaxPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredSubGenre1MaxPercentage"));
            }
        }

        public int FilteredSubGenre2MaxPercentage
        {
            get { return filteredSubGenre2MaxPercentage; }
            set
            {
                filteredSubGenre2MaxPercentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredSubGenre2MaxPercentage"));
            }
        }

        public ScreenplayModel SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                RefreshPieChart();

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public ObservableCollection<ScreenplayModel> Archives
        {
            get { return archives; }
            set
            {
                archives = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Archives"));
            }
        }

        public ObservableCollection<string> GenreOptions
        {
            get { return genreOptions; }
            set
            {
                genreOptions = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreOptions"));
            }
        }

        public ObservableCollection<string> SubGenre1Options
        {
            get { return subGenre1Options; }
            set
            {
                subGenre1Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1Options"));
            }
        }

        public ObservableCollection<string> SubGenre2Options
        {
            get { return subGenre2Options; }
            set
            {
                subGenre2Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2Options"));
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
        public ArchivesByPercentViewModel() { }

        // Methods
        #region Commands
        public Command BackCommand
        {
            get { return new Command(() => ArchivesViewModel.ShowView(ArchivesViewModel.ArchivesSelectionView)); }
        }

        public Command ChangeFilteredGenrePercentageRangeCommand
        {
            get
            {
                return new Command(() =>
                {
                    NumericUpDown minPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredGenreMinPercentageNumericUpDown"),
                        maxPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredGenreMaxPercentageNumericUpDown");

                    FilteredGenreMinPercentage = minPercentageNumericUpDown.Value;
                    FilteredGenreMaxPercentage = maxPercentageNumericUpDown.Value;
                });
            }
        }

        public Command ChangeFilteredSubGenre1PercentageRangeCommand
        {
            get
            {
                return new Command(() =>
                {
                    NumericUpDown minPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre1MinPercentageNumericUpDown"),
                        maxPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre1MaxPercentageNumericUpDown");

                    FilteredSubGenre1MinPercentage = minPercentageNumericUpDown.Value;
                    FilteredSubGenre1MaxPercentage = maxPercentageNumericUpDown.Value;
                });
            }
        }

        public Command ChangeFilteredSubGenre2PercentageRangeCommand
        {
            get
            {
                return new Command(() =>
                {
                    NumericUpDown minPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre2MinPercentageNumericUpDown"),
                        maxPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre2MaxPercentageNumericUpDown");

                    FilteredSubGenre2MinPercentage = minPercentageNumericUpDown.Value;
                    FilteredSubGenre2MaxPercentage = maxPercentageNumericUpDown.Value;
                });
            }
        }

        public Command FilterArchivesCommand
        {
            get
            {
                return new Command(() =>
                {
                    ICollectionView archivesCollectionView = CollectionViewSource.GetDefaultView(Archives);

                    genreFilter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredGenre) ? true : ((ScreenplayModel)o).ActualGenre == FilteredGenre;
                    };
                    subGenre1Filter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredSubGenre1) ? true : ((ScreenplayModel)o).ActualSubGenre1 == FilteredSubGenre1;
                    };
                    subGenre2Filter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredSubGenre2) ? true : ((ScreenplayModel)o).ActualSubGenre2 == FilteredSubGenre2;
                    };

                    genrePercentageFilter = (o) =>
                    {
                        ScreenplayModel screenplay = (ScreenplayModel)o;
                        float genrePercentage = screenplay.GenrePercentages[screenplay.ActualGenre];

                        return string.IsNullOrEmpty(FilteredGenre) ? true
                            : (genrePercentage >= FilteredGenreMinPercentage) && (genrePercentage <= FilteredGenreMaxPercentage);
                    };
                    subGenre1PercentageFilter = (o) =>
                    {
                        ScreenplayModel screenplay = (ScreenplayModel)o;
                        float subGenre1Percentage = screenplay.GenrePercentages[screenplay.ActualSubGenre1];

                        return string.IsNullOrEmpty(FilteredSubGenre1) ? true
                            : (subGenre1Percentage >= FilteredSubGenre1MinPercentage) && (subGenre1Percentage <= FilteredSubGenre1MaxPercentage);
                    };
                    subGenre2PercentageFilter = (o) =>
                    {
                        ScreenplayModel screenplay = (ScreenplayModel)o;
                        float subGenre2Percentage = screenplay.GenrePercentages[screenplay.ActualSubGenre2];

                        return string.IsNullOrEmpty(FilteredSubGenre2) ? true
                            : (subGenre2Percentage >= FilteredSubGenre2MinPercentage) && (subGenre2Percentage <= FilteredSubGenre2MaxPercentage);
                    };

                    archivesCollectionView.Filter = (o) =>
                    {
                        return (genreFilter.Invoke(o)) && (subGenre1Filter.Invoke(o)) && (subGenre2Filter.Invoke(o)) &&
                            (genrePercentageFilter.Invoke(o)) && (subGenre1PercentageFilter.Invoke(o)) && (subGenre2PercentageFilter.Invoke(o));
                    };
                    archivesCollectionView.Refresh();

                    RefreshPieChart();
                });
            }
        }

        public Command ClearFilterCommand
        {
            get
            {
                return new Command(() =>
                {
                    // TODO: FIX (doesn't clear options)
                    NumericUpDown genreMinPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredGenreMinPercentageNumericUpDown"),
                        genreMaxPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredGenreMaxPercentageNumericUpDown"),
                        subGenre1MinPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre1MinPercentageNumericUpDown"),
                        subGenre1MaxPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre1MaxPercentageNumericUpDown"),
                        subGenre2MinPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre2MinPercentageNumericUpDown"),
                        subGenre2MaxPercentageNumericUpDown = (NumericUpDown)ArchivesByPercentView.FindName("FilteredSubGenre2MaxPercentageNumericUpDown");

                    GenreOptions = JSON.LoadedGenres;
                    SubGenre1Options = JSON.LoadedGenres;
                    SubGenre2Options = JSON.LoadedGenres;

                    FilteredGenre = null;
                    FilteredSubGenre1 = null;
                    FilteredSubGenre2 = null;

                    SelectedScreenplay = null;

                    genreMinPercentageNumericUpDown.Value = FilteredGenreMinPercentage = 0;
                    genreMaxPercentageNumericUpDown.Value = FilteredGenreMaxPercentage = 100;

                    subGenre1MinPercentageNumericUpDown.Value = FilteredSubGenre1MinPercentage = 0;
                    subGenre1MaxPercentageNumericUpDown.Value = FilteredSubGenre1MaxPercentage = 100;

                    subGenre2MinPercentageNumericUpDown.Value = FilteredSubGenre2MinPercentage = 0;
                    subGenre2MaxPercentageNumericUpDown.Value = FilteredSubGenre2MaxPercentage = 100;

                    FilterArchivesCommand.Execute(null);
                });
            }
        }
        #endregion

        public void Init(ArchivesByPercentView archivesByPercentView, ArchivesViewModel archivesViewModel)
        {
            ArchivesViewModel = archivesViewModel;
            ArchivesByPercentView = archivesByPercentView;
        }

        public void RefreshArchives(List<ScreenplayModel> refreshedArchives)
        {
            Archives = new ObservableCollection<ScreenplayModel>();

            foreach (ScreenplayModel screenplay in refreshedArchives)
                if (!Archives.Contains(screenplay))
                    Archives.Add(screenplay);

            Archives = Archives; // Triggers PropertyChanged event
        }

        private void RefreshPieChart()
        {
            float genrePercentage;

            PercentageSeries = new SeriesCollection();
            if (SelectedScreenplay == null)
                return;

            foreach (string genreName in JSON.LoadedGenres)
            {
                genrePercentage = SelectedScreenplay.GenrePercentages[genreName];
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