using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ArchivesByGenreViewModel : INotifyPropertyChanged
    {
        // Fields
        private Dictionary<string, ObservableCollection<ScreenplayModel>> archives;
        private ImageSource genreImage;
        private int genreOffset;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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

        public ImageSource GenreImage
        {
            get { return genreImage; }
            set
            {
                genreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreImage"));
            }
        }

        public int GenreOffset
        {
            get { return genreOffset; }
            set
            {
                string selectedGenre = null;

                genreOffset = value;

                if (genreOffset != -1)
                {
                    selectedGenre = new List<string>(Archives.Keys)[genreOffset];

                    GenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, selectedGenre)));
                    ShowScreenplays(selectedGenre);
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreOffset"));
            }
        }

        // Constructors
        public ArchivesByGenreViewModel() { }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Keyboard.IsKeyDown(Key.Left))
                        GoToPreviousCommand.Execute(null);
                    else if (Keyboard.IsKeyDown(Key.Right))
                        GoToNextCommand.Execute(null);
                });
            }
        }
        public Command GoToPreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (Archives.Count == 0)
                        return;

                    GenreOffset = GenreOffset - 1 < 0 ? Archives.Count - 1 : GenreOffset - 1;
                });
            }
        }
        public Command GoToNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (Archives.Count == 0)
                        return;

                    GenreOffset = GenreOffset + 1 > Archives.Count - 1 ? 0 : GenreOffset + 1;
                });
            }
        }

        public Command StopMusicCommand
        {
            get
            {
                GenreView genreView = null;
                GenreViewModel genreViewModel = null;

                return new Command(() =>
                {
                    // Validation
                    if (ArchivesByGenreView == null)
                        return;

                    genreView = (GenreView)ArchivesByGenreView.FindName("GenreView");
                    genreViewModel = (GenreViewModel)genreView.DataContext;

                    genreViewModel.StopMusicCommand.Execute(null);
                });
            }
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
            GenreOffset = -1;
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
            GenreViewModel genreViewModel = null;

            // Validation
            if (ArchivesByGenreView == null)
                return;

            genreView = (GenreView)ArchivesByGenreView.FindName("GenreView");
            genreViewModel = (GenreViewModel)genreView.DataContext;

            genreViewModel.Init(genreView, genre, Archives[genre]);
        }
    }
}
