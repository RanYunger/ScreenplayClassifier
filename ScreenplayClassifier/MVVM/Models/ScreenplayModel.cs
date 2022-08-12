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
        private string[] predictedGenres, actualGenres;
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
        public string PredictedGenre
        {
            get { return predictedGenres[0]; }
            set
            {
                predictedGenres[0] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedGenre"));
            }
        }

        public string PredictedSubGenre1
        {
            get { return predictedGenres[1]; }
            set
            {
                predictedGenres[1] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedSubGenre1"));
            }
        }

        public string PredictedSubGenre2
        {
            get { return predictedGenres[2]; }
            set
            {
                predictedGenres[2] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedSubGenre2"));
            }
        }
        public string ActualGenre
        {
            get { return actualGenres[0]; }
            set
            {
                actualGenres[0] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualGenre"));
            }
        }

        public string ActualSubGenre1
        {
            get { return actualGenres[1]; }
            set
            {
                actualGenres[1] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualSubGenre1"));
            }
        }

        public string ActualSubGenre2
        {
            get { return actualGenres[2]; }
            set
            {
                actualGenres[2] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualSubGenre2"));
            }
        }

        // Constructors
        public ScreenplayModel(string name, string filePath)
        {
            predictedGenres = new string[3];
            actualGenres = new string[3];

            ID = IDGenerator++;
            Name = name;
            FilePath = filePath;
            ActualGenre = ActualSubGenre1 = ActualSubGenre2 = "Unknown";
            PredictedGenre = PredictedSubGenre1 = PredictedSubGenre2 = "Unknown";
        }
    }
}
