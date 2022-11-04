using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<UserModel> authenticatedUsers;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public SettingsView SettingsView { get; private set; }

        public ObservableCollection<UserModel> AuthenticatedUsers
        {
            get { return authenticatedUsers; }
            set
            {
                authenticatedUsers = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AuthenticatedUsers"));
            }
        }

        // Constructors
        public SettingsViewModel() { }

        // Methods
        #region Commands
        public Command ApplyCommand
        {
            get
            {
                return new Command(() =>
                {
                });
            }
        }
        #endregion

        public void Init(SettingsView settingsView, MainViewModel mainViewModel, ObservableCollection<UserModel> authenticatedUsers)
        {
            AuthenticatedUsers = authenticatedUsers;

            MainViewModel = mainViewModel;
            SettingsView = settingsView;
        }
    }
}
