using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.ComponentModel;
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

        private void SlideLeftAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            // TODO: COMPLETE (SHIFT TO NEW LOCATIONS: LEFT CENTER RIGHT --> RIGHT LEFT CENTER)

            DoubleAnimation enlargeAnimation = new DoubleAnimation(300, 400, animationDuration),
                reduceAnimation = new DoubleAnimation(400, 300, animationDuration);

            leftImage.BeginAnimation(Image.HeightProperty, enlargeAnimation, HandoffBehavior.Compose);
            leftImage.BeginAnimation(Image.WidthProperty, enlargeAnimation, HandoffBehavior.Compose);

            centerImage.BeginAnimation(Image.HeightProperty, reduceAnimation, HandoffBehavior.Compose);
            centerImage.BeginAnimation(Image.WidthProperty, reduceAnimation, HandoffBehavior.Compose);
        }

        private void SlideRightAnimation(Image leftImage, Image centerImage, Image rightImage)
        {
            // TODO: COMPLETE (SHIFT TO NEW LOCATIONS: LEFT CENTER RIGHT --> CENTER RIGHT LEFT)

            DoubleAnimation enlargeAnimation = new DoubleAnimation(300, 400, animationDuration),
                reduceAnimation = new DoubleAnimation(400, 300, animationDuration);

            rightImage.BeginAnimation(Image.HeightProperty, enlargeAnimation, HandoffBehavior.Compose);
            rightImage.BeginAnimation(Image.WidthProperty, enlargeAnimation, HandoffBehavior.Compose);

            centerImage.BeginAnimation(Image.HeightProperty, reduceAnimation, HandoffBehavior.Compose);
            centerImage.BeginAnimation(Image.WidthProperty, reduceAnimation, HandoffBehavior.Compose);
        }

        private void RotateLeft()
        {
            Image leftImage = null, centerImage = null, rightImage = null;

            switch (ModuleName)
            {
                case "Archives":
                    {
                        leftImage = (Image)HomeView.FindName("ReportsImage");
                        centerImage = (Image)HomeView.FindName("ArchivesImage");
                        rightImage = (Image)HomeView.FindName("ClassificationImage");

                        ModuleName = "Reports";
                    }
                    break;
                case "Classification":
                    {
                        leftImage = (Image)HomeView.FindName("ArchivesImage");
                        centerImage = (Image)HomeView.FindName("ClassificationImage");
                        rightImage = (Image)HomeView.FindName("ReportsImage");

                        ModuleName = "Archives";
                    }
                    break;
                case "Reports":
                    {
                        leftImage = (Image)HomeView.FindName("ClassificationImage");
                        centerImage = (Image)HomeView.FindName("ReportsImage");
                        rightImage = (Image)HomeView.FindName("ArchivesImage");

                        ModuleName = "Classification";
                    }
                    break;
            }

            SlideLeftAnimation(leftImage, centerImage, rightImage);
        }

        private void RotateRight()
        {
            Image leftImage = null, centerImage = null, rightImage = null;

            switch (ModuleName)
            {
                case "Archives":
                    {
                        leftImage = (Image)HomeView.FindName("ReportsImage");
                        centerImage = (Image)HomeView.FindName("ArchivesImage");
                        rightImage = (Image)HomeView.FindName("ClassificationImage");

                        ModuleName = "Classification";
                    }
                    break;
                case "Classification":
                    {
                        leftImage = (Image)HomeView.FindName("ArchivesImage");
                        centerImage = (Image)HomeView.FindName("ClassificationImage");
                        rightImage = (Image)HomeView.FindName("ReportsImage");

                        ModuleName = "Reports";
                    }
                    break;
                case "Reports":
                    {
                        leftImage = (Image)HomeView.FindName("ClassificationImage");
                        centerImage = (Image)HomeView.FindName("ReportsImage");
                        rightImage = (Image)HomeView.FindName("ArchivesImage");

                        ModuleName = "Archives";
                    }
                    break;
            }

            SlideRightAnimation(leftImage, centerImage, rightImage);
        }
    }
}
