using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesSelectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private string filterText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesSelectionView ArchivesSelectionView { get; private set; }
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ScreenplaysSelectionViewModel ScreenplaysSelectionViewModel { get; private set; }


        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilterText"));
            }
        }

        // Constructors
        public ArchivesSelectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowArchivesFilterViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ArchivesSelectionView == null)
                        return;

                    HideView();
                    ArchivesViewModel.ArchivesFilterViewModel.ShowView();
                });
            }
        }

        public Command ShowReportsInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    MainViewModel mainViewModel = ArchivesViewModel.MainViewModel;
                    ReportsViewModel reportsViewModel = (ReportsViewModel)mainViewModel.ReportsView.DataContext;

                    reportsViewModel.ReportsInspectionViewModel.RefreshView(ScreenplaysSelectionViewModel.ClassifiedScreenplays);

                    reportsViewModel.ReportsSelectionViewModel.HideView();
                    reportsViewModel.ReportsInspectionViewModel.ShowView();
                    mainViewModel.ShowView(mainViewModel.ReportsView);
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="archivesSelectionView">The view to obtain controls from</param>
        /// <param name="archivesViewModel">The view model which manages the archives module</param>
        public void Init(ArchivesSelectionView archivesSelectionView, ArchivesViewModel archivesViewModel)
        {
            ScreenplaysSelectionView screenplaysSelectionView = null;

            ArchivesSelectionView = archivesSelectionView;
            screenplaysSelectionView = (ScreenplaysSelectionView)ArchivesSelectionView.FindName("ScreenplaysSelectionView");
            ScreenplaysSelectionViewModel = (ScreenplaysSelectionViewModel)screenplaysSelectionView.DataContext;
            ScreenplaysSelectionViewModel.Init(screenplaysSelectionView);
            ArchivesViewModel = archivesViewModel;

            FilterText = string.Empty;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ArchivesSelectionView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesSelectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ReportsViewModel reportsViewModel = (ReportsViewModel)ArchivesViewModel.MainViewModel.ReportsView.DataContext;
            ICollectionView reportsCollectionView = CollectionViewSource.GetDefaultView(reportsViewModel.Reports);
            ObservableCollection<ReportModel> filteredReports = new ObservableCollection<ReportModel>();

            reportsCollectionView.Filter = ArchivesViewModel.ArchivesFilterViewModel.Filter;
            reportsCollectionView.Refresh();

            foreach (ReportModel filteredReport in reportsCollectionView)
                filteredReports.Add(filteredReport);

            ScreenplaysSelectionViewModel.RefreshView(filteredReports);

            RefreshFilterText();
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ArchivesSelectionView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesSelectionView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Refreshes the filter texts.
        /// </summary>
        private void RefreshFilterText()
        {
            UserModel user = ArchivesViewModel.MainViewModel.UserToolbarViewModel.User;
            ArchivesFilterViewModel filterViewModel = ArchivesViewModel.ArchivesFilterViewModel;
            string owner = filterViewModel.FilteredOwner, genre = filterViewModel.FilteredGenre,
                subGenre1 = filterViewModel.FilteredSubGenre1, subGenre2 = filterViewModel.FilteredSubGenre2;
            int[] genreRange = new int[] { filterViewModel.FilteredGenreMinPercentage, filterViewModel.FilteredGenreMaxPercentage },
                subGenre1Range = new int[] { filterViewModel.FilteredSubGenre1MinPercentage, filterViewModel.FilteredSubGenre1MaxPercentage },
                subGenre2Range = new int[] { filterViewModel.FilteredSubGenre2MinPercentage, filterViewModel.FilteredSubGenre2MaxPercentage };

            // Builds the filter string 
            FilterText = string.Empty;
            if (user.Role == UserModel.UserRole.ADMIN)
                FilterText = string.Format("Owner: {0} | ", string.IsNullOrEmpty(owner) ? "All Owners" : owner);

            FilterText += string.Format("Main Genre: {0} | Subgenre 1: {1} | Subgenre 2: {2}",
                BuildGenreFilterText(genre, genreRange),
                BuildGenreFilterText(subGenre1, subGenre1Range),
                BuildGenreFilterText(subGenre2, subGenre2Range));
        }

        /// <summary>
        /// Builds a formatted filter string for a given genre.
        /// </summary>
        /// <param name="genreName">The genre's name</param>
        /// <param name="genreRange">The genre's percentage range</param>
        /// <returns>The formatted filter string for the genre</returns>
        private string BuildGenreFilterText(string genreName, int[] genreRange)
        {
            return string.Format("{0} ({1}% - {2}%)", string.IsNullOrEmpty(genreName) ? "All" : genreName, genreRange[0], genreRange[1]);
        }
    }
}