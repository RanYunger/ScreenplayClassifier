using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ArchivesByGenreViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        private Dictionary<string, ObservableCollection<ScreenplayModel>> archives;
        public ArchivesByGenreView ArchivesByGenreView { get; private set; }

        public Dictionary<string, ObservableCollection<ScreenplayModel>> Archives
        {
            get { return archives; }
            set
            {
                archives = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Archives"));
            }
        }

        // Constructors
        public ArchivesByGenreViewModel() { }

        // Methods
        #region Commands
        public Command ShowActionScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Action")); }
        }

        public Command ShowAdventureScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Adventure")); }
        }

        public Command ShowComedyScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Comedy")); }
        }

        public Command ShowCrimeScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Crime")); }
        }

        public Command ShowDramaScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Drama")); }
        }

        public Command ShowFamilyScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Family")); }
        }

        public Command ShowFantasyScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Fantasy")); }
        }

        public Command ShowHorrorScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Horror")); }
        }

        public Command ShowRomanceScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Romance")); }
        }

        public Command ShowSciFiScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("SciFi")); }
        }

        public Command ShowThrillerScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("Thriller")); }
        }

        public Command ShowWarScreenplaysCommand
        {
            get { return new Command(() => ShowScreenplays("War")); }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="archivesByGenreView">The view to obtain controls from</param>
        public void Init(ArchivesByGenreView archivesByGenreView)
        {
            ArchivesByGenreView = archivesByGenreView;

            Archives = new Dictionary<string, ObservableCollection<ScreenplayModel>>();
        }

        /// <summary>
        /// Refreshes the archives
        /// </summary>
        /// <param name="genres">List of genre labels</param>
        /// <param name="screenplays">List of screenplays to be categorized into genres</param>
        public void RefreshArchives(ObservableCollection<string> genres, List<ScreenplayModel> screenplays)
        {
            List<ScreenplayModel> genreScreenplays;

            foreach (string genreName in genres)
            {
                genreScreenplays = screenplays.FindAll(screenplay => screenplay.OwnerGenre == genreName);
                Archives[genreName] = new ObservableCollection<ScreenplayModel>(genreScreenplays);
            }
        }

        /// <summary>
        /// Shows screenplays of a given genre.
        /// </summary>
        /// <param name="genre">The screenplay's genre</param>
        private void ShowScreenplays(string genre)
        {
            GenreView genreView = null;

            // Validation
            if (ArchivesByGenreView == null)
                return;

            genreView = (GenreView)ArchivesByGenreView.FindName("GenreView");

            ((GenreViewModel)genreView.DataContext).Init(genreView, genre, Archives[genre]);
        }
    }
}
