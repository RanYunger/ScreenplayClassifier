using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<BrowseModel> filteredScreenplays, checkedScreenplays;
        private string ownerFilterText, genresFilterText;
        private int selectedScreenplay;
        private bool canInspect;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesInspectionView ArchivesInspectionView { get; private set; }

        public ObservableCollection<BrowseModel> FilteredScreenplays
        {
            get { return filteredScreenplays; }
            set
            {
                filteredScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredScreenplays"));
            }
        }

        public ObservableCollection<BrowseModel> CheckedScreenplays
        {
            get { return checkedScreenplays; }
            set
            {
                checkedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedScreenplays"));
            }
        }

        public string OwnerFilterText
        {
            get { return ownerFilterText; }
            set
            {
                ownerFilterText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerFilterText"));
            }
        }

        public string GenresFilterText
        {
            get { return genresFilterText; }
            set
            {
                genresFilterText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenresFilterText"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if (ArchivesInspectionView != null)
                    CheckSelectionCommand.Execute(null);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanInspect
        {
            get { return canInspect; }
            set
            {
                canInspect = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanInspect"));
            }
        }

        // Constructors
        public ArchivesInspectionViewModel() { }

        // Methods
        #region Commands
        public Command CheckSelectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    BrowseModel chosenScreenplay = null;

                    // Validation
                    if ((selectedScreenplay == -1) || (FilteredScreenplays.Count == 0))
                        return;

                    chosenScreenplay = FilteredScreenplays[selectedScreenplay];
                    if (CheckedScreenplays.Contains(chosenScreenplay))
                    {
                        chosenScreenplay.IsChecked = false;
                        CheckedScreenplays.Remove(chosenScreenplay);
                    }
                    else
                    {
                        chosenScreenplay.IsChecked = true;
                        CheckedScreenplays.Add(chosenScreenplay);
                    }

                    CanInspect = CheckedScreenplays.Count > 0;
                });
            }
        }

        public Command ShowFilterViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ArchivesInspectionView == null)
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
                    // TODO: COMPLETE
                    //MainViewModel mainViewModel = ArchivesViewModel.MainViewModel;
                    //ReportsViewModel reportsViewModel = (ReportsViewModel)mainViewModel.ReportsView.DataContext;

                    //HideView();
                    //reportsViewModel.ReportsSelectionViewModel.HideView();
                    //reportsViewModel.ReportsInspectionViewModel.ShowView();

                    //mainViewModel.ShowView(mainViewModel.ReportsView);
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="archivesInspectionView">The view to obtain controls from</param>
        /// <param name="archivesViewModel">The view model which manages the archives module</param>
        public void Init(ArchivesInspectionView archivesInspectionView, ArchivesViewModel archivesViewModel)
        {
            ArchivesInspectionView = archivesInspectionView;
            ArchivesViewModel = archivesViewModel;

            FilteredScreenplays = new ObservableCollection<BrowseModel>();
            CheckedScreenplays = new ObservableCollection<BrowseModel>();

            RefreshView();

            OwnerFilterText = string.Empty;
            GenresFilterText = string.Empty;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ArchivesInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ReportsViewModel reportsViewModel = (ReportsViewModel)ArchivesViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesFilterViewModel filterViewModel = ArchivesViewModel.ArchivesFilterViewModel;
            string owner = filterViewModel.FilteredOwner, genre = filterViewModel.FilteredGenre,
                subGenre1 = filterViewModel.FilteredSubGenre1, subGenre2 = filterViewModel.FilteredSubGenre2;
            int[] genreRange = new int[] { filterViewModel.FilteredGenreMinPercentage, filterViewModel.FilteredGenreMaxPercentage },
                subGenre1Range = new int[] { filterViewModel.FilteredSubGenre1MinPercentage, filterViewModel.FilteredSubGenre1MaxPercentage },
                subGenre2Range = new int[] { filterViewModel.FilteredSubGenre2MinPercentage, filterViewModel.FilteredSubGenre2MaxPercentage };

            FilteredScreenplays.Clear();
            foreach (BrowseModel classifiedScreenplay in reportsViewModel.ReportsSelectionViewModel.ClassifiedScreenplays)
                FilteredScreenplays.Add(new BrowseModel(classifiedScreenplay.ScreenplayFilePath));
            CheckedScreenplays.Clear();

            OwnerFilterText = string.Format("Owner: {0}", string.IsNullOrEmpty(owner) ? "All Owners" : owner);
            GenresFilterText = string.Format("Genres: Main Genre: {0} | Subgenre 1: {1} | Subgenre 2: {2}",
                BuildGenreFilterText(genre, genreRange), BuildGenreFilterText(subGenre1, subGenre1Range),
                BuildGenreFilterText(subGenre2, subGenre2Range));

            SelectedScreenplay = -1;
            CanInspect = false;
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ArchivesInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesInspectionView.Visibility = Visibility.Collapsed);
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