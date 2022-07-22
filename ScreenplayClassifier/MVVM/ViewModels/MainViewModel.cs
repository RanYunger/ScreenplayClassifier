using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class MainViewModel
    {
        // Properties
        public UserToolbarViewModel UserToolbarViewModel { get; private set; }

        // Constructors
        public MainViewModel() { }

        // Methods
        #region Commands
        public Command PlayInstructionalVideoCommand
        {
            get
            {
                MainView mainView = null;
                MediaElement choiceMediaElement = null;
                StackPanel normalDisplayStackPanel = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => choiceMediaElement = (MediaElement)mainView.FindName("ChoiceMediaElement"));
                App.Current.Dispatcher.Invoke(() => normalDisplayStackPanel = (StackPanel)mainView.FindName("NormalDisplayStackPanel"));

                return new Command(() =>
                {
                    System.Timers.Timer videoTimer = new System.Timers.Timer(102500);

                    normalDisplayStackPanel.Visibility = Visibility.Collapsed;

                    choiceMediaElement.Visibility = Visibility.Visible;
                    choiceMediaElement.Source = new Uri(Environment.CurrentDirectory + @"\Media\Videos\Choice.mp4");
                    choiceMediaElement.Play();

                    videoTimer.Elapsed += (sender, e) => VideoTimer_Elapsed(sender, e, normalDisplayStackPanel, choiceMediaElement);
                    videoTimer.Start();
                });
            }
        }

        private void VideoTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, StackPanel stackPanel, MediaElement mediaElement)
        {
            App.Current.Dispatcher.Invoke(() => stackPanel.Visibility = Visibility.Visible);
            App.Current.Dispatcher.Invoke(() => mediaElement.Visibility = Visibility.Collapsed);
        }

        #endregion

        public void Init(UserModel user)
        {
            MainView mainView = null;
            UserToolbarView userToolbarView = null;

            App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
            App.Current.Dispatcher.Invoke(() => userToolbarView = (UserToolbarView)mainView.FindName("UserToolbarView"));
            App.Current.Dispatcher.Invoke(() => userToolbarView.DataContext = UserToolbarViewModel = new UserToolbarViewModel(user));
        }
    }
}
