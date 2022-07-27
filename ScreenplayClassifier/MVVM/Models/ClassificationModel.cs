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

        // Constructors
        public ClassificationModel(ScreenplayModel screenplay)
        {
            Screenplay = screenplay;
        }
    }
}
