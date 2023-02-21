using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenreViewModel : INotifyPropertyChanged
    {
        // Fields
        private Timer audioTimer;
        private ImageSource audioImage, genreImage, genreGif;
        private MediaPlayer mediaPlayer;
        private ObservableCollection<ScreenplayModel> screenplaysInGenre;
        private string genre, screenplaysCountText;
        private bool audioOn;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public GenreView GenreView { get; private set; }

        public ImageSource AudioImage
        {
            get { return audioImage; }
            set
            {
                audioImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AudioImage"));
            }
        }

        public ImageSource GenreImage
        {
            get { return genreImage; }
            set
            {
                genreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreImage"));
            }
        }

        public ImageSource GenreGif
        {
            get { return genreGif; }
            set
            {
                genreGif = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreGif"));
            }
        }

        public ObservableCollection<ScreenplayModel> ScreenplaysInGenre
        {
            get { return screenplaysInGenre; }
            set
            {
                screenplaysInGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplaysInGenre"));
            }
        }

        public string Genre
        {
            get { return genre; }
            set
            {
                genre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Genre"));
            }
        }

        public string ScreenplaysCountText
        {
            get { return screenplaysCountText; }
            set
            {
                screenplaysCountText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplaysCountText"));
            }
        }

        public bool AudioOn
        {
            get { return audioOn; }
            set
            {
                audioOn = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AudioOn"));
            }
        }

        // Constructors
        public GenreViewModel()
        {
            audioTimer = new Timer(2500) { AutoReset = false };
            audioTimer.Elapsed += AudioTimer_Elapsed;

            mediaPlayer = new MediaPlayer();

            AudioOn = true;
            AudioImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "AudioOn.png"));
        }

        // Methods
        #region Commands
        public Command ToggleAudioCommand
        {
            get
            {
                return new Command(() =>
                {
                    string imagePath = string.Empty;

                    // Toggles the audio's image and state
                    AudioOn = !AudioOn;

                    if (AudioOn)
                        mediaPlayer.Play();
                    else
                        mediaPlayer.Pause();

                    imagePath = string.Format("{0}Audio{1}.png", FolderPaths.IMAGES, AudioOn ? "On" : "Off");
                    AudioImage = new BitmapImage(new Uri(imagePath));

                });
            }
        }

        public Command StopMusicCommand
        {
            get { return new Command(() => mediaPlayer.Stop()); }
        }

        private void AudioTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => mediaPlayer.Open(new Uri(string.Format("{0}{1}.mp3", FolderPaths.GENREAUDIOS, Genre))));
            App.Current.Dispatcher.Invoke(() => mediaPlayer.Play());
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="genreView">The view to obtain controls from</param>
        /// <param name="genreName">The genre to be represented by the GenreView</param>
        /// <param name="screenplayClassifiers">The MainView's view model</param>
        public void Init(GenreView genreView, string genreName, ObservableCollection<ScreenplayModel> screenplayClassifiers)
        {
            GenreView = genreView;

            Genre = genreName;
            GenreImage = new BitmapImage(new Uri(string.Format(@"{0}{1}.png", FolderPaths.GENREIMAGES, Genre)));
            GenreGif = new BitmapImage(new Uri(string.Format(@"{0}{1}.gif", FolderPaths.GENREGIFS, Genre)));

            ScreenplaysInGenre = screenplayClassifiers;
            ScreenplaysCountText = string.Format("{0} Screenplay{1}",
                ScreenplaysInGenre.Count, ScreenplaysInGenre.Count != 1 ? "s" : string.Empty);

            audioTimer.Start();
        }
    }
}