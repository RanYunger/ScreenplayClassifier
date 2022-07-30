using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel : INotifyPropertyChanged
    {
        // Fields
        private static int IDGenerator = 0;

        private int screenplayID;
        private string name, filePath;
        private string[] genres;
        private ImageSource genreImage, subgenre1Image, subGenre2Image;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public int ID
        {
            get { return screenplayID; }
            set
            {
                screenplayID = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilePath"));
            }
        }

        public string Genre
        {
            get { return genres[0]; }
            set
            {
                string imagePath = string.Empty;

                genres[0] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Genre"));

                imagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, genres[0]);
                GenreImage = new BitmapImage(new Uri(imagePath));
            }
        }
        public string SubGenre1
        {
            get { return genres[1]; }
            set
            {
                string imagePath = string.Empty;

                genres[1] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1"));

                imagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, genres[1]);
                SubGenre1Image = new BitmapImage(new Uri(imagePath));
            }
        }
        public string SubGenre2
        {
            get { return genres[2]; }
            set
            {
                string imagePath = string.Empty;

                genres[2] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2"));

                imagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, genres[2]);
                SubGenre2Image = new BitmapImage(new Uri(imagePath));
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

        public ImageSource SubGenre1Image
        {
            get { return subgenre1Image; }
            set
            {
                subgenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenreImage1"));
            }
        }

        public ImageSource SubGenre2Image
        {
            get { return subGenre2Image; }
            set
            {
                subGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2Image"));
            }
        }

        // Constructors
        public ScreenplayModel(string name, string filePath)
        {
            genres = new string[3];

            ID = IDGenerator++;
            Name = name;
            FilePath = filePath;
            Genre = SubGenre1 = SubGenre2 = "Unknown";
        }
    }
}
