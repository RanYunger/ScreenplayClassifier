using ScreenplayClassifier.MVVM.Models;
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
        private Thickness iconStartMargin, iconEndMargin;
        private Duration animationDuration;
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
            animationDuration = new Duration(TimeSpan.FromSeconds(0.3));
            iconStartMargin = new Thickness(10, 0, 10, 0);
            iconEndMargin = new Thickness(10, 150, 10, 0);

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
                    App.Current.MainWindow = new SignInView();
                    App.Current.MainWindow.Show();

                    MainViewModel.MainView.Close();
                });
            }
        }

        public Command DropIconCommand
        {
            get
            {
                return new Command(() =>
                {
                    ThicknessAnimation dropAnimation = new ThicknessAnimation(iconStartMargin, iconEndMargin, animationDuration);
                    Image iconImage = (Image)UserToolbarView.FindName("IconImage");

                    mediaPlayer.Play();

                    iconImage.BeginAnimation(Border.MarginProperty, dropAnimation);
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
        /// <param name="user">The user who authenticated to the system</param>
        /// <param name="userToolbarView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(UserModel user, UserToolbarView userToolbarView, MainViewModel mainViewModel)
        {
            User = user;
            UserToolbarView = userToolbarView;
            MainViewModel = mainViewModel;
        }
    }
}
