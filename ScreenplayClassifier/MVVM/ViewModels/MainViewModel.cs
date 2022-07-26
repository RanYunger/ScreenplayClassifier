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
        public MainView MainView { get; private set; }
        public UserToolbarView UserToolbarView { get; private set; }

        public HomeView HomeView { get; private set; }
        public SettingsView SettingsView { get; private set; }
        public AboutView AboutView { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ClassificationView ClassificationView { get; private set; }
        public ReportsView ReportsView { get; private set; }

        public Dictionary<string, int> GenresDictionary { get; private set; }

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
            App.Current.Dispatcher.Invoke(() => MainView = (MainView)App.Current.MainWindow);
            UserToolbarView = (UserToolbarView)MainView.FindName("UserToolbarView");
            UserToolbarView.DataContext = new UserToolbarViewModel(user, this);

            HomeView = (HomeView)MainView.FindName("HomeView");
            SettingsView = (SettingsView)MainView.FindName("SettingsView");
            AboutView = (AboutView)MainView.FindName("AboutView");
            ArchivesView = (ArchivesView)MainView.FindName("ArchivesView");
            ClassificationView = (ClassificationView)MainView.FindName("ClassificationView");
            ReportsView = (ReportsView)MainView.FindName("ReportsView");

            ((HomeViewModel)HomeView.DataContext).Init(HomeView, this);
            ((SettingsViewModel)SettingsView.DataContext).Init(SettingsView, this);
            ((ArchivesViewModel)ArchivesView.DataContext).Init(ArchivesView, this, GenresDictionary);
            ((ClassificationViewModel)ClassificationView.DataContext).Init(ClassificationView, this);
            ((ReportsViewModel)ReportsView.DataContext).Init(ReportsView, this);
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
