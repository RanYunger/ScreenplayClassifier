﻿using ScreenplayClassifier.MVVM.Views;
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
        public bool UserInstructed { get; private set; }

        // Constructors
        public HomeViewModel()
        {
        }

        // Methods
        #region Commands   
        public Command ArchivesCommand
        {
            get
            {
                MainView mainView = null;
                MainViewModel mainViewModel = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => mainViewModel = (MainViewModel)mainView.DataContext);

                return new Command(() =>
                {
                    if (mainViewModel.UserToolbarViewModel.User.Role != Models.UserModel.UserRole.GUEST)
                        mainViewModel.ShowView(mainViewModel.ArchivesView);
                    else
                    {
                        if (!UserInstructed)
                        {
                            UserInstructed = true;
                            MessageBoxHandler.Show("Before we start, an instructional video will be played", "Disclaimer",
                                3, MessageBoxImage.Information);
                            PlayInstructionalVideoCommand.Execute(null);
                        }
                        else
                            mainViewModel.ShowView(mainViewModel.ArchivesView);
                    }
                });
            }
        }
        public Command ClassificationCommand
        {
            get
            {
                MainView mainView = null;
                MainViewModel mainViewModel = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => mainViewModel = (MainViewModel)mainView.DataContext);

                return new Command(() =>
                {
                    if (mainViewModel.UserToolbarViewModel.User.Role != Models.UserModel.UserRole.GUEST)
                        mainViewModel.ShowView(mainViewModel.ClassificationView);
                    else
                    {
                        if (!UserInstructed)
                        {
                            UserInstructed = true;
                            MessageBoxHandler.Show("Before we start, an instructional video will be played", "Disclaimer",
                                3, MessageBoxImage.Information);
                            PlayInstructionalVideoCommand.Execute(null);
                        }
                        else
                            mainViewModel.ShowView(mainViewModel.ClassificationView);
                    }
                });
            }
        }
        public Command ReportsCommand
        {
            get
            {
                MainView mainView = null;
                MainViewModel mainViewModel = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => mainViewModel = (MainViewModel)mainView.DataContext);

                return new Command(() =>
                {
                    if (mainViewModel.UserToolbarViewModel.User.Role != Models.UserModel.UserRole.GUEST)
                        mainViewModel.ShowView(mainViewModel.ReportsView);
                    else
                    {
                        if (!UserInstructed)
                        {
                            UserInstructed = true;
                            MessageBoxHandler.Show("Before we start, an instructional video will be played", "Disclaimer",
                                3, MessageBoxImage.Information);
                            PlayInstructionalVideoCommand.Execute(null);
                        }
                        else
                            mainViewModel.ShowView(mainViewModel.ReportsView);
                    }
                });
            }
        }

        public Command PlayInstructionalVideoCommand
        {
            get
            {
                MainView mainView = null;
                HomeView homeView = null;
                StackPanel menuStackPanel = null;
                MediaElement choiceMediaElement = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => homeView = ((MainViewModel)mainView.DataContext).HomeView);
                App.Current.Dispatcher.Invoke(() => menuStackPanel = (StackPanel)homeView.FindName("MenuStackPanel"));
                App.Current.Dispatcher.Invoke(() => choiceMediaElement = (MediaElement)mainView.FindName("ChoiceMediaElement"));

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
    }
}
