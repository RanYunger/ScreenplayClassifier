﻿using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class UserToolbarViewModel : INotifyPropertyChanged
    {
        // Fields
        private System.Timers.Timer clapperTimer;
        private MediaPlayer mediaPlayer;
        private UserModel user;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public UserToolbarView UserToolbarView { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public UserModel User
        {
            get { return user; }
            set
            {
                user = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("User"));
            }
        }

        // Constructors
        public UserToolbarViewModel()
        {
            clapperTimer = new System.Timers.Timer(1100);
            clapperTimer.Elapsed += ClapperTimer_Elapsed;

            mediaPlayer = new MediaPlayer();
        }

        // Methods
        #region Commands
        public Command ActivateClapperCommand
        {
            get
            {
                Image iconImage = null;

                return new Command(() =>
                {
                    // Validation
                    if (UserToolbarView == null)
                        return;

                    iconImage = (Image)UserToolbarView.FindName("IconImage");

                    clapperTimer.Start();
                    mediaPlayer.Play();
                    mediaPlayer.Open(new Uri(string.Format("{0}Clapper.mp3", FolderPaths.AUDIOS)));

                    iconImage.Visibility = Visibility.Collapsed;
                });
            }
        }

        public Command ShowHomeCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.HomeView)); }
        }

        public Command ShowSettingsCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.SettingsView)); }
        }

        public Command ShowAboutCommand
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

        private void ClapperTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Image iconImage = null;

            App.Current.Dispatcher.Invoke(() => iconImage = (Image)UserToolbarView.FindName("IconImage"));
            App.Current.Dispatcher.Invoke(() => clapperTimer.Stop());
            App.Current.Dispatcher.Invoke(() => iconImage.Visibility = Visibility.Visible);
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

            mediaPlayer.Open(new Uri(string.Format("{0}Clapper.mp3", FolderPaths.AUDIOS)));
        }
    }
}