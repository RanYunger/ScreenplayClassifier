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
        private string ownerGenre, ownerSubGenre1, ownerSubGenre2;

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

        public string OwnerGenre
        {
            get { return ownerGenre; }
            set
            {
                ownerGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerGenre"));
            }
        }

        public string OwnerSubGenre1
        {
            get { return ownerSubGenre1; }
            set
            {
                ownerSubGenre1 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre1"));
            }
        }

        public string OwnerSubGenre2
        {
            get { return ownerSubGenre2; }
            set
            {
                ownerSubGenre2 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre2"));
            }
        }

        public bool Isfeedbacked
        {
            get { return (OwnerGenre != "Unknown") && (OwnerSubGenre1 != "Unknown") && (OwnerSubGenre2 != "Unknown"); }
        }

        public bool IsClassifiedCorrectly
        {
            get { return (ModelGenre == OwnerGenre) && (ModelSubGenre1 == OwnerSubGenre1) && (ModelSubGenre2 == OwnerSubGenre2); }
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

            OwnerGenre = "Unknown";
            OwnerSubGenre1 = "Unknown";
            OwnerSubGenre2 = "Unknown";
        }
    }
}