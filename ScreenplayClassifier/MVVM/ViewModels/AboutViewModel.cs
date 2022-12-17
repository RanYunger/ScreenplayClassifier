using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows.Controls;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public AboutView AboutView { get; private set; }

        // Constructors
        public AboutViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(AboutView aboutView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            AboutView = aboutView;
        }
    }
}
