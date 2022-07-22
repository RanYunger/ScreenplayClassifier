using ScreenplayClassifier.MVVM.Views;
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
    public class MenuViewModel
    {
        // Properties

        // Constructors
        public MenuViewModel() { }

        // Methods
        #region Commands   
        public Command OpenArchivesViewCommand
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
                    {
                        new ArchivesView().Show();
                        mainView.Close();
                    }
                    else if (QuestionUser(mainViewModel))
                    {
                        new ArchivesView().Show();
                        mainView.Close();
                    }
                });
            }
        }
        public Command OpenClassificationViewCommand
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
                    {
                        new ClassificationView().Show();
                        mainView.Close();
                    }
                    else if (QuestionUser(mainViewModel))
                    {
                        new ClassificationView().Show();
                        mainView.Close();
                    }
                });
            }
        }
        public Command OpenReportsViewCommand
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
                    {
                        new ReportsView().Show();
                        mainView.Close();
                    }
                    else if (QuestionUser(mainViewModel))
                    {
                        new ReportsView().Show();
                        mainView.Close();
                    }
                });
            }
        }
        #endregion

        private bool QuestionUser(MainViewModel mainViewModel)
        {
            MessageBoxResult userBriefed, userUnderstood;

            userBriefed = MessageBox.Show("Have you undergone a safety briefing?", "Before we start",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (userBriefed == MessageBoxResult.No)
            {
                mainViewModel.PlayInstructionalVideoCommand.Execute(null);
                return false;
            }

            userUnderstood = MessageBox.Show("Have you internalized the covered material?", "Almost there",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            return userUnderstood == MessageBoxResult.Yes;
        }
    }
}
