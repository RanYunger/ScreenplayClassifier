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
using System.Windows.Data;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesByPercentViewModel : INotifyPropertyChanged
    {
        // Fields
        private List<string> allGenres;
        private Predicate<object> genreFilter;
        private Predicate<object> subGenre1Filter;
        private Predicate<object> subGenre2Filter;

        private string filteredGenre, filteredSubGenre1, filteredSubGenre2;
        private ScreenplayModel selectedScreenplay;
        private ObservableCollection<ScreenplayModel> archives;
        private ObservableCollection<string> genreOptions, subGenre1Options, subGenre2Options;
        private SeriesCollection selectedScreenplayGenresSeries;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesByPercentView ArchivesByPercentView { get; private set; }

        public string FilteredGenre
        {
            get { return filteredGenre; }
            set
            {
                filteredGenre = value;

                if (filteredGenre != null)
                {
                    SubGenre1Options = new ObservableCollection<string>(allGenres);
                    SubGenre2Options = new ObservableCollection<string>(allGenres);

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
                ObservableCollection<string> filteredOptions = new ObservableCollection<string>(allGenres);

                filteredSubGenre1 = value;

                if (filteredSubGenre1 != null)
                {
                    GenreOptions = new ObservableCollection<string>(allGenres);
                    SubGenre2Options = new ObservableCollection<string>(allGenres);

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
                ObservableCollection<string> filteredOptions = new ObservableCollection<string>(allGenres);

                filteredSubGenre2 = value;

                if (filteredSubGenre2 != null)
                {
                    GenreOptions = new ObservableCollection<string>(allGenres);
                    SubGenre1Options = new ObservableCollection<string>(allGenres);

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

        public ScreenplayModel SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

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

        // Constructors
        public ArchivesByPercentViewModel() { }

        // Methods
        #region Commands
        public Command BackCommand
        {
            get { return new Command(() => ArchivesViewModel.ShowView(ArchivesViewModel.ArchivesSelectionView)); }
        }

        public Command FilterReportsCommand
        {
            get
            {
                return new Command(() =>
                {
                    ICollectionView screenplaysCollectionView = CollectionViewSource.GetDefaultView(ArchivesViewModel.Screenplays);

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

                    screenplaysCollectionView.Filter = (o) =>
                    {
                        return (genreFilter.Invoke(o)) && (subGenre1Filter.Invoke(o)) && (subGenre2Filter.Invoke(o));
                    };
                    screenplaysCollectionView.Refresh();

                    //RefreshPieCharts(screenplaysCollectionView);
                });
            }
        }

        public Command ClearFilterCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreOptions = new ObservableCollection<string>(allGenres);
                    SubGenre1Options = new ObservableCollection<string>(allGenres);
                    SubGenre2Options = new ObservableCollection<string>(allGenres);

                    FilteredGenre = null;
                    FilteredSubGenre1 = null;
                    FilteredSubGenre2 = null;

                    FilterReportsCommand.Execute(null);
                });
            }
        }
        #endregion

        public void Init(ArchivesByPercentView archivesByPercentView, ArchivesViewModel archivesViewModel)
        {
            allGenres = Storage.LoadGenres();

            ArchivesViewModel = archivesViewModel;
            ArchivesByPercentView = archivesByPercentView;
        }

        public void RefreshArchives(List<ScreenplayModel> refreshedArchives)
        {
            Archives = new ObservableCollection<ScreenplayModel>();

            foreach (ScreenplayModel screenplay in refreshedArchives)
            {
                if (!Archives.Contains(screenplay))
                    Archives.Add(screenplay);
            }

            Archives = Archives; // Triggers PropertyChanged event
        }

        private void RefreshPieCharts(ICollectionView reportsCollectionView)
        {
            float genreCount;

            selectedScreenplayGenresSeries = new SeriesCollection();

            foreach (string genreName in allGenres)
            {
                genreCount = SelectedScreenplay.MatchingPercentages[genreName];
                if (genreCount > 0)
                    selectedScreenplayGenresSeries.Add(new PieSeries()
                    {
                        Title = genreName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(genreCount) },
                        DataLabels = true
                    });
            }
        }
    }
}
