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

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Fields
        private MediaPlayer mediaPlayer;
        private Dictionary<string, ObservableCollection<ScreenplayModel>> genresDictionary;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainView MainView { get; private set; }
        public UserToolbarView UserToolbarView { get; private set; }

        public HomeView HomeView { get; private set; }
        public SettingsView SettingsView { get; private set; }
        public AboutView AboutView { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ClassificationView ClassificationView { get; private set; }
        public ReportsView ReportsView { get; private set; }

        public Dictionary<string, ObservableCollection<ScreenplayModel>> GenresDictionary
        {
            get { return genresDictionary; }
            set
            {
                genresDictionary = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenresDictionary"));
            }
        }

        // Constructors
        public MainViewModel()
        {
            mediaPlayer = new MediaPlayer();

            GenresDictionary = Storage.LoadArchives();

            HomeView = new HomeView();
            SettingsView = new SettingsView();
            AboutView = new AboutView();

            ArchivesView = new ArchivesView();
            ClassificationView = new ClassificationView();
            ReportsView = new ReportsView();
        }

        // Methods
        #region Commands
        public Command CloseCommand
        {
            get { return new Command(() => Storage.SaveArchives(GenresDictionary)); }
        }
        #endregion

        public void Init(UserModel user)
        {
            foreach (Window view in App.Current.Windows)
                if (view is MainView)
                {
                    MainView = (MainView)view;
                    break;
                }

            UserToolbarView = (UserToolbarView)MainView.FindName("UserToolbarView");
            ((UserToolbarViewModel)UserToolbarView.DataContext).Init(user, this);

            HomeView = (HomeView)MainView.FindName("HomeView");
            SettingsView = (SettingsView)MainView.FindName("SettingsView");
            AboutView = (AboutView)MainView.FindName("AboutView");
            ArchivesView = (ArchivesView)MainView.FindName("ArchivesView");
            ClassificationView = (ClassificationView)MainView.FindName("ClassificationView");
            ReportsView = (ReportsView)MainView.FindName("ReportsView");

            ((HomeViewModel)HomeView.DataContext).Init(HomeView, this);
            ((SettingsViewModel)SettingsView.DataContext).Init(SettingsView, this);
            ((AboutViewModel)AboutView.DataContext).Init(AboutView, this);
            ((ArchivesViewModel)ArchivesView.DataContext).Init(ArchivesView, this, GenresDictionary);
            ((ClassificationViewModel)ClassificationView.DataContext).Init(ClassificationView, this);
            ((ReportsViewModel)ReportsView.DataContext).Init(ReportsView, this);
        }

        public void ShowView(UserControl viewToShow)
        {
            UserControl[] views = { HomeView, SettingsView, AboutView, ArchivesView, ClassificationView, ReportsView };

            if (viewToShow.Visibility == Visibility.Visible)
            {
                viewToShow.Focus();
                return;
            }

            foreach (UserControl view in views)
                view.Visibility = view == viewToShow ? Visibility.Visible : Visibility.Collapsed;

            if (ClassificationView.Visibility == Visibility.Collapsed)
                ((ClassificationViewModel)ClassificationView.DataContext).StopVideoCommand.Execute(null);

            if (viewToShow == AboutView)
                ((AboutViewModel)AboutView.DataContext).PlayVideoCommand.Execute(null);
        }
    }
}
