using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        public string[] PredictedGenres
        {
            get { return predictedGenres; }
            set
            {
                predictedGenres = value;

                if (predictedGenres != null)
                {
                    PredictedGenre = predictedGenres[0];
                    PredictedSubGenre1 = predictedGenres[1];
                    PredictedSubGenre2 = predictedGenres[2];
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PredictedGenres"));
            }
        }

        public string[] ActualGenres
        {
            get { return actualGenres; }
            set
            {
                actualGenres = value;

                if (actualGenres != null)
                {
                    ActualGenre = actualGenres[0];
                    ActualSubGenre1 = actualGenres[1];
                    ActualSubGenre2 = actualGenres[2];
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualGenres"));
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
        public ScreenplayModel(string name, string filePath, string[] predictedGenres, string[] actualGenres)
        {
            ID = IDGenerator++;
            Name = name;
            FilePath = filePath;

            PredictedGenres = predictedGenres;
            ActualGenres = actualGenres;
        }
    }
}
