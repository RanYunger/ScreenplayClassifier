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
        private ImageSource leftImage, centerImage, rightImage;
        private string leftModule, centerModule, rightModule, moduleTooltip;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public HomeView HomeView { get; private set; }

        public ImageSource LeftImage
        {
            get { return leftImage; }
            set
            {
                leftImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LeftImage"));
            }
        }

        public ImageSource CenterImage
        {
            get { return centerImage; }
            set
            {
                centerImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CenterImage"));
            }
        }

        public ImageSource RightImage
        {
            get { return rightImage; }
            set
            {
                rightImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RightImage"));
            }
        }

        public string LeftModule
        {
            get { return leftModule; }
            set
            {
                leftModule = value;

                LeftImage = new BitmapImage(new Uri(FolderPaths.IMAGES + leftModule + "Disabled.png"));

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LeftModule"));
            }
        }

        public string CenterModule
        {
            get { return centerModule; }
            set
            {
                centerModule = value;

                switch (centerModule)
                {
                    case "Archives": ModuleTooltip = "View classifications categorized by genre"; break;
                    case "Classification": ModuleTooltip = "Classify screenplays to genres"; break;
                    case "Reports": ModuleTooltip = "View classification reports"; break;
                }

                CenterImage = new BitmapImage(new Uri(FolderPaths.IMAGES + centerModule + "Enabled.png"));

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CenterModule"));
            }
        }

        public string RightModule
        {
            get { return rightModule; }
            set
            {
                rightModule = value;

                RightImage = new BitmapImage(new Uri(FolderPaths.IMAGES + rightModule + "Disabled.png"));

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RightModule"));
            }
        }

        public string ModuleTooltip
        {
            get { return moduleTooltip; }
            set
            {
                moduleTooltip = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModuleTooltip"));
            }
        }

        // Constructors
        public HomeViewModel() { }

        // Methods
        #region Commands   
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    string temp = string.Empty;

                    if (Keyboard.IsKeyDown(Key.Left))
                    {
                        temp = LeftModule;
                        LeftModule = CenterModule;
                        CenterModule = RightModule;
                        RightModule = temp;
                    }
                    else if (Keyboard.IsKeyDown(Key.Right))
                    {
                        temp = RightModule;
                        RightModule = CenterModule;
                        CenterModule = LeftModule;
                        LeftModule = temp;
                    }
                });
            }
        }

        public Command ShowModuleViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    switch (CenterModule)
                    {
                        case "Archives": MainViewModel.ShowView(MainViewModel.ArchivesView); break;
                        case "Classification": MainViewModel.ShowView(MainViewModel.ClassificationView); break;
                        case "Reports": MainViewModel.ShowView(MainViewModel.ReportsView); break;
                    }
                });
            }
        }
        #endregion

        public void Init(HomeView homeView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            HomeView = homeView;

            LeftModule = "Archives";
            CenterModule = "Classification";
            RightModule = "Reports";
        }
    }
}
