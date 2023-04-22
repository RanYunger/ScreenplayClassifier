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
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ScreenplaysSelectionViewModel : PropertyChangeNotifier
    {
        // Fields
        private Predicate<object> titleFilter;
        private MediaPlayer mediaPlayer;

        private ObservableCollection<SelectionEntryModel> selectionEntries;
        private ImageSource genreGif;
        private string noScreenplaysMessage, selectionGoal;
        private int selectedScreenplay;
        private bool? allSelected;
        private bool allSelectedChanging, hasEntries, hasSelections, isFilteredByGenre;

        // Properties
        public ScreenplaysSelectionView ScreenplaysSelectionView { get; private set; }

        public ObservableCollection<SelectionEntryModel> SelectionEntries
        {
            get { return selectionEntries; }
            set
            {
                selectionEntries = value;

                NotifyPropertyChange();
            }
        }

        public ImageSource GenreGif
        {
            get { return genreGif; }
            set
            {
                genreGif = value;

                NotifyPropertyChange();
            }
        }

        public string NoScreenplaysMessage
        {
            get { return noScreenplaysMessage; }
            set
            {
                noScreenplaysMessage = value;

                NotifyPropertyChange();
            }
        }

        public string SelectionGoal
        {
            get { return selectionGoal; }
            set
            {
                selectionGoal = value;

                NotifyPropertyChange();
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                NotifyPropertyChange();
            }
        }

        public bool? AllSelected
        {
            get { return allSelected; }
            set
            {
                // Validation (property changes only if a different value is assigned)
                if (allSelected == value)
                    return;

                allSelected = value;

                // Sets all the other Checkboxes
                AllSelectedChanging = true;

                if (allSelected != null)
                    foreach (SelectionEntryModel entry in SelectionEntries)
                        entry.IsChecked = allSelected.Value;

                AllSelectedChanging = false;

                HasSelections = new List<SelectionEntryModel>(selectionEntries).Exists(entry => entry.IsChecked);

                NotifyPropertyChange();
            }
        }

        public bool AllSelectedChanging
        {
            get { return allSelectedChanging; }
            set
            {
                allSelectedChanging = value;

                NotifyPropertyChange();
            }
        }

        public bool HasEntries
        {
            get { return hasEntries; }
            set
            {
                hasEntries = value;

                NotifyPropertyChange();
            }
        }

        public bool HasSelections
        {
            get { return hasSelections; }
            set
            {
                hasSelections = value;

                NotifyPropertyChange();
            }
        }

        public bool IsFilteredByGenre
        {
            get { return isFilteredByGenre; }
            set
            {
                isFilteredByGenre = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ScreenplaysSelectionViewModel()
        {
            mediaPlayer = new MediaPlayer();
        }

        // Methods
        #region Commands
        public Command StopMusicCommand
        {
            get { return new Command(() => mediaPlayer.Stop()); }
        }

        public Command EnterTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string usernameInput = string.Empty;

                    // Validation
                    if (ScreenplaysSelectionView == null)
                        return;

                    titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
                    usernameInput = titleTextBox.Text;

                    if (string.Equals(usernameInput, "Search screenplay by title"))
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
                    string titleInput = string.Empty;

                    if (ScreenplaysSelectionView == null)
                        return;

                    titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
                    titleInput = titleTextBox.Text;

                    if (string.IsNullOrEmpty(titleInput))
                    {
                        titleTextBox.Foreground = Brushes.Gray;
                        titleTextBox.Text = "Search screenplay by title";
                    }
                });
            }
        }

        public Command SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    TextBox titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
                    ICollectionView screenplaysCollectionView = CollectionViewSource.GetDefaultView(SelectionEntries);
                    string titleInput = titleTextBox.Text;

                    // Updates and activates the filter
                    titleFilter = (o) =>
                    {
                        return (string.IsNullOrEmpty(titleInput.Trim())) || (string.Equals(titleInput, "Search screenplay by title"))
                            ? true : ((SelectionEntryModel)o).ScreenplayFileName.Contains(titleInput);
                    };
                    screenplaysCollectionView.Filter = (o) => { return titleFilter.Invoke(o); };
                    screenplaysCollectionView.Refresh();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// <param name="screenplaySelectionView">The view to obtain controls from</param>
        /// </summary>
        public void Init(ScreenplaysSelectionView screenplaysSelectionView)
        {
            StopMusicCommand.Execute(null); // For silent initiation

            ScreenplaysSelectionView = screenplaysSelectionView;

            SelectionEntries = new ObservableCollection<SelectionEntryModel>();

            SelectionGoal = "classify";
            NoScreenplaysMessage = string.Empty;

            SelectedScreenplay = -1;

            AllSelected = false;
            AllSelectedChanging = false;
            HasEntries = false;
            HasSelections = false;
        }

        /// <summary>
        /// Refreshes the view.
        /// <param name="entries">The screenplay entires to show in the view</param>
        /// <param name="selectionGoal">The goad this view is used for: classify / inspect</param>
        /// <param name="noResultsMessage">The message indicating there are no results to show</param>
        /// </summary>
        public void RefreshView(ObservableCollection<SelectionEntryModel> entries, string selectionGoal, string noResultsMessage)
        {
            TextBox titleTextBox = null;

            if (string.Equals(selectionGoal, "inspect"))
                ClearEntries();
            foreach (SelectionEntryModel entry in entries)
                AddEntry(entry);

            SelectionGoal = selectionGoal;
            NoScreenplaysMessage = HasEntries ? string.Empty : noResultsMessage;
            SelectedScreenplay = -1;

            //IsFilteredByGenre = !string.IsNullOrEmpty(filteredGenre);
            //if (IsFilteredByGenre)
            //{
            //    GenreGif = new BitmapImage(new Uri(string.Format(@"{0}{1}.gif", FolderPaths.GENREGIFS, filteredGenre)));

            //    mediaPlayer.Open(new Uri(string.Format("{0}{1}.mp3", FolderPaths.GENREAUDIOS, filteredGenre)));
            //    mediaPlayer.Play();
            //}

            titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
            titleTextBox.Foreground = Brushes.Gray;
            titleTextBox.Text = "Search screenplay by title";
        }

        /// <summary>
        /// Adds an entry to the entries collection.
        /// </summary>
        /// <param name="entry">The entry to add</param>
        public void AddEntry(SelectionEntryModel entry)
        {
            if (AllSelected != null)
                entry.IsChecked = AllSelected.Value;

            // Listens to changes in IsChecked property
            entry.PropertyChanged += EntryPropertyChanged;

            SelectionEntries.Add(entry);

            HasEntries = true;
            HasSelections = new List<SelectionEntryModel>(selectionEntries).Exists(entry => entry.IsChecked);
        }

        /// <summary>
        /// Removes an entry to the entries collection.
        /// </summary>
        /// <param name="entry">The entry to remove</param>
        public void RemoveEntry(SelectionEntryModel entry)
        {
            SelectionEntries.Remove(entry);

            HasEntries = SelectionEntries.Count > 0;
            HasSelections = new List<SelectionEntryModel>(selectionEntries).Exists(entry => entry.IsChecked);
        }

        /// <summary>
        /// Clears the entries collection.
        /// </summary>
        public void ClearEntries()
        {
            while (HasEntries)
                RemoveEntry(SelectionEntries[0]);
        }

        /// <summary>
        /// Rechecks the All Selected Checkbox. 
        /// </summary>
        private void RecheckAllSelected()
        {
            List<SelectionEntryModel> entriesList = null;

            // Validation (Has this change been caused by some other change?)
            if (AllSelectedChanging)
                return;

            AllSelectedChanging = true;

            entriesList = new List<SelectionEntryModel>(SelectionEntries);
            if (entriesList.TrueForAll(entry => entry.IsChecked))
                AllSelected = true;
            else if (entriesList.TrueForAll(entry => !entry.IsChecked))
                AllSelected = false;
            else
                AllSelected = null;

            AllSelectedChanging = false;
        }

        private void EntryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // Re-checks only if the IsChecked property changed
            if (args.PropertyName == nameof(SelectionEntryModel.IsChecked))
                RecheckAllSelected();

            HasSelections = new List<SelectionEntryModel>(selectionEntries).Exists(entry => entry.IsChecked);
        }
    }
}