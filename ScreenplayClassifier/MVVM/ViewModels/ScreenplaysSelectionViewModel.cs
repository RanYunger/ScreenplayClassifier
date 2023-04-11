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
    public class ScreenplaysSelectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private Predicate<object> titleFilter;
        private MediaPlayer mediaPlayer;

        private ObservableCollection<SelectionModel> classifiedScreenplays, checkedScreenplays;
        private ImageSource genreGif;
        private string noScreenplaysMessage;
        private int selectedScreenplay;
        private bool canSelect, canInspect, isFilteredByGenre;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ScreenplaysSelectionView ScreenplaysSelectionView { get; private set; }

        public ObservableCollection<SelectionModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public ObservableCollection<SelectionModel> CheckedScreenplays
        {
            get { return checkedScreenplays; }
            set
            {
                checkedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedScreenplays"));
            }
        }

        public ImageSource GenreGif
        {
            get { return genreGif; }
            set
            {
                genreGif = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreGif"));
            }
        }

        public string NoScreenplaysMessage
        {
            get { return noScreenplaysMessage; }
            set
            {
                noScreenplaysMessage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NoScreenplaysMessage"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if (selectedScreenplay != -1)
                    CheckSelectionCommand.Execute(null);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanSelect
        {
            get { return canSelect; }
            set
            {
                canSelect = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanSelect"));
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

        public bool IsFilteredByGenre
        {
            get { return isFilteredByGenre; }
            set
            {
                isFilteredByGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsFilteredByGenre"));
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
                    ICollectionView screenplaysCollectionView = CollectionViewSource.GetDefaultView(ClassifiedScreenplays);
                    string titleInput = titleTextBox.Text;

                    // Updates and activates the filter
                    titleFilter = (o) =>
                    {
                        return (string.IsNullOrEmpty(titleInput.Trim())) || (string.Equals(titleInput, "Search screenplay by title"))
                            ? true : ((SelectionModel)o).ScreenplayFileName.Contains(titleInput);
                    };
                    screenplaysCollectionView.Filter = (o) => { return titleFilter.Invoke(o); };
                    screenplaysCollectionView.Refresh();
                });
            }
        }

        public Command CheckSelectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectionModel chosenScreenplay = null;

                    // Validation
                    if ((SelectedScreenplay == -1) || (ClassifiedScreenplays.Count == 0))
                        return;

                    chosenScreenplay = ClassifiedScreenplays[SelectedScreenplay];
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
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// <param name="screenplaySelectionView">The view to obtain controls from</param>
        /// </summary>
        public void Init(ScreenplaysSelectionView screenplaysSelectionView)
        {
            StopMusicCommand.Execute(null); // For silent initiation

            ScreenplaysSelectionView = screenplaysSelectionView;
        }

        /// <summary>
        /// Refreshes the view.
        /// <param name="screenplays">The screenplays to show in the view</param>
        /// <param name="noResultsMessage">The message indicating there are no results to show</param>
        /// <param name="filteredGenre">The main genre the screenplays are filtered by</param>
        /// </summary>
        public void RefreshView(ObservableCollection<ReportModel> screenplays, string noResultsMessage, string filteredGenre)
        {
            TextBox titleTextBox = null;

            ClassifiedScreenplays = new ObservableCollection<SelectionModel>();
            foreach (ReportModel report in screenplays)
                ClassifiedScreenplays.Add(new SelectionModel(report.Owner.Username, report.Screenplay.FilePath));

            CheckedScreenplays = new ObservableCollection<SelectionModel>();

            NoScreenplaysMessage = ClassifiedScreenplays.Count > 0 ? string.Empty : noResultsMessage;
            SelectedScreenplay = -1;

            IsFilteredByGenre = !string.IsNullOrEmpty(filteredGenre);
            if (IsFilteredByGenre)
            {
                GenreGif = new BitmapImage(new Uri(string.Format(@"{0}{1}.gif", FolderPaths.GENREGIFS, filteredGenre)));

                mediaPlayer.Open(new Uri(string.Format("{0}{1}.mp3", FolderPaths.GENREAUDIOS, filteredGenre)));
                mediaPlayer.Play();
            }

            CanSelect = ClassifiedScreenplays.Count > 0;
            CanInspect = false;

            titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
            titleTextBox.Foreground = Brushes.Gray;
            titleTextBox.Text = "Search screenplay by title";
        }
    }
}