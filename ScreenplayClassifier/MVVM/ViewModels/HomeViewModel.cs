using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class HomeViewModel : PropertyChangeNotifier
    {
        // Fields

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public HomeView HomeView { get; private set; }

        // Constructors
        public HomeViewModel() { }

        // Methods
        #region Commands  
        public Command ShowArchivesViewCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.ArchivesView)); }
        }

        public Command ShowClassificationViewCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.ClassificationView)); }
        }

        public Command ShowReportsViewCommand
        {
            get { return new Command(() => MainViewModel.ShowView(MainViewModel.ReportsView)); }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="homeView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(HomeView homeView, MainViewModel mainViewModel)
        {
            HomeView = homeView;
            MainViewModel = mainViewModel;
        }
    }
}