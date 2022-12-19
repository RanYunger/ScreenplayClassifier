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

        public Timer VideoTimer
        {
            get { return videoTimer; }
            set
            {
                videoTimer = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("VideoTimer"));
            }
        }

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
        public Command ScreamCommand
        {
            get
            {
                return new Command(() =>
                {
                    MediaPlayer mediaPlayer = new MediaPlayer();

                    mediaPlayer.Open(new Uri(FolderPaths.AUDIOS + "Wilhelm Scream.mp3"));
                    mediaPlayer.Play();
                });
            }
        }

        public Command PlayVideoCommand
        {
            get
            {
                MediaElement curtainsMediaElement = (MediaElement)AboutView.FindName("CurtainsMediaElement");

                return new Command(() =>
                {
                    curtainsMediaElement.Source = new Uri(FolderPaths.VIDEOS + "Curtains.mp4");
                    curtainsMediaElement.Play();

                    videoTimer.Start();
                });
            }
        }

        public Command InterruptVideoCommand
        {
            get { return new Command(() => VideoTimer.Stop()); }
        }
        #endregion

        public void Init(AboutView aboutView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            AboutView = aboutView;

            VideoTimer = new Timer(6000);
            VideoTimer.Elapsed += VideoTimer_Elapsed;
        }

        private void VideoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            VideoTimer.Stop();
            IsPlaying = false;
        }
    }
}
