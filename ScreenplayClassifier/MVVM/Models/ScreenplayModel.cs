using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel : INotifyPropertyChanged
    {
        // Fields
        private Dictionary<string, float> genrePercentages;
        private string filePath, title;
        private string modelGenre, modelSubGenre1, modelSubGenre2;
        private string userGenre, userSubGenre1, userSubGenre2;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public Dictionary<string, float> GenrePercentages
        {
            get { return genrePercentages; }
            set
            {
                genrePercentages = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenrePercentages"));
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

        public string Title
        {
            get { return title; }
            set
            {
                title = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }

        public string ModelGenre
        {
            get { return modelGenre; }
            set
            {
                modelGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModelGenre"));
            }
        }

        public string ModelSubGenre1
        {
            get { return modelSubGenre1; }
            set
            {
                modelSubGenre1 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModelSubGenre1"));
            }
        }

        public string ModelSubGenre2
        {
            get { return modelSubGenre2; }
            set
            {
                modelSubGenre2 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModelSubGenre2"));
            }
        }

        public string UserGenre
        {
            get { return userGenre; }
            set
            {
                userGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserGenre"));
            }
        }

        public string UserSubGenre1
        {
            get { return userSubGenre1; }
            set
            {
                userSubGenre1 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserSubGenre1"));
            }
        }

        public string UserSubGenre2
        {
            get { return userSubGenre2; }
            set
            {
                userSubGenre2 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserSubGenre2"));
            }
        }

        public bool Isfeedbacked
        {
            get { return (UserGenre != "Unknown") && (UserSubGenre1 != "Unknown") && (UserSubGenre2 != "Unknown"); }
        }

        public bool IsClassifiedCorrectly
        {
            get { return (ModelGenre == UserGenre) && (ModelSubGenre1 == UserSubGenre1) && (ModelSubGenre2 == UserSubGenre2); }
        }

        // Constructors
        public ScreenplayModel(string filePath, Dictionary<string, float> genrePercentages)
        {
            List<string> predictedGenres = new List<string>(genrePercentages.Keys);

            FilePath = filePath;
            Title = Path.GetFileNameWithoutExtension(filePath);

            GenrePercentages = genrePercentages;

            ModelGenre = predictedGenres[0];
            ModelSubGenre1 = predictedGenres[1];
            ModelSubGenre2 = predictedGenres[2];

            UserGenre = "Unknown";
            UserSubGenre1 = "Unknown";
            UserSubGenre2 = "Unknown";
        }
    }
}