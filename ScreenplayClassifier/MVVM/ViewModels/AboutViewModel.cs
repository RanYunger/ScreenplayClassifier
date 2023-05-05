using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class AboutViewModel : PropertyChangeNotifier
    {
        // Fields
        private bool isPlayingVideo;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public AboutView AboutView { get; private set; }

        public bool IsPlayingVideo
        {
            get { return isPlayingVideo; }
            set
            {
                MediaElement mediaElement = null;

                // Validation
                if (AboutView == null)
                    return;

                mediaElement = (MediaElement)AboutView.FindName("MediaElement");

                isPlayingVideo = value;

                if (isPlayingVideo)
                    mediaElement.Play();
                else
                    mediaElement.Pause();

                NotifyPropertyChange();
            }
        }

        // Constructors
        public AboutViewModel() { }

        // Methods
        #region Commands
        public Command ToggleVideoStateCommand
        {
            get { return new Command(() => IsPlayingVideo = !IsPlayingVideo); }
        }

        public Command StartVideoCommand
        {
            get
            {
                MediaElement mediaElement = null;

                return new Command(() =>
                {
                    // Restarts the video
                    mediaElement = (MediaElement)AboutView.FindName("MediaElement");
                    mediaElement.Position = TimeSpan.Zero; 

                    IsPlayingVideo = true;
                });
            }
        }

        public Command EndVideoCommand
        {
            get { return new Command(() => IsPlayingVideo = false); }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="aboutView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(AboutView aboutView, MainViewModel mainViewModel)
        {
            MediaElement mediaElement = null;

            AboutView = aboutView;
            MainViewModel = mainViewModel;

            mediaElement = (MediaElement)AboutView.FindName("MediaElement");
            mediaElement.Source = new Uri(FolderPaths.VIDEOS + "About.mp4");
        }
    }
}