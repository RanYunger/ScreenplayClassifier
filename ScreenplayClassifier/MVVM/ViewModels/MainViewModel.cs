using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;

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
