using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class MenuViewModel
    {
        // Fields
        private int currentOffset;

        // Properties
        public ObservableCollection<string> ModulesList { get; private set; }
        public string CurrentModule { get; private set; }

        // Constructors
        public MenuViewModel()
        {
            currentOffset = 1;

            ModulesList = new ObservableCollection<string>(new string[] { "History", "Classification", "Reports" });
        }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Keyboard.IsKeyDown(Key.Left))
                        RotateLeftCommand.Execute(null);
                    else if (Keyboard.IsKeyDown(Key.Right))
                        RotateRightCommand.Execute(null);
                });
            }
        }
        public Command RotateLeftCommand
        {
            get
            {
                MainView mainView = null;
                MenuView menuView = null;
                TextBlock currentModuleTextBlock = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => menuView = (MenuView)mainView.FindName("MenuView"));
                App.Current.Dispatcher.Invoke(() => currentModuleTextBlock = (TextBlock)menuView.FindName("CurrentModuleTextBlock"));

                return new Command(() =>
                {
                    // TODO: COMPLETE ANIMATIONS!
                    currentOffset = currentOffset == 0 ? ModulesList.Count - 1 : --currentOffset;
                    currentModuleTextBlock.Text = CurrentModule = ModulesList[currentOffset % ModulesList.Count];
                });
            }
        }
        public Command RotateRightCommand
        {
            get
            {
                MainView mainView = null;
                MenuView menuView = null;
                TextBlock currentModuleTextBlock = null;

                App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => menuView = (MenuView)mainView.FindName("MenuView"));
                App.Current.Dispatcher.Invoke(() => currentModuleTextBlock = (TextBlock)menuView.FindName("CurrentModuleTextBlock"));

                return new Command(() =>
                {
                    // TODO: COMPLETE ANIMATIONS!
                    currentOffset = currentOffset == ModulesList.Count - 1 ? 0 : ++currentOffset;
                    currentModuleTextBlock.Text = CurrentModule = ModulesList[currentOffset % ModulesList.Count];
                });
            }
        }
        #endregion
    }
}
