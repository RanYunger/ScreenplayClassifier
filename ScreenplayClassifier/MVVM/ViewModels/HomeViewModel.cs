using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class HomeViewModel
    {
        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public HomeView HomeView { get; private set; }

        public bool UserInstructed { get; private set; }

        // Constructors
        public HomeViewModel() { }

        // Methods
        #region Commands   
        public Command ShowArchivesViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    MainViewModel.ShowView(MainViewModel.ArchivesView);

                    //if (mainViewModel.UserToolbarViewModel.User.Role != Models.UserModel.UserRole.GUEST)
                    //    mainViewModel.ShowView(mainViewModel.ArchivesView);
                    //else
                    //{
                    //    if (!UserInstructed)
                    //    {
                    //        UserInstructed = true;
                    //        MessageBoxHandler.Show("Before we start, an instructional video will be played", "Disclaimer",
                    //            3, MessageBoxImage.Information);
                    //        PlayInstructionalVideoCommand.Execute(null);
                    //    }
                    //    else
                    //        mainViewModel.ShowView(mainViewModel.ArchivesView);
                    //}
                });
            }
        }
        public Command ShowClassificationViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    MainViewModel.ShowView(MainViewModel.ClassificationView);

                    //if (mainViewModel.UserToolbarViewModel.User.Role != Models.UserModel.UserRole.GUEST)
                    //    mainViewModel.ShowView(mainViewModel.ClassificationView);
                    //else
                    //{
                    //    if (!UserInstructed)
                    //    {
                    //        UserInstructed = true;
                    //        MessageBoxHandler.Show("Before we start, an instructional video will be played", "Disclaimer",
                    //            3, MessageBoxImage.Information);
                    //        PlayInstructionalVideoCommand.Execute(null);
                    //    }
                    //    else
                    //        mainViewModel.ShowView(mainViewModel.ClassificationView);
                    //}
                });
            }
        }
        public Command ShowReportsViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    MainViewModel.ShowView(MainViewModel.ReportsView);

                    //if (mainViewModel.UserToolbarViewModel.User.Role != Models.UserModel.UserRole.GUEST)
                    //    mainViewModel.ShowView(mainViewModel.ReportsView);
                    //else
                    //{
                    //    if (!UserInstructed)
                    //    {
                    //        UserInstructed = true;
                    //        MessageBoxHandler.Show("Before we start, an instructional video will be played", "Disclaimer",
                    //            3, MessageBoxImage.Information);
                    //        PlayInstructionalVideoCommand.Execute(null);
                    //    }
                    //    else
                    //        mainViewModel.ShowView(mainViewModel.ReportsView);
                    //}
                });
            }
        }

        public Command PlayInstructionalVideoCommand
        {
            get
            {
                MainView mainView = MainViewModel.MainView;
                StackPanel menuStackPanel = menuStackPanel = (StackPanel)HomeView.FindName("MenuStackPanel");
                MediaElement choiceMediaElement = (MediaElement)mainView.FindName("ChoiceMediaElement");

                return new Command(() =>
                {
                    System.Timers.Timer videoTimer = new System.Timers.Timer(102500);

                    Mouse.OverrideCursor = Cursors.None;

                    mainView.WindowState = WindowState.Maximized;
                    mainView.WindowStyle = WindowStyle.None;

                    menuStackPanel.Visibility = Visibility.Collapsed;

                    choiceMediaElement.Visibility = Visibility.Visible;
                    choiceMediaElement.Source = new Uri(Environment.CurrentDirectory + @"\Media\Videos\Choice.mp4");
                    choiceMediaElement.Play();

                    videoTimer.Elapsed += (sender, e) => VideoTimer_Elapsed(sender, e, mainView, menuStackPanel, choiceMediaElement);
                    videoTimer.Start();
                });
            }
        }

        private void VideoTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, MainView mainView,
            StackPanel stackPanel, MediaElement mediaElement)
        {
            App.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);

            App.Current.Dispatcher.Invoke(() => mainView.WindowState = WindowState.Normal);
            App.Current.Dispatcher.Invoke(() => mainView.WindowStyle = WindowStyle.SingleBorderWindow);

            App.Current.Dispatcher.Invoke(() => stackPanel.Visibility = Visibility.Visible);
            App.Current.Dispatcher.Invoke(() => mediaElement.Visibility = Visibility.Collapsed);
        }
        #endregion

        public void Init(HomeView homeView, MainViewModel mainViewModel)
        {
            HomeView = homeView;
            MainViewModel = mainViewModel;
        }
    }
}
