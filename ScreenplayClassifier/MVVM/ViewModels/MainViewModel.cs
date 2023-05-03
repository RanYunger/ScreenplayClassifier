using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class MainViewModel : PropertyChangeNotifier
    {
        // Fields

        // Properties
        public MainView MainView { get; private set; }
        public UserToolbarViewModel UserToolbarViewModel { get; private set; }

        public HomeView HomeView { get; private set; }
        public SettingsView SettingsView { get; private set; }
        public AboutView AboutView { get; private set; }
        public ReportsView ReportsView { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        // Constructors
        public MainViewModel()
        {
            HomeView = new HomeView();
            SettingsView = new SettingsView();
            AboutView = new AboutView();

            ReportsView = new ReportsView();
            ArchivesView = new ArchivesView();
            ClassificationView = new ClassificationView();
        }

        // Methods
        #region Commands
        public Command ShowHomeViewCommand
        {
            get { return new Command(() => ShowView(HomeView)); }
        }

        public Command CloseCommand
        {
            get
            {
                return new Command(() =>
                {
                    SettingsViewModel settingsViewModel = (SettingsViewModel)SettingsView.DataContext;
                    ArchivesViewModel archivesViewModel = (ArchivesViewModel)ArchivesView.DataContext;
                    ClassificationViewModel classificationViewModel = (ClassificationViewModel)ClassificationView.DataContext;
                    ReportsViewModel reportsViewModel = (ReportsViewModel)ReportsView.DataContext;
                    AboutViewModel aboutViewModel = (AboutViewModel)AboutView.DataContext;

                    // Stops all sounds
                    aboutViewModel.IsPlaying = false;
                    archivesViewModel.ArchivesSelectionViewModel.IsPlayingMedia = false;

                    // Kills the classification thread (if it's active)
                    if (classificationViewModel.ClassificationProgressViewModel.ClassificationThread != null)
                        classificationViewModel.ClassificationProgressViewModel.IsThreadAlive = false;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="user">The user who authenticated to the system</param>
        public void Init(UserModel user)
        {
            UserToolbarView userToolbarView;

            foreach (Window view in App.Current.Windows)
                if (view is MainView)
                {
                    MainView = (MainView)view;
                    break;
                }

            DATABASE.LoadGenres();

            userToolbarView = (UserToolbarView)MainView.FindName("UserToolbarView");
            UserToolbarViewModel = (UserToolbarViewModel)userToolbarView.DataContext;
            UserToolbarViewModel.Init(userToolbarView, this, user);

            HomeView = (HomeView)MainView.FindName("HomeView");
            SettingsView = (SettingsView)MainView.FindName("SettingsView");
            AboutView = (AboutView)MainView.FindName("AboutView");
            ReportsView = (ReportsView)MainView.FindName("ReportsView");
            ArchivesView = (ArchivesView)MainView.FindName("ArchivesView");
            ClassificationView = (ClassificationView)MainView.FindName("ClassificationView");

            ((HomeViewModel)HomeView.DataContext).Init(HomeView, this);
            ((SettingsViewModel)SettingsView.DataContext).Init(SettingsView, this);
            ((AboutViewModel)AboutView.DataContext).Init(AboutView, this);
            ((ArchivesViewModel)ArchivesView.DataContext).Init(ArchivesView, this);
            ((ClassificationViewModel)ClassificationView.DataContext).Init(ClassificationView, this);
            ((ReportsViewModel)ReportsView.DataContext).Init(ReportsView, this, UserToolbarViewModel.User);
        }

        /// <summary>
        /// Shows a child view.
        /// </summary>
        /// <param name="viewToShow">The child view to show within the view</param>
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

            ((AboutViewModel)AboutView.DataContext).IsPlaying = viewToShow == AboutView;

            if (viewToShow != ArchivesView)
                ((ArchivesViewModel)ArchivesView.DataContext).ArchivesFilterViewModel.ShowView();

            if (viewToShow == ReportsView)
                ((ReportsViewModel)ReportsView.DataContext).ReportsSelectionViewModel.RefreshView();
            else
                ((ReportsViewModel)ReportsView.DataContext).ReportsSelectionViewModel.ShowView();
        }
    }
}