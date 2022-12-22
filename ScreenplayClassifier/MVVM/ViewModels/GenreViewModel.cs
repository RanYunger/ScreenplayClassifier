using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenreViewModel : INotifyPropertyChanged
    {
        // Fields
        private string genre, screenplaysCountText;
        private bool audioOn;
        private ImageSource audioImage, genreImage;
        private MediaPlayer mediaPlayer;
        private ObservableCollection<ScreenplayModel> screenplaysInGenre;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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

        // Constructors
        public GenreViewModel()
        {
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
        #endregion

        public void Init(string genreName, ObservableCollection<ScreenplayModel> screenplayClassifiers)
        {
            Genre = genreName;
            GenreImage = new BitmapImage(new Uri(string.Format(@"{0}{1}.png", FolderPaths.GENREIMAGES, genreName)));

            ScreenplaysInGenre = screenplayClassifiers;
            ScreenplaysCountText = string.Format("{0} Screenplay{1}",
                ScreenplaysInGenre.Count, ScreenplaysInGenre.Count != 1 ? "s" : string.Empty);

            mediaPlayer.Open(new Uri(string.Format("{0}{1}.mp3", FolderPaths.GENREAUDIOS, Genre)));
            mediaPlayer.Play();
        }
    }
}
