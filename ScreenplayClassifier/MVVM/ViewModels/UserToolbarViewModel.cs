using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class UserToolbarViewModel : PropertyChangeNotifier
    {
        // Fields
        private MediaPlayer mediaPlayer;
        private UserModel user;

        // Properties
        public UserToolbarView UserToolbarView { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public UserModel User
        {
            get { return user; }
            set
            {
                user = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public UserToolbarViewModel()
        {
            mediaPlayer = new MediaPlayer();
        }

        // Methods
        #region Commands
        public Command ShowHomeViewCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.HomeView)); }
        }

        public Command ShowSettingsViewCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.SettingsView)); }
        }

        public Command ShowAboutViewCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.AboutView)); }
        }

        public Command SignoutCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.Current.MainWindow = new EntryView();
                    App.Current.MainWindow.Show();

                    MainViewModel.MainView.Close();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="userToolbarView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        /// <param name="user">The user who authenticated to the system</param>
        public void Init(UserToolbarView userToolbarView, MainViewModel mainViewModel, UserModel user)
        {
            UserToolbarView = userToolbarView;
            MainViewModel = mainViewModel;
            User = user;
        }
    }
}