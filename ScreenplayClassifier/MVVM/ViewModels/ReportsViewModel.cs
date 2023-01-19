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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ReportsViewModel : INotifyPropertyChanged
    {
        // Fields
        private Predicate<object> titleFilter;
        private Predicate<object> genreFilter;
        private Predicate<object> subGenre1Filter;
        private Predicate<object> subGenre2Filter;

        private ObservableCollection<ClassificationModel> reports;
        private ObservableCollection<string> genreOptions, subGenre1Options, subGenre2Options;
        private SeriesCollection genreSeries, subGenre1Series, subGenre2Series, ownerSeries;
        private Func<double, string> labelFormatter;
        private string[] ownerLabels;
        private string filteredGenre, filteredSubGenre1, filteredSubGenre2;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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

        public string FilteredGenre
        {
            get { return filteredGenre; }
            set
            {
                filteredGenre = value;

                if (filteredGenre != null)
                {
                    // Restores options to their default collection
                    SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
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
                ObservableCollection<string> filteredOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                filteredSubGenre1 = value;

                if (filteredSubGenre1 != null)
                {
                    // Restores options to their default collection
                    GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
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
                ObservableCollection<string> filteredOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                filteredSubGenre2 = value;

                if (filteredSubGenre2 != null)
                {
                    // Restores options to their default collection
                    GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
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
        public Command EnterTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string titlePattern = string.Empty;

                    // Validation
                    if (ReportsView == null)
                        return;

                    titleTextBox = (TextBox)ReportsView.FindName("TitleTextBox");
                    titlePattern = titleTextBox.Text;

                    if (string.Equals(titlePattern, "Pattern"))
                    {
                        titleTextBox.Foreground = Brushes.Black;
                        titleTextBox.Text = string.Empty;
                    }
                });
            }
        }

        public Command LeaveTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string titlePattern = string.Empty;

                    // Validation
                    if (ReportsView == null)
                        return;

                    titleTextBox = (TextBox)ReportsView.FindName("TitleTextBox");
                    titlePattern = titleTextBox.Text;

                    if (string.IsNullOrEmpty(titlePattern))
                    {
                        titleTextBox.Foreground = Brushes.Gray;
                        titleTextBox.Text = "Pattern";
                    }
                });
            }
        }

        public Command FilterReportsCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    ICollectionView reportsCollectionView = CollectionViewSource.GetDefaultView(Reports);
                    string titlePattern = null;

                    // Validation
                    if (ReportsView == null)
                        return;

                    titleTextBox = (TextBox)ReportsView.FindName("TitleTextBox");
                    titlePattern = titleTextBox.Text;

                    // Updates all filters
                    titleFilter = (o) =>
                    {
                        return string.IsNullOrEmpty(titlePattern.Trim()) || string.Equals(titlePattern, "Pattern")
                            ? true : ((ClassificationModel)o).Screenplay.Title.Contains(titlePattern);
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

                    // Activates a combination of all filters
                    reportsCollectionView.Filter = (o) =>
                    {
                        return (titleFilter.Invoke(o)) && (genreFilter.Invoke(o)) && (subGenre1Filter.Invoke(o)) && (subGenre2Filter.Invoke(o));
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
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    // Validation
                    if (ReportsView == null)
                        return;

                    // Restores options to their default collection
                    GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Clears filtered values
                    titleTextBox = (TextBox)ReportsView.FindName("TitleTextBox");
                    titleTextBox.Foreground = Brushes.Gray;
                    titleTextBox.Text = "Pattern";

                    FilteredGenre = null;
                    FilteredSubGenre1 = null;
                    FilteredSubGenre2 = null;

                    FilterReportsCommand.Execute(null);
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="user">The user who authenticated to the system</param>
        public void Init(ReportsView reportsView, UserModel user)
        {
            ReportsView = reportsView;

            InitReports(user);
        }

        /// <summary>
        /// Initiates the reports.
        /// </summary>
        /// <param name="user">The user authenticated to the system</param>
        private void InitReports(UserModel user)
        {
            List<ClassificationModel> memberReports;

            if (user.Role == UserModel.UserRole.GUEST)
                Reports = new ObservableCollection<ClassificationModel>();
            else
            {
                JSON.LoadReports();
                Reports = new ObservableCollection<ClassificationModel>(JSON.LoadedReports);

                // Members can only view the reports they own
                if (user.Role == UserModel.UserRole.MEMBER)
                {
                    memberReports = new List<ClassificationModel>(Reports).FindAll(report => report.Owner.Username.Equals(user.Username));
                    Reports = new ObservableCollection<ClassificationModel>(memberReports);
                }
            }
        }

        /// <summary>
        /// Refreshes the pie charts.
        /// </summary>
        /// <param name="reportsCollectionView">The filtered reports collection</param>
        private void RefreshPieCharts(ICollectionView reportsCollectionView)
        {
            int genreCount, subGenre1Count, subGenre2Count;

            GenreSeries = new SeriesCollection();
            SubGenre1Series = new SeriesCollection();
            SubGenre2Series = new SeriesCollection();

            // Creates a slice for each genre criteria
            foreach (string genreName in JSON.LoadedGenres)
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

        /// <summary>
        /// Refreshes the bar chart.
        /// </summary>
        /// <param name="reportsCollectionView">The filtered reports collection</param>
        private void RefreshBarChart(ICollectionView reportsCollectionView)
        {
            int ownerCount;

            OwnerSeries = new SeriesCollection();
            LabelFormatter = value => value.ToString("N");
            OwnerLabels = new string[] { };

            // Creates a bar for each user
            foreach (UserModel owner in JSON.LoadedUsers)
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

        /// <summary>
        /// Counts records by a given genre query.
        /// </summary>
        /// <param name="reportsCollectionView">The filtered reports collection</param>
        /// <param name="genreName">The genre label to count by</param>
        /// <param name="genreType">The genre's type: Main/SubGenre1/SubGenre2</param>
        /// <returns></returns>
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

        /// <summary>
        /// Counts records by a given owner.
        /// </summary>
        /// <param name="reportsCollectionView">The filtered reports collection</param>
        /// <param name="owner">The owenr to check records by</param>
        /// <returns></returns>
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
