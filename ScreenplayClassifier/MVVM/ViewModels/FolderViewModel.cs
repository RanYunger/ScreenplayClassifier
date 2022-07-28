using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FolderViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource folderImage, genreImage;
        private string screenplaysCountText;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public FolderView FolderView { get; private set; }

        public ImageSource FolderImage
        {
            get { return folderImage; }
            set
            {
                folderImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FolderImage"));
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

        // Constructors
        public FolderViewModel() { }

        // Methods
        #region Commands
        public Command ShowGenreViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreView genreView = new GenreView();

                    ((GenreViewModel)genreView.DataContext).Init(GenreImage);
                    genreView.Show();
                });
            }
        }
        #endregion

        public void Init(string genreName, int itemsCount)
        {
            string folderImageFilePath = string.Format("{0}{1}", FolderPaths.IMAGES, itemsCount > 0 ? "FullFolder.png" : "EmptyFolder.png"),
                genreImageFilePath = string.Format(@"{0}{1}.png", FolderPaths.GENREIMAGES, genreName);

            FolderImage = new BitmapImage(new Uri(folderImageFilePath));
            GenreImage = new BitmapImage(new Uri(genreImageFilePath));
            ScreenplaysCountText = string.Format("{0} {1}", itemsCount, itemsCount == 1 ? "Screenplay" : "Screenplays");
        }
    }
}
