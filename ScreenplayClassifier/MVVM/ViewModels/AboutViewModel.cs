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
        private Timer videoTimer;
        private bool isPlaying;

        public event PropertyChangedEventHandler PropertyChanged;

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

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPlaying"));
            }
        }

        // Constructors
        public AboutViewModel()
        {
            videoTimer = new Timer(141000);
            videoTimer.Elapsed += VideoTimer_Elapsed;
        }

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

                    videoTimer.Start();
                    mediaElement.Play();
                });
            }
        }

        public Command InterruptVideoCommand
        {
            get
            {
                MediaElement mediaElement = (MediaElement)AboutView.FindName("MediaElement");

                return new Command(() =>
                {
                    videoTimer.Stop();
                    mediaElement.Stop();
                });
            }
        }

        private void VideoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => MainViewModel.UserToolbarViewModel.ShowHomeCommand.Execute(null));
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