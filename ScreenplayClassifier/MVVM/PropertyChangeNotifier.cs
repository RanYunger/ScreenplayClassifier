using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ScreenplayClassifier.MVVM
{
    public abstract class PropertyChangeNotifier : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        protected virtual void NotifyPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}