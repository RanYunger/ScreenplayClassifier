﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel : INotifyPropertyChanged
    {
        // Constants
        private static int IDGenerator = 1;

        // Fields
        private int screenplayID;
        private string title;
        private string predictedGenre, predictedSubGenre1, predictedSubGenre2;
        private string actualGenre, actualSubGenre1, actualSubGenre2;
        private Dictionary<string, float> genrePercentages;
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

        // Constructors
        public ScreenplayModel(string title, Dictionary<string, float> genrePercentages)
        {
            List<string> predictedGenres = new List<string>(genrePercentages.Keys);

            ID = IDGenerator++;
            Title = title;

            GenrePercentages = genrePercentages;

            PredictedGenre = predictedGenres[0];
            PredictedSubGenre1 = predictedGenres[1];
            PredictedSubGenre2 = predictedGenres[2];

            ActualGenre = "Unknown";
            ActualSubGenre1 = "Unknown";
            ActualSubGenre2 = "Unknown";
        }
    }
}
