using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class UserToolbarViewModel
    {
        // Properties
        public UserModel User { get; set; }
        public MainViewModel MainViewModel { get; set; }

        // Constructors
        public UserToolbarViewModel(UserModel user, MainViewModel mainViewModel)
        {
            User = user;
            MainViewModel = mainViewModel;
        }

        // Methods
        #region Commands
        public Command HomeCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.HomeView)); }
        }
        public Command SignoutCommand
        {
            get
            {
                MainView mainView = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);

                return new Command(() =>
                {
                    App.Current.MainWindow = new SignInView();
                    App.Current.MainWindow.Show();

                    mainView.Close();
                });
            }
        }
        public Command SettingsCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.SettingsView)); }
        }

        public Command AboutCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.AboutView)); }
        }
        #endregion
    }
}
