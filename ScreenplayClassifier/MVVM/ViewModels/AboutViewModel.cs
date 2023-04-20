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
        private bool isPlaying;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public AboutView AboutView { get; private set; }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;

                if (isPlaying)
                    PlayVideoCommand.Execute(null);
                else
                    InterruptVideoCommand.Execute(null);

                NotifyPropertyChange();
            }
        }

        // Constructors
        public AboutViewModel() { }

        // Methods
        #region Commands
        public Command PlayVideoCommand
        {
            get
            {
                MediaElement mediaElement = (MediaElement)AboutView.FindName("MediaElement");

                return new Command(() =>
                {
                    mediaElement.Source = new Uri(FolderPaths.VIDEOS + "About.mp4");

                    mediaElement.Play();
                });
            }
        }

        public Command InterruptVideoCommand
        {
            get
            {
                MediaElement mediaElement = (MediaElement)AboutView.FindName("MediaElement");

                return new Command(() => mediaElement.Stop());
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="aboutView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(AboutView aboutView, MainViewModel mainViewModel)
        {
            AboutView = aboutView;
            MainViewModel = mainViewModel;
        }
    }
}