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

        // Constructors
        public ClassificationModel(ScreenplayModel screenplay)
        {
            Screenplay = screenplay;
            Duration = TimeSpan.Zero;
        } 
    }
}
