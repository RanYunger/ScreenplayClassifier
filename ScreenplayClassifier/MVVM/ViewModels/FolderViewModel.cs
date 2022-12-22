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
        private ImageSource genreGif;
        private string genre;
        private ObservableCollection<ScreenplayModel> screenplaysInGenre;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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
        public FolderViewModel() { }

        // Methods
        #region Commands
        public Command ShowGenreViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreView genreView = null;

                    if (screenplaysInGenre.Count == 0)
                        MessageBoxHandler.Show(string.Format("No {0} screenplays to show", genre), "Error", 2, MessageBoxImage.Stop);
                    else
                    {
                        foreach (Window view in App.Current.Windows)
                        {
                            if (view is GenreView)
                            {
                                view.Focus();
                                return;
                            }
                        }

                        genreView = new GenreView();
                        ((GenreViewModel)genreView.DataContext).Init(Genre, ScreenplaysInGenre);
                        genreView.Show();
                    }
                });
            }
        }
        #endregion

        public void Init(string genreName, ObservableCollection<ScreenplayModel> screenplaysInGenre)
        {
            string folderImageFilePath = string.Format("{0}{1}", FolderPaths.IMAGES, screenplaysInGenre.Count > 0
                ? "FullFolder.png" : "EmptyFolder.png"),
                genreImageFilePath = string.Format(@"{0}{1}.gif", FolderPaths.GENREGIFS, Genre = genreName);

            GenreGif = new BitmapImage(new Uri(genreImageFilePath));

            ScreenplaysInGenre = screenplaysInGenre;
        }
    }
}
