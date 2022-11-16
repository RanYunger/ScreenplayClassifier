using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource leftArrowImage, rightArrowImage;
        private Thickness leftMargin, centerMargin, rightMargin;
        private Duration animationDuration;
        private string moduleName, moduleDescription;
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

                switch (moduleName)
                {
                    case "Archives": ModuleDescription = "View categorized screenplays"; break;
                    case "Classification": ModuleDescription = "Classify screenplays to genres"; break;
                    case "Reports": ModuleDescription = "View classification reports"; break;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModuleName"));
            }
        }

        public string ModuleDescription
        {
            get { return moduleDescription; }
            set
            {
                moduleDescription = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModuleDescription"));
            }
        }

        // Constructors
        public HomeViewModel()
        {
            animationDuration = new Duration(TimeSpan.FromSeconds(0.3));
            leftMargin = new Thickness(50, 50, 50, 50);
            centerMargin = new Thickness(400, 0, 0, 0);
            rightMargin = new Thickness(850, 50, 50, 50);

            ModuleName = "Classification";
        }

        // Methods
        #region Commands  
        public Command ShowArchivesViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (ModuleName == "Archives")
                        MainViewModel.ShowView(MainViewModel.ArchivesView);
                });
            }
        }

        public Command ShowClassificationViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (ModuleName == "Classification")
                        MainViewModel.ShowView(MainViewModel.ClassificationView);
                });
            }
        }

        public Command ShowReportsViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (ModuleName == "Reports")
                        MainViewModel.ShowView(MainViewModel.ReportsView);
                });
            }
        }

        public Command PressLeftCommand
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

        public Command UnpressLeftCommand
        {
            get { return new Command(() => LeftArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LeftArrowUnpressed.png"))); }
        }

        public Command PressRightCommand
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

        public Command UnpressRightCommand
        {
            get { return new Command(() => RightArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "RightArrowUnpressed.png"))); }
        }
        #endregion

        public void Init(HomeView homeView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            HomeView = homeView;

            LeftArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "LeftArrowUnpressed.png"));
            RightArrowImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "RightArrowUnpressed.png"));
        }

        private void LeftAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            // Updates Z indices of all images for the illusion of circularity
            Canvas.SetZIndex(rightImage, 1);
            Canvas.SetZIndex(centerImage, 2);
            Canvas.SetZIndex(leftImage, 3);

            LeftSlideAnimation(leftImage, centerImage, rightImage);
            LeftResizeAnimation(leftImage, centerImage, rightImage);
        }

        private void LeftSlideAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            ThicknessAnimation leftToCenterAnimation = new ThicknessAnimation(leftMargin, centerMargin, animationDuration),
                centerToRightAnimation = new ThicknessAnimation(centerMargin, rightMargin, animationDuration),
                rightToLeftAnimation = new ThicknessAnimation(rightMargin, leftMargin, animationDuration);

            leftImage.BeginAnimation(Border.MarginProperty, leftToCenterAnimation);
            centerImage.BeginAnimation(Border.MarginProperty, centerToRightAnimation);
            rightImage.BeginAnimation(Border.MarginProperty, rightToLeftAnimation);
        }

        private void LeftResizeAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            DoubleAnimation enlargeAnimation = new DoubleAnimation(300, 400, animationDuration),
                reduceAnimation = new DoubleAnimation(400, 300, animationDuration);

            leftImage.BeginAnimation(Image.HeightProperty, enlargeAnimation, HandoffBehavior.Compose);
            leftImage.BeginAnimation(Image.WidthProperty, enlargeAnimation, HandoffBehavior.Compose);

            centerImage.BeginAnimation(Image.HeightProperty, reduceAnimation, HandoffBehavior.Compose);
            centerImage.BeginAnimation(Image.WidthProperty, reduceAnimation, HandoffBehavior.Compose);
        }

        private void RightAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            // Updates Z indices of all images for the illusion of circularity
            Canvas.SetZIndex(leftImage, 1);
            Canvas.SetZIndex(centerImage, 2);
            Canvas.SetZIndex(rightImage, 3);

            RightSlideAnimation(leftImage, centerImage, rightImage);
            RightResizeAnimation(leftImage, centerImage, rightImage);
        }

        private void RightSlideAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            ThicknessAnimation leftToRightAnimation = new ThicknessAnimation(leftMargin, rightMargin, animationDuration),
                centerToLeftAnimation = new ThicknessAnimation(centerMargin, leftMargin, animationDuration),
                rightToCenterAnimation = new ThicknessAnimation(rightMargin, centerMargin, animationDuration);

            leftImage.BeginAnimation(Border.MarginProperty, leftToRightAnimation);
            centerImage.BeginAnimation(Border.MarginProperty, centerToLeftAnimation);
            rightImage.BeginAnimation(Border.MarginProperty, rightToCenterAnimation);
        }

        private void RightResizeAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            DoubleAnimation enlargeAnimation = new DoubleAnimation(300, 400, animationDuration),
                reduceAnimation = new DoubleAnimation(400, 300, animationDuration);

            rightImage.BeginAnimation(Image.HeightProperty, enlargeAnimation, HandoffBehavior.Compose);
            rightImage.BeginAnimation(Image.WidthProperty, enlargeAnimation, HandoffBehavior.Compose);

            centerImage.BeginAnimation(Image.HeightProperty, reduceAnimation, HandoffBehavior.Compose);
            centerImage.BeginAnimation(Image.WidthProperty, reduceAnimation, HandoffBehavior.Compose);
        }

        private void RotateLeft()
        {
            Image leftGif = null, centerGif = null, rightGif = null;

            switch (ModuleName)
            {
                case "Archives":
                    {
                        leftGif = (Image)HomeView.FindName("ReportsGif");
                        centerGif = (Image)HomeView.FindName("ArchivesGif");
                        rightGif = (Image)HomeView.FindName("ClassificationGif");

                        LeftAnimation(leftGif, centerGif, rightGif);

                        ModuleName = "Reports";
                    }
                    break;
                case "Classification":
                    {
                        leftGif = (Image)HomeView.FindName("ArchivesGif");
                        centerGif = (Image)HomeView.FindName("ClassificationGif");
                        rightGif = (Image)HomeView.FindName("ReportsGif");

                        LeftAnimation(leftGif, centerGif, rightGif);

                        ModuleName = "Archives";
                    }
                    break;
                case "Reports":
                    {
                        leftGif = (Image)HomeView.FindName("ClassificationGif");
                        centerGif = (Image)HomeView.FindName("ReportsGif");
                        rightGif = (Image)HomeView.FindName("ArchivesGif");

                        LeftAnimation(leftGif, centerGif, rightGif);

                        ModuleName = "Classification";
                    }
                    break;
            }
        }

        private void RotateRight()
        {
            Image leftGif = null, centerGif = null, rightGif = null;

            switch (ModuleName)
            {
                case "Archives":
                    {
                        leftGif = (Image)HomeView.FindName("ReportsGif");
                        centerGif = (Image)HomeView.FindName("ArchivesGif");
                        rightGif = (Image)HomeView.FindName("ClassificationGif");

                        RightAnimation(leftGif, centerGif, rightGif);

                        ModuleName = "Classification";
                    }
                    break;
                case "Classification":
                    {
                        leftGif = (Image)HomeView.FindName("ArchivesGif");
                        centerGif = (Image)HomeView.FindName("ClassificationGif");
                        rightGif = (Image)HomeView.FindName("ReportsGif");

                        RightAnimation(leftGif, centerGif, rightGif);

                        ModuleName = "Reports";
                    }
                    break;
                case "Reports":
                    {
                        leftGif = (Image)HomeView.FindName("ClassificationGif");
                        centerGif = (Image)HomeView.FindName("ReportsGif");
                        rightGif = (Image)HomeView.FindName("ArchivesGif");

                        RightAnimation(leftGif, centerGif, rightGif);

                        ModuleName = "Archives";
                    }
                    break;
            }
        }
    }
}
