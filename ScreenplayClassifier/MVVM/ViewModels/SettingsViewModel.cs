using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public SettingsView SettingsView { get; private set; }

        // Constructors
        public SettingsViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(SettingsView settingsView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            SettingsView = settingsView;
        }
    }
}
