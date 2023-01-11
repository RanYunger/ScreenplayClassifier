using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        // Fields
        private bool isPlaying;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPlaying"));
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
                    mediaElement.Source = new Uri(FolderPaths.VIDEOS + "Credits.mp4");
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
        public void Init(AboutView aboutView)
        {
            AboutView = aboutView;
        }
    }
}