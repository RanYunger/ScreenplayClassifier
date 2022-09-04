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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource leftArrowImage, rightArrowImage;
        private readonly Duration animationDuration;
        private string moduleName, moduleTooltip;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public HomeView HomeView { get; private set; }

        public ImageSource LeftArrowImage
        {
            get { return leftArrowImage; }
            set
            {
                leftArrowImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LeftArrowImage"));
            }
        }

        public ImageSource RightArrowImage
        {
            get { return rightArrowImage; }
            set
            {
                rightArrowImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RightArrowImage"));
            }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;

                switch (ModuleName)
                {
                    case "Archives": ModuleTooltip = "View screenplays categorized by genre"; break;
                    case "Classification": ModuleTooltip = "Classify screenplays to genres"; break;
                    case "Reports": ModuleTooltip = "View classification reports"; break;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModuleName"));
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
        public HomeViewModel()
        {
            animationDuration = new Duration(TimeSpan.FromSeconds(0.3));

            ModuleName = "Classification";
        }

        // Methods
        #region Commands  
        public Command PressLeftArrowCommand
        {
            get
            {
                return new Command(() =>
                {
                    RotateLeft();

                    LeftArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LeftArrowPressed.png"));
                });
            }
        }

        public Command UnpressLeftArrowCommand
        {
            get { return new Command(() => LeftArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LeftArrowUnpressed.png"))); }
        }

        public Command PressRightArrowCommand
        {
            get
            {
                return new Command(() =>
                {
                    RotateRight();

                    RightArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "RightArrowPressed.png"));
                });
            }
        }

        public Command UnpressRightArrowCommand
        {
            get { return new Command(() => RightArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "RightArrowUnpressed.png"))); }
        }

        public Command ShowModuleViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    switch (ModuleName)
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

            LeftArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LeftArrowUnpressed.png"));
            RightArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "RightArrowUnpressed.png"));
        }

        private DoubleAnimation CreateDoubleAnimation(double from, double to, EventHandler completedEventHandler)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(from, to, animationDuration);

            if (completedEventHandler != null)
                doubleAnimation.Completed += completedEventHandler;

            return doubleAnimation;
        }

        private void RotateLeft()
        {
            // TODO: COMPLETE ANIMATIONS

            switch (ModuleName)
            {
                case "Archives": ModuleName = "Reports"; break;
                case "Classification": ModuleName = "Archives"; break;
                case "Reports": ModuleName = "Classification"; break;
            }
        }

        private void RotateRight()
        {
            // TODO: COMPLETE ANIMATIONS

            switch (ModuleName)
            {
                case "Archives": ModuleName = "Classification"; break;
                case "Classification": ModuleName = "Reports"; break;
                case "Reports": ModuleName = "Archives"; break;
            }
        }
    }
}
