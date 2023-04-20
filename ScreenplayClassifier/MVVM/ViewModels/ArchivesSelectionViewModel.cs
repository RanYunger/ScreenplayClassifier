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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesSelectionViewModel : PropertyChangeNotifier
    {
        // Fields
        private string filterText;

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

                NotifyPropertyChange();
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

                    ScreenplaysSelectionViewModel.StopMusicCommand.Execute(null);

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

                    ScreenplaysSelectionViewModel.StopMusicCommand.Execute(null);

                    reportsViewModel.ReportsInspectionViewModel.RefreshView(ScreenplaysSelectionViewModel.ClassifiedScreenplays, "Archives");
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
            ArchivesFilterViewModel archivesFilterViewModel = ArchivesViewModel.ArchivesFilterViewModel;
            string genre = archivesFilterViewModel.FilteredGenre, subGenre1 = archivesFilterViewModel.FilteredSubGenre1,
                subGenre2 = archivesFilterViewModel.FilteredSubGenre2;
            bool canPlayGif = false;

            reportsCollectionView.Filter = ArchivesViewModel.ArchivesFilterViewModel.Filter;
            reportsCollectionView.Refresh();

            foreach (ReportModel filteredReport in reportsCollectionView)
                filteredReports.Add(filteredReport);
            canPlayGif = (!string.IsNullOrEmpty(genre)) && (string.IsNullOrEmpty(subGenre1)) && (string.IsNullOrEmpty(subGenre2))
                && (filteredReports.Count > 0);

            ScreenplaysSelectionViewModel.RefreshView(filteredReports, "No screenplays matching the criteria", canPlayGif ? genre : string.Empty);

            RefreshFilterText(archivesFilterViewModel);
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
        /// <param name="filterViewModel">The view model which stored the filter criteria</param>
        /// </summary>
        private void RefreshFilterText(ArchivesFilterViewModel filterViewModel)
        {
            UserModel user = ArchivesViewModel.MainViewModel.UserToolbarViewModel.User;
            string owner = filterViewModel.FilteredOwner;
            int[] genreRange = new int[] { filterViewModel.FilteredGenreMinPercentage, filterViewModel.FilteredGenreMaxPercentage },
                subGenre1Range = new int[] { filterViewModel.FilteredSubGenre1MinPercentage, filterViewModel.FilteredSubGenre1MaxPercentage },
                subGenre2Range = new int[] { filterViewModel.FilteredSubGenre2MinPercentage, filterViewModel.FilteredSubGenre2MaxPercentage };

            // Builds the filter string 
            FilterText = string.Empty;
            if (user.Role == UserModel.UserRole.ADMIN)
                FilterText = string.Format("Owner: {0} | ", string.IsNullOrEmpty(owner) ? "All Owners" : owner);

            FilterText += string.Format("Main Genre: {0} | Subgenre 1: {1} | Subgenre 2: {2}",
                BuildGenreFilterText(filterViewModel.FilteredGenre, genreRange),
                BuildGenreFilterText(filterViewModel.FilteredSubGenre1, subGenre1Range),
                BuildGenreFilterText(filterViewModel.FilteredSubGenre2, subGenre2Range));
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