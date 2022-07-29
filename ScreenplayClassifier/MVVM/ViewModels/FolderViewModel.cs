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
    public class FolderViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource folderImage, genreImage;
        private string genre, screenplaysCountText;
        private ObservableCollection<ScreenplayModel> screenplaysInGenre;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public FolderView FolderView { get; private set; }

        public ImageSource FolderImage
        {
            get { return folderImage; }
            set
            {
                folderImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FolderImage"));
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

        public string ScreenplaysCountText
        {
            get { return screenplaysCountText; }
            set
            {
                screenplaysCountText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplaysCountText"));
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
        public FolderViewModel() { }

        // Methods
        #region Commands
        public Command ShowGenreViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreView genreView = new GenreView();

                    if (screenplaysInGenre.Count == 0)
                    {
                        MessageBoxHandler.Show("There are no screenplays in this genre", string.Empty, 2, MessageBoxImage.Information);
                        return;
                    }

                    ((GenreViewModel)genreView.DataContext).Init(Genre, ScreenplaysInGenre);
                    genreView.Show();
                });
            }
        }
        #endregion

        public void Init(string genreName, ObservableCollection<ScreenplayModel> screenplaysInGenre)
        {
            string folderImageFilePath = string.Format("{0}{1}", FolderPaths.IMAGES, screenplaysInGenre.Count > 0
                ? "FullFolder.png" : "EmptyFolder.png"),
                genreImageFilePath = string.Format(@"{0}{1}.png", FolderPaths.GENREIMAGES, Genre = genreName);

            FolderImage = new BitmapImage(new Uri(folderImageFilePath));
            GenreImage = new BitmapImage(new Uri(genreImageFilePath));
            ScreenplaysCountText = string.Format("{0} {1}", screenplaysInGenre.Count, screenplaysInGenre.Count == 1
                ? "Screenplay" : "Screenplays");

            ScreenplaysInGenre = screenplaysInGenre;
        }
    }
}
