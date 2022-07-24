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
    public class MainViewModel
    {
        // Properties
        public Dictionary<string, int> GenresDictionary { get; private set; }
        public UserToolbarViewModel UserToolbarViewModel { get; private set; }
        public HomeView HomeView { get; private set; }
        public SettingsView SettingsView { get; private set; }
        public AboutView AboutView { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ClassificationView ClassificationView { get; private set; }
        public ReportsView ReportsView { get; private set; }

        // Constructors
        public MainViewModel()
        {
            GenresDictionary = ReadGenresDictionary();

            HomeView = new HomeView();
            SettingsView = new SettingsView();
            AboutView = new AboutView();

            ArchivesView = new ArchivesView();
            ClassificationView = new ClassificationView();
            ReportsView = new ReportsView();
        }

        // Methods
        #region Commands    
        #endregion

        public void Init(UserModel user)
        {
            MainView mainView = null;
            UserToolbarView userToolbarView = null;

            App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
            App.Current.Dispatcher.Invoke(() => userToolbarView = (UserToolbarView)mainView.FindName("UserToolbarView"));
            App.Current.Dispatcher.Invoke(() => userToolbarView.DataContext = UserToolbarViewModel = new UserToolbarViewModel(user, this));

            App.Current.Dispatcher.Invoke(() => HomeView = (HomeView)mainView.FindName("HomeView"));
            App.Current.Dispatcher.Invoke(() => SettingsView = (SettingsView)mainView.FindName("SettingsView"));
            App.Current.Dispatcher.Invoke(() => AboutView = (AboutView)mainView.FindName("AboutView"));
            App.Current.Dispatcher.Invoke(() => ArchivesView = (ArchivesView)mainView.FindName("ArchivesView"));
            App.Current.Dispatcher.Invoke(() => ClassificationView = (ClassificationView)mainView.FindName("ClassificationView"));
            App.Current.Dispatcher.Invoke(() => ReportsView = (ReportsView)mainView.FindName("ReportsView"));

            ((ArchivesViewModel)ArchivesView.DataContext).Init(GenresDictionary);
        }

        public Dictionary<string, int> ReadGenresDictionary()
        {
            Dictionary<string, int> genresDictionary = new Dictionary<string, int>();

            genresDictionary["Action"] = 0;
            genresDictionary["Adventure"] = 0;
            genresDictionary["Comedy"] = 2;
            genresDictionary["Crime"] = 0;

            genresDictionary["Drama"] = 0;
            genresDictionary["Family"] = 0;
            genresDictionary["Fantasy"] = 3;
            genresDictionary["Horror"] = 0;

            genresDictionary["Romance"] = 5;
            genresDictionary["SciFi"] = 1;
            genresDictionary["Thriller"] = 0;
            genresDictionary["War"] = 0;

            return genresDictionary;
        }

        public void ShowView(UserControl viewToShow)
        {
            UserControl[] views = { HomeView, SettingsView, AboutView, ArchivesView, ClassificationView, ReportsView };

            foreach (UserControl view in views)
                view.Visibility = view == viewToShow ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
