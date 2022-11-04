using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class UserToolbarViewModel : INotifyPropertyChanged
    {
        // Fields
        private UserModel user;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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
        public UserToolbarViewModel() { }

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
                    App.Current.MainWindow = new SignInView();
                    App.Current.MainWindow.Show();

                    MainViewModel.MainView.Close();
                });
            }
        }

        public Command AboutCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.AboutView)); }
        }

        public Command SpeakNameCommand
        {
            get
            {
                return new Command(() =>
                {
                    MediaPlayer mediaPlayer = new MediaPlayer();

                    if (User.Role == UserModel.UserRole.GUEST)
                    {
                        mediaPlayer.Open(new Uri(FolderPaths.AUDIOS + "Jim.mp3"));
                        mediaPlayer.Play();
                    }
                });
            }
        }
        #endregion

        public void Init(UserModel user, MainViewModel mainViewModel)
        {
            User = user;
            MainViewModel = mainViewModel;
        }
    }
}
