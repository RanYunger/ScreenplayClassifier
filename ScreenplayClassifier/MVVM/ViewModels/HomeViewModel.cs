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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

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

        public void Init(HomeView homeView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            HomeView = homeView;
        }
    }
}
