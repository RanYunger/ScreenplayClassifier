using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenreViewModel : INotifyPropertyChanged
    {
        // Fields
        private ImageSource genreImage;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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

        // Constructors
        public GenreViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ImageSource genreImage)
        {
            GenreImage = genreImage;
        }
    }
}
