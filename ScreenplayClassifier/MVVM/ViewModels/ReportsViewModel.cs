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
    class ReportsViewModel : INotifyPropertyChanged
    {
        // Fields
        private Predicate<object> nameFilter;
        private Predicate<object> genreFilter;
        private Predicate<object> subGenre1Filter;
        private Predicate<object> subGenre2Filter;

        private ObservableCollection<ClassificationModel> reports;
        private ObservableCollection<string> genreOptions, subGenre1Options, subGenre2Options;
        private SeriesCollection genreSeries, subGenre1Series, subGenre2Series, ownerSeries;
        private Func<double, string> labelFormatter;
        private string[] ownerLabels;
        private string namePattern, filteredGenre, filteredSubGenre1, filteredSubGenre2;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ReportsView ReportsView { get; private set; }

        public ObservableCollection<ClassificationModel> Reports
        {
            get { return reports; }
            set
            {
                reports = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationReports"));
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

        public SeriesCollection GenreSeries
        {
            get { return genreSeries; }
            set
            {
                genreSeries = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreSeries"));
            }
        }

        public SeriesCollection SubGenre1Series
        {
            get { return subGenre1Series; }
            set
            {
                subGenre1Series = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1Series"));
            }
        }

        public SeriesCollection SubGenre2Series
        {
            get { return subGenre2Series; }
            set
            {
                subGenre2Series = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2Series"));
            }
        }

        public SeriesCollection OwnerSeries
        {
            get { return ownerSeries; }
            set
            {
                ownerSeries = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSeries"));
            }
        }

        public Func<double, string> LabelFormatter
        {
            get { return labelFormatter; }
            set
            {
                labelFormatter = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LabelFormatter"));
            }
        }

        public string[] OwnerLabels
        {
            get { return ownerLabels; }
            set
            {
                ownerLabels = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerLabels"));
            }
        }

        public string NamePattern
        {
            get { return namePattern; }
            set
            {
                namePattern = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NamePattern"));
            }
        }

        public string FilteredGenre
        {
            get { return filteredGenre; }
            set
            {
                filteredGenre = value;

                if (filteredGenre != null)
                {
                    SubGenre1Options = new ObservableCollection<string>(JSON.loadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.loadedGenres);

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
                ObservableCollection<string> filteredOptions = new ObservableCollection<string>(JSON.loadedGenres);

                filteredSubGenre1 = value;

                if (filteredSubGenre1 != null)
                {
                    GenreOptions = new ObservableCollection<string>(JSON.loadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.loadedGenres);

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
                ObservableCollection<string> filteredOptions = new ObservableCollection<string>(JSON.loadedGenres);

                filteredSubGenre2 = value;

                if (filteredSubGenre2 != null)
                {
                    GenreOptions = new ObservableCollection<string>(JSON.loadedGenres);
                    SubGenre1Options = new ObservableCollection<string>(JSON.loadedGenres);

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

        // Constructors
        public ReportsViewModel() { }

        // Methods
        #region Commands
        public Command FilterReportsCommand
        {
            get
            {
                return new Command(() =>
                {
                    ICollectionView reportsCollectionView = CollectionViewSource.GetDefaultView(Reports);

                    nameFilter = (o) =>
                    {
                        return string.IsNullOrEmpty(NamePattern) ? true
                            : ((ClassificationModel)o).Screenplay.Name.Contains(NamePattern);
                    };
                    genreFilter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredGenre) ? true
                            : ((ClassificationModel)o).Screenplay.ActualGenre == FilteredGenre;
                    };
                    subGenre1Filter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredSubGenre1) ? true
                            : ((ClassificationModel)o).Screenplay.ActualSubGenre1 == FilteredSubGenre1;
                    };
                    subGenre2Filter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredSubGenre2) ? true
                            : ((ClassificationModel)o).Screenplay.ActualSubGenre2 == FilteredSubGenre2;
                    };

                    reportsCollectionView.Filter = (o) =>
                    {
                        return (nameFilter.Invoke(o)) && (genreFilter.Invoke(o)) && (subGenre1Filter.Invoke(o)) && (subGenre2Filter.Invoke(o));
                    };
                    reportsCollectionView.Refresh();

                    RefreshPieCharts(reportsCollectionView);
                    RefreshBarChart(reportsCollectionView);
                });
            }
        }

        public Command ClearFilterCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreOptions = JSON.loadedGenres;
                    SubGenre1Options = JSON.loadedGenres;
                    SubGenre2Options = JSON.loadedGenres;

                    NamePattern = null;
                    FilteredGenre = null;
                    FilteredSubGenre1 = null;
                    FilteredSubGenre2 = null;

                    FilterReportsCommand.Execute(null);
                });
            }
        }
        #endregion

        public void Init(ReportsView reportsView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ReportsView = reportsView;

            InitReports();
        }

        private void InitReports()
        {
            UserModel user = MainViewModel.UserToolbarViewModel.User;
            List<ClassificationModel> memberReports;

            if (user.Role == UserModel.UserRole.GUEST)
                Reports = new ObservableCollection<ClassificationModel>();
            else
            {
                Reports = JSON.LoadReports();

                if (user.Role == UserModel.UserRole.MEMBER)
                {
                    memberReports = new List<ClassificationModel>(Reports).FindAll(report => report.Owner.Username.Equals(user.Username));
                    Reports = new ObservableCollection<ClassificationModel>(memberReports);
                }
            }
        }

        private void RefreshPieCharts(ICollectionView reportsCollectionView)
        {
            int genreCount, subGenre1Count, subGenre2Count;

            GenreSeries = new SeriesCollection();
            SubGenre1Series = new SeriesCollection();
            SubGenre2Series = new SeriesCollection();

            foreach (string genreName in JSON.loadedGenres)
            {
                genreCount = CountRecordsByGenre(reportsCollectionView, genreName, "Genre");
                if (genreCount > 0)
                    GenreSeries.Add(new PieSeries()
                    {
                        Title = genreName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(genreCount) },
                        DataLabels = true
                    });

                subGenre1Count = CountRecordsByGenre(reportsCollectionView, genreName, "SubGenre1");
                if (subGenre1Count > 0)
                    SubGenre1Series.Add(new PieSeries()
                    {
                        Title = genreName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(subGenre1Count) },
                        DataLabels = true
                    });

                subGenre2Count = CountRecordsByGenre(reportsCollectionView, genreName, "SubGenre2");
                if (subGenre2Count > 0)
                    SubGenre2Series.Add(new PieSeries()
                    {
                        Title = genreName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(subGenre2Count) },
                        DataLabels = true
                    });
            }
        }

        private void RefreshBarChart(ICollectionView reportsCollectionView)
        {
            int ownerCount;

            OwnerSeries = new SeriesCollection();
            LabelFormatter = value => value.ToString("N");
            OwnerLabels = new string[] { };

            foreach (UserModel owner in JSON.loadedUsers)
            {
                ownerCount = CountRecordsByOwner(reportsCollectionView, owner);

                if (ownerCount > 0)
                    OwnerSeries.Add(new ColumnSeries()
                    {
                        Title = owner.Username,
                        Values = new ChartValues<double> { ownerCount }
                    });
            }
        }

        private int CountRecordsByGenre(ICollectionView reportsCollectionView, string genreName, string genreType)
        {
            ClassificationModel currentReport;
            int count = 0;

            reportsCollectionView.MoveCurrentToFirst();

            do
            {
                currentReport = (ClassificationModel)reportsCollectionView.CurrentItem;
                if (currentReport != null)
                    switch (genreType)
                    {
                        case "Genre": count += Convert.ToInt32(currentReport.Screenplay.ActualGenre == genreName); break;
                        case "SubGenre1": count += Convert.ToInt32(currentReport.Screenplay.ActualSubGenre1 == genreName); break;
                        case "SubGenre2": count += Convert.ToInt32(currentReport.Screenplay.ActualSubGenre2 == genreName); break;
                    }
            }
            while (reportsCollectionView.MoveCurrentToNext());

            return count;
        }

        private int CountRecordsByOwner(ICollectionView reportsCollectionView, UserModel owner)
        {
            ClassificationModel currentReport;
            int count = 0;

            reportsCollectionView.MoveCurrentToFirst();

            do
            {
                currentReport = (ClassificationModel)reportsCollectionView.CurrentItem;
                if (currentReport != null)
                    count += Convert.ToInt32(currentReport.Owner.Username == owner.Username);
            }
            while (reportsCollectionView.MoveCurrentToNext());

            return count;
        }
    }
}
