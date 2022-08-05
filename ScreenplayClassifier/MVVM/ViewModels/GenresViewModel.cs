using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenresViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private ImageSource genreImage, subGenre1Image, subGenre2Image;
        private string affectedGenre;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public GenresView GenresView { get; private set; }

        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplay"));
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

        public ImageSource SubGenre1Image
        {
            get { return subGenre1Image; }
            set
            {
                subGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1Image"));
            }
        }

        public ImageSource SubGenre2Image
        {
            get { return subGenre2Image; }
            set
            {
                subGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2Image"));
            }
        }

        public string AffectedGenre
        {
            get { return affectedGenre; }
            set
            {
                affectedGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AffectedGenre"));
            }
        }

        // Constructors
        public GenresViewModel()
        {
            AffectedGenre = string.Empty;
        }

        // Methods
        #region Commands
        public Command SelectGenreCommand
        {
            get
            {
                return new Command(() =>
                {
                    affectedGenre = "Genre";
                    ShowGenreSelectionViewCommand.Execute(null);
                });
            }
        }
        public Command SelectSubGenre1Command
        {
            get
            {
                return new Command(() =>
                {
                    affectedGenre = "SubGenre1";
                    ShowGenreSelectionViewCommand.Execute(null);
                });
            }
        }
        public Command SelectSubGenre2Command
        {
            get
            {
                return new Command(() =>
                {
                    AffectedGenre = "SubGenre2";
                    ShowGenreSelectionViewCommand.Execute(null);
                });
            }
        }

        public Command ShowGenreSelectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    GenreSelectionView genreSelectionView = null;
                    string genreType = string.Empty;

                    foreach (Window view in App.Current.Windows)
                        if(view is GenreSelectionView)
                        {
                            view.Focus();
                            return;
                        }

                    genreSelectionView = new GenreSelectionView();
                    ((GenreSelectionViewModel)genreSelectionView.DataContext).Init(AffectedGenre, Screenplay, genreSelectionView, GenresView);
                    genreSelectionView.Show();
                });
            }
        }
        #endregion

        public void Init(ScreenplayModel screenplay, string genresAffiliation, GenresView genresView)
        {
            bool isDesignated = genresAffiliation == "Designated";
            string genreImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                isDesignated ? screenplay.DesignatedGenre : screenplay.ClassifiedGenre),
                subGenre1ImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                isDesignated ? screenplay.DesignatedSubGenre1 : screenplay.ClassifiedSubGenre1),
                subGenre2ImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                isDesignated ? screenplay.DesignatedSubGenre2 : screenplay.ClassifiedSubGenre2);

            Screenplay = screenplay;
            GenreImage = new BitmapImage(new Uri(genreImagePath));
            SubGenre1Image = new BitmapImage(new Uri(subGenre1ImagePath));
            SubGenre2Image = new BitmapImage(new Uri(subGenre2ImagePath));
            GenresView = genresView;
        }
    }
}
