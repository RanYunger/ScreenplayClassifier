using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows.Controls;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        // Fields
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
                return new Command(() =>
                {
                    MediaElement mediaElement = (MediaElement)AboutView.FindName("MediaElement");
                    Timer videoTimer = new Timer(12900);

                    IsPlaying = true;

                    mediaElement.Source = new Uri(FolderPaths.VIDEOS + "Countdown.mp4");
                    mediaElement.Play();

                    videoTimer.Elapsed += VideoTimer_Elapsed;
                    videoTimer.Start();
                });
            }
        }

        private void VideoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsPlaying = false;
        }
        #endregion

        public void Init(AboutView aboutView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            AboutView = aboutView;
        }
    }
}
