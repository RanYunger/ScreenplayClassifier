using NumericUpDownLib;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesFilterViewModel : PropertyChangeNotifier
    {
        // Fields
        private Predicate<object> ownerFilter;
        private Predicate<object> genreFilter, genrePercentageFilter;
        private Predicate<object> subGenre1Filter, subGenre1PercentageFilter;
        private Predicate<object> subGenre2Filter, subGenre2PercentageFilter;

        private ObservableCollection<string> ownerOptions, genreOptions, subGenre1Options, subGenre2Options;
        private string filteredOwner, filteredGenre, filteredSubGenre1, filteredSubGenre2;
        private int filteredGenreMinPercentage, filteredSubGenre1MinPercentage, filteredSubGenre2MinPercentage;
        private int filteredGenreMaxPercentage, filteredSubGenre1MaxPercentage, filteredSubGenre2MaxPercentage;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesFilterView ArchivesFilterView { get; private set; }
        public Predicate<object> Filter { get; private set; }

        public ObservableCollection<string> OwnerOptions
        {
            get { return ownerOptions; }
            set
            {
                ownerOptions = value;

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<string> GenreOptions
        {
            get { return genreOptions; }
            set
            {
                genreOptions = value;

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<string> SubGenre1Options
        {
            get { return subGenre1Options; }
            set
            {
                subGenre1Options = value;

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<string> SubGenre2Options
        {
            get { return subGenre2Options; }
            set
            {
                subGenre2Options = value;

                NotifyPropertyChange();
            }
        }

        public string FilteredOwner
        {
            get { return filteredOwner; }
            set
            {
                filteredOwner = value;

                NotifyPropertyChange();
            }
        }

        public string FilteredGenre
        {
            get { return filteredGenre; }
            set
            {
                filteredGenre = value;

                SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                SubGenre1Options.Remove(filteredGenre);
                SubGenre1Options.Remove(FilteredSubGenre2);

                SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);
                SubGenre2Options.Remove(filteredGenre);
                SubGenre2Options.Remove(FilteredSubGenre1);

                NotifyPropertyChange();
            }
        }

        public string FilteredSubGenre1
        {
            get { return filteredSubGenre1; }
            set
            {
                filteredSubGenre1 = value;

                GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                GenreOptions.Remove(filteredSubGenre1);
                GenreOptions.Remove(FilteredSubGenre2);

                SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);
                SubGenre2Options.Remove(filteredSubGenre1);
                SubGenre2Options.Remove(FilteredGenre);

                NotifyPropertyChange();
            }
        }

        public string FilteredSubGenre2
        {
            get { return filteredSubGenre2; }
            set
            {
                filteredSubGenre2 = value;

                GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                GenreOptions.Remove(filteredSubGenre2);
                GenreOptions.Remove(FilteredSubGenre1);

                SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                SubGenre1Options.Remove(filteredSubGenre2);
                SubGenre1Options.Remove(FilteredGenre);

                NotifyPropertyChange();
            }
        }

        public int FilteredGenreMinPercentage
        {
            get { return filteredGenreMinPercentage; }
            set
            {
                filteredGenreMinPercentage = value;

                NotifyPropertyChange();
            }
        }

        public int FilteredSubGenre1MinPercentage
        {
            get { return filteredSubGenre1MinPercentage; }
            set
            {
                filteredSubGenre1MinPercentage = value;

                NotifyPropertyChange();
            }
        }

        public int FilteredSubGenre2MinPercentage
        {
            get { return filteredSubGenre2MinPercentage; }
            set
            {
                filteredSubGenre2MinPercentage = value;

                NotifyPropertyChange();
            }
        }

        public int FilteredGenreMaxPercentage
        {
            get { return filteredGenreMaxPercentage; }
            set
            {
                filteredGenreMaxPercentage = value;

                NotifyPropertyChange();
            }
        }

        public int FilteredSubGenre1MaxPercentage
        {
            get { return filteredSubGenre1MaxPercentage; }
            set
            {
                filteredSubGenre1MaxPercentage = value;

                NotifyPropertyChange();
            }
        }

        public int FilteredSubGenre2MaxPercentage
        {
            get { return filteredSubGenre2MaxPercentage; }
            set
            {
                filteredSubGenre2MaxPercentage = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ArchivesFilterViewModel() { }

        // Methods
        #region Commands
        public Command ChangeFilteredGenrePercentageRangeCommand
        {
            get
            {
                return new Command(() =>
                {
                    NumericUpDown minPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredGenreMinPercentageNumericUpDown"),
                        maxPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredGenreMaxPercentageNumericUpDown");

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
                    NumericUpDown minPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre1MinPercentageNumericUpDown"),
                        maxPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre1MaxPercentageNumericUpDown");

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
                    NumericUpDown minPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre2MinPercentageNumericUpDown"),
                        maxPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre2MaxPercentageNumericUpDown");

                    FilteredSubGenre2MinPercentage = minPercentageNumericUpDown.Value;
                    FilteredSubGenre2MaxPercentage = maxPercentageNumericUpDown.Value;
                });
            }
        }

        public Command FilterArchivesCommand
        {
            get
            {
                RadioButton allOwnersRadioButton = null;

                return new Command(() =>
                {
                    // Validation
                    if (ArchivesFilterView == null)
                        return;

                    allOwnersRadioButton = (RadioButton)ArchivesFilterView.FindName("AllOwnersRadioButton");
                    FilteredOwner = allOwnersRadioButton.IsChecked.Value ? string.Empty : FilteredOwner;

                    // Updates all filters
                    ownerFilter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredOwner) ? true : ((ReportModel)o).Owner.Username == FilteredOwner;
                    };
                    genreFilter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredGenre) ? true : ((ReportModel)o).Screenplay.OwnerGenre == FilteredGenre;
                    };
                    subGenre1Filter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredSubGenre1) ? true : ((ReportModel)o).Screenplay.OwnerSubGenre1 == FilteredSubGenre1;
                    };
                    subGenre2Filter = (o) =>
                    {
                        return string.IsNullOrEmpty(FilteredSubGenre2) ? true : ((ReportModel)o).Screenplay.OwnerSubGenre2 == FilteredSubGenre2;
                    };

                    genrePercentageFilter = (o) =>
                    {
                        ScreenplayModel screenplay = ((ReportModel)o).Screenplay;
                        float genrePercentage = screenplay.GenrePercentages[screenplay.OwnerGenre];

                        return string.IsNullOrEmpty(FilteredGenre) ? true
                            : (genrePercentage >= FilteredGenreMinPercentage) && (genrePercentage <= FilteredGenreMaxPercentage);
                    };
                    subGenre1PercentageFilter = (o) =>
                    {
                        ScreenplayModel screenplay = ((ReportModel)o).Screenplay;
                        float subGenre1Percentage = screenplay.GenrePercentages[screenplay.OwnerSubGenre1];

                        return string.IsNullOrEmpty(FilteredSubGenre1) ? true
                            : (subGenre1Percentage >= FilteredSubGenre1MinPercentage) && (subGenre1Percentage <= FilteredSubGenre1MaxPercentage);
                    };
                    subGenre2PercentageFilter = (o) =>
                    {
                        ScreenplayModel screenplay = ((ReportModel)o).Screenplay;
                        float subGenre2Percentage = screenplay.GenrePercentages[screenplay.OwnerSubGenre2];

                        return string.IsNullOrEmpty(FilteredSubGenre2) ? true
                            : (subGenre2Percentage >= FilteredSubGenre2MinPercentage) && (subGenre2Percentage <= FilteredSubGenre2MaxPercentage);
                    };

                    Filter = (o) =>
                    {
                        return (ownerFilter.Invoke(o)) && (genreFilter.Invoke(o)) && (subGenre1Filter.Invoke(o)) && (subGenre2Filter.Invoke(o))
                            && (genrePercentageFilter.Invoke(o)) && (subGenre1PercentageFilter.Invoke(o)) && (subGenre2PercentageFilter.Invoke(o));
                    };

                    ShowInspectionViewCommand.Execute(null);
                });
            }
        }

        public Command ClearFilterCommand
        {
            get
            {
                RadioButton allOwnersRadioButton = null;
                NumericUpDown filteredGenreMinPercentageNumericUpDown = null, filteredGenreMaxPercentageNumericUpDown = null,
                    filteredSubGenre1MinPercentageNumericUpDown = null, filteredSubGenre1MaxPercentageNumericUpDown = null,
                    filteredSubGenre2MinPercentageNumericUpDown = null, filteredSubGenre2MaxPercentageNumericUpDown = null;

                return new Command(() =>
                {
                    // Validation
                    if (ArchivesFilterView == null)
                        return;

                    allOwnersRadioButton = (RadioButton)ArchivesFilterView.FindName("AllOwnersRadioButton");
                    allOwnersRadioButton.IsChecked = true;

                    filteredGenreMinPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredGenreMinPercentageNumericUpDown");
                    filteredGenreMaxPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredGenreMaxPercentageNumericUpDown");
                    filteredSubGenre1MinPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre1MinPercentageNumericUpDown");
                    filteredSubGenre1MaxPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre1MaxPercentageNumericUpDown");
                    filteredSubGenre2MinPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre2MinPercentageNumericUpDown");
                    filteredSubGenre2MaxPercentageNumericUpDown = (NumericUpDown)ArchivesFilterView.FindName("FilteredSubGenre2MaxPercentageNumericUpDown");

                    Filter = (o) => { return true; }; // Default filter

                    GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    FilteredOwner = string.Empty;
                    FilteredGenre = string.Empty;
                    FilteredSubGenre1 = string.Empty;
                    FilteredSubGenre2 = string.Empty;

                    filteredGenreMinPercentageNumericUpDown.Value = FilteredGenreMinPercentage = 0;
                    filteredGenreMaxPercentageNumericUpDown.Value = FilteredGenreMaxPercentage = 100;
                    filteredSubGenre1MinPercentageNumericUpDown.Value = FilteredSubGenre1MinPercentage = 0;
                    filteredSubGenre1MaxPercentageNumericUpDown.Value = FilteredSubGenre1MaxPercentage = 100;
                    filteredSubGenre2MinPercentageNumericUpDown.Value = FilteredSubGenre2MinPercentage = 0;
                    filteredSubGenre2MaxPercentageNumericUpDown.Value = FilteredSubGenre2MaxPercentage = 100;
                });
            }
        }

        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ArchivesFilterView == null)
                        return;

                    HideView();
                    ArchivesViewModel.ArchivesInspectionViewModel.RefreshView();
                    ArchivesViewModel.ArchivesInspectionViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="archivesFilterView">The view to obtain controls from</param>
        /// <param name="archivesViewModel">The view model which manages the archives module</param>
        public void Init(ArchivesFilterView archivesFilterView, ArchivesViewModel archivesViewModel)
        {
            ArchivesFilterView = archivesFilterView;
            ArchivesViewModel = archivesViewModel;

            Filter = (o) => { return true; }; // Default filter

            InitOwners();

            GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
            SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
            SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ArchivesFilterView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesFilterView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ArchivesFilterView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesFilterView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Initiates the owner options.
        /// </summary>
        private void InitOwners()
        {
            UserModel user = ArchivesViewModel.MainViewModel.UserToolbarViewModel.User;

            OwnerOptions = new ObservableCollection<string>() { user.Username };

            if (user.Role == UserModel.UserRole.ADMIN)
            {
                OwnerOptions.Clear();
                foreach (UserModel owner in JSON.LoadedUsers)
                    OwnerOptions.Add(owner.Username);
            }
        }
    }
}