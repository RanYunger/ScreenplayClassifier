using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel : INotifyPropertyChanged
    {
        // Fields
        private int screenplayID;
        private string name;
        private string predictedGenre, predictedSubGenre1, predictedSubGenre2;
        private string actualGenre, actualSubGenre1, actualSubGenre2;
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

        // Constructors
        public ScreenplayModel(int screenplayID, string name, string predictedGenre, string predictedSubGenre1, string predictedSubGenre2)
        {
            ID = screenplayID;
            Name = name;

            PredictedGenre = predictedGenre;
            PredictedSubGenre1 = predictedSubGenre1;
            PredictedSubGenre2 = predictedSubGenre2;

            ActualGenre = "Unknown";
            ActualSubGenre1 = "Unknown";
            ActualSubGenre2 = "Unknown";
        }
    }
}
