using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
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
        public Command CheckKeyCommand
        {
            get
            {
                MainView mainView = null;
                MenuView menuView = null;
                MenuViewModel menuViewModel = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => menuView = (MenuView)mainView.FindName("MenuView"));
                App.Current.Dispatcher.Invoke(() => menuViewModel = (MenuViewModel)menuView.DataContext);

                return new Command(() =>
                {
                    if (Keyboard.IsKeyDown(Key.Enter))
                        OpenModuleCommand.Execute(menuViewModel.CurrentModule);
                    else if (Keyboard.IsKeyDown(Key.Left))
                        menuViewModel.RotateLeftCommand.Execute(null);
                    else if (Keyboard.IsKeyDown(Key.Right))
                        menuViewModel.RotateRightCommand.Execute(null);
                });
            }
        }
        public Command OpenModuleCommand
        {
            get
            {
                return new Command(() =>
                {
                    // TODO: COMPLETE (open view by text)
                    new AboutView().Show();
                });
            }
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
