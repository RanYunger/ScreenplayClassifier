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
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(string.Format("{0}Wilhelm Scream.mp3", FolderPaths.AUDIOS)));
        }

        // Methods
        #region Commands
        public Command HomeCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.HomeView)); }
        }

        public Command SettingsCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.SettingsView)); }
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

        public Command OptionsCommand
        {
            get
            {
                return new Command(() =>
                {
                    // TODO: COMPLETE
                });
            }
        }

        public Command AboutCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.AboutView)); }
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
