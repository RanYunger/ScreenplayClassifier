using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        // Fields
        private bool userInstructed;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public HomeView HomeView { get; private set; }

        public bool UserInstructed
        {
            get { return userInstructed; }
            set
            {
                userInstructed = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserInstructed"));
            }
        }
        public Dictionary<string, int> GenresDictionary { get; private set; }

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
                    //MainViewModel.ShowView(MainViewModel.ArchivesView);

                    UserModel user = ((UserToolbarViewModel)MainViewModel.UserToolbarView.DataContext).User;

                    if (user.Role != UserModel.UserRole.GUEST)
                        MainViewModel.ShowView(MainViewModel.ArchivesView);
                    else
                    {
                        if (!UserInstructed)
                        {
                            UserInstructed = true;
                            MessageBoxHandler.Show("Before we start, an instructional video will be played", "Not so fast",
                                3, MessageBoxImage.Information);
                            PlayInstructionalVideoCommand.Execute(null);
                        }
                        else
                            MainViewModel.ShowView(MainViewModel.ArchivesView);
                    }
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

                    //UserModel user = ((UserToolbarViewModel)MainViewModel.UserToolbarView.DataContext).User;

                    //if (user.Role != UserModel.UserRole.GUEST)
                    //    MainViewModel.ShowView(MainViewModel.ClassificationView);
                    //else
                    //{
                    //    if (!UserInstructed)
                    //    {
                    //        UserInstructed = true;
                    //        MessageBoxHandler.Show("Before we start, an instructional video will be played", "Not so fast",
                    //            3, MessageBoxImage.Information);
                    //        PlayInstructionalVideoCommand.Execute(null);
                    //    }
                    //    else
                    //        MainViewModel.ShowView(MainViewModel.ClassificationView);
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
                    //MainViewModel.ShowView(MainViewModel.ReportsView);

                    UserModel user = ((UserToolbarViewModel)MainViewModel.UserToolbarView.DataContext).User;

                    if (user.Role != UserModel.UserRole.GUEST)
                        MainViewModel.ShowView(MainViewModel.ReportsView);
                    else
                    {
                        if (!UserInstructed)
                        {
                            UserInstructed = true;
                            MessageBoxHandler.Show("Before we start, an instructional video will be played", "Not so fast",
                                3, MessageBoxImage.Information);
                            PlayInstructionalVideoCommand.Execute(null);
                        }
                        else
                            MainViewModel.ShowView(MainViewModel.ReportsView);
                    }
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

                    mainView.WindowStyle = WindowStyle.None;

                    menuStackPanel.Visibility = Visibility.Collapsed;

                    choiceMediaElement.Visibility = Visibility.Visible;
                    choiceMediaElement.Source = new Uri(FolderPaths.VIDEOS + "Choice.mp4");
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

            App.Current.Dispatcher.Invoke(() => mainView.WindowStyle = WindowStyle.SingleBorderWindow);

            App.Current.Dispatcher.Invoke(() => stackPanel.Visibility = Visibility.Visible);
            App.Current.Dispatcher.Invoke(() => mediaElement.Visibility = Visibility.Collapsed);
        }
        #endregion

        public void Init(HomeView homeView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            HomeView = homeView;
        }
    }
}
