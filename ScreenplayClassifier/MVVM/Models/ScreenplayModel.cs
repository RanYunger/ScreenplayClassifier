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
                genres[0] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Genre"));
            }
        }
        public string SubGenre1
        {
            get { return genres[1]; }
            set
            {
                genres[1] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1"));
            }
        }
        public string SubGenre2
        {
            get { return genres[2]; }
            set
            {
                genres[2] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2"));
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
