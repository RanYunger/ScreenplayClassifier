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
        private string predictedGenre, predictedSubGenre1, predictedSubGenre2;
        private string actualGenre, actualSubGenre1, actualSubGenre2;
        private bool isfeedbacked;
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

        public string PredictedGenre
        {
            get { return predictedGenre; }
            set
            {
                predictedGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedGenre"));
            }
        }

        public string PredictedSubGenre1
        {
            get { return predictedSubGenre1; }
            set
            {
                predictedSubGenre1 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedSubGenre1"));
            }
        }

        public string PredictedSubGenre2
        {
            get { return predictedSubGenre2; }
            set
            {
                predictedSubGenre2 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedSubGenre2"));
            }
        }

        public string ActualGenre
        {
            get { return actualGenre; }
            set
            {
                actualGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualGenre"));
            }
        }

        public string ActualSubGenre1
        {
            get { return actualSubGenre1; }
            set
            {
                actualSubGenre1 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualSubGenre1"));
            }
        }

        public string ActualSubGenre2
        {
            get { return actualSubGenre2; }
            set
            {
                actualSubGenre2 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualSubGenre2"));
            }
        }

        public bool Isfeedbacked
        {
            get { return isfeedbacked; }
            set
            {
                isfeedbacked = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Isfeedbacked"));
            }
        }

        // Constructors
        public ScreenplayModel(string filePath, Dictionary<string, float> genrePercentages)
        {
            List<string> predictedGenres = new List<string>(genrePercentages.Keys);

            FilePath = filePath;
            Title = Path.GetFileNameWithoutExtension(filePath);

            GenrePercentages = genrePercentages;

            PredictedGenre = predictedGenres[0];
            PredictedSubGenre1 = predictedGenres[1];
            PredictedSubGenre2 = predictedGenres[2];

            ActualGenre = "Unknown";
            ActualSubGenre1 = "Unknown";
            ActualSubGenre2 = "Unknown";

            Isfeedbacked = false;
        }
    }
}