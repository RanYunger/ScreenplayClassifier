using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ClassificationModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private TimeSpan duration;
        private Dictionary<string, List<int>> concordance;
        private Dictionary<string, int> wordFrequencies;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplay"));
            }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
            }
        }
        public Dictionary<string, List<int>> Concordance
        {
            get { return concordance; }
            set
            {
                concordance = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Concordance"));
            }
        }
        public Dictionary<string, int> WordFrequencies
        {
            get { return wordFrequencies; }
            set
            {
                wordFrequencies = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WordFrequencies"));
            }
        }

        // Constructors
        public ClassificationModel(ScreenplayModel screenplay)
        {
            Screenplay = screenplay;
            Duration = TimeSpan.Zero;
            Concordance = new Dictionary<string, List<int>>();
            WordFrequencies = new Dictionary<string, int>();
        }
        public ClassificationModel(ScreenplayModel screenplay, Dictionary<string, List<int>> concordance)
            :this(screenplay)
        {
            Concordance = concordance;

            WordFrequencies = new Dictionary<string, int>();
            foreach (string word in Concordance.Keys)
                WordFrequencies[word] = Concordance[word].Count;
        } 
    }
}
