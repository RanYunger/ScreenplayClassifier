using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class CircularProgressBarViewModel : INotifyPropertyChanged
    {
        // Fields
        private int percent;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public CircularProgressBarView CircularProgressBarView { get; set; }

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
            CircularProgressBarView = circularProgressBarView;

            Percent = 0;
        }

        public void ShowView(int classificationsRequired)
        {
            Percent = 0;
        }
    }
}
