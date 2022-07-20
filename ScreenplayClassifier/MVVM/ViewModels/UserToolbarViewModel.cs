using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class UserToolbarViewModel
    {
        // Properties
        public UserModel User { get; set; }

        // Constructors
        public UserToolbarViewModel(UserModel user)
        {
            User = user;
        }

        // Methods
        #region Commands
        public Command ShowMainViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    foreach (Window window in App.Current.Windows)
                        if (window != App.Current.MainWindow)
                            window.Close();

                    App.Current.MainWindow.Focus();
                });
            }
        }
        public Command SignoutCommand
        {
            get
            {
                MainView mainView = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);

                return new Command(() =>
                {
                    App.Current.MainWindow = new SignInView();
                    App.Current.MainWindow.Show();

                    mainView.Close();
                });
            }
        }

        public Command ShowAboutViewCommand
        {
            get { return new Command(() => new AboutView().Show()); }
        }
        #endregion
    }
}
