using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class CircularProgressBarViewModel : INotifyPropertyChanged
    {
        // Fields
        private int percent;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public CircularProgressBarView CircularProgressBarView { get; private set; }
        public int Percent
        {
            get { return percent; }
            set
            {
                percent = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Percent"));
            }
        }

        // Constructors
        public CircularProgressBarViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(CircularProgressBarView circularProgressBarView)
        {
            Percent = 0;

            CircularProgressBarView = circularProgressBarView;
        }
    }
}
