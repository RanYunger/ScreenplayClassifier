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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReelViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource genreImage;
        private string genre;
        private ObservableCollection<ScreenplayModel> screenplaysInGenre;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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

        public string Genre
        {
            get { return genre; }
            set
            {
                genre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Genre"));
            }
        }

        public ObservableCollection<ScreenplayModel> ScreenplaysInGenre
        {
            get { return screenplaysInGenre; }
            set
            {
                screenplaysInGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplaysInGenre"));
            }
        }

        // Constructors
        public ReelViewModel() { }

        // Methods
        #region Commands
        public Command ShowGenreViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreView genreView = null;

                    // Focuses on an open GenreView (if there's one)
                    foreach (Window view in App.Current.Windows)
                        if (view is GenreView)
                        {
                            view.Focus();
                            return;
                        }

                    // Opens a new GenreView to the genre
                    genreView = new GenreView();
                    ((GenreViewModel)genreView.DataContext).Init(Genre, ScreenplaysInGenre);
                    genreView.Show();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="genreName">The genre represented by the TicketView</param>
        /// <param name="screenplaysInGenre">Collection of screenplays whose main genre is <genreName></param>
        public void Init(string genreName, ObservableCollection<ScreenplayModel> screenplaysInGenre)
        {
            string genreImageFilePath = string.Format(@"{0}{1}.png", FolderPaths.GENREIMAGES, Genre = genreName);

            GenreImage = new BitmapImage(new Uri(genreImageFilePath));

            ScreenplaysInGenre = screenplaysInGenre;
        }
    }
}
