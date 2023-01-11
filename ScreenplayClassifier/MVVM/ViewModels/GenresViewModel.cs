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
                    AffectedGenre = "Genre";
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
                    AffectedGenre = "SubGenre1";
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
                    GenreSelectionViewModel genreSelectionViewModel = null;
                    string genreType = string.Empty;

                    // Focuses on an existing GenreSelectionView (if there's one)
                    foreach (Window view in App.Current.Windows)
                        if (view is GenreSelectionView)
                        {
                            genreSelectionViewModel = (GenreSelectionViewModel)view.DataContext;

                            if (genreSelectionViewModel.AffectedGenre.Equals(AffectedGenre))
                            {
                                view.Focus();
                                return;
                            }
                            else
                            {
                                view.Close();
                                break;
                            }
                        }

                    // Opens a new GenreSelectionView
                    genreSelectionView = new GenreSelectionView();
                    ((GenreSelectionViewModel)genreSelectionView.DataContext).Init(AffectedGenre, Screenplay, genreSelectionView, GenresView);
                    genreSelectionView.Show();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="genresView">The view to obtain controls from</param>
        public void Init(GenresView genresView)
        {
            GenresView = genresView;
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        /// <param name="screenplay">The screenplay the genres of which are shown in the view</param>
        /// <param name="genresAffiliation">The genres' affiliation: Predicted/Actual</param>
        public void RefreshView(ScreenplayModel screenplay, string genresAffiliation)
        {
            bool isActual = genresAffiliation == "Actual";
            string genreImagePath = string.Format("{0}Unknown.png", FolderPaths.GENREIMAGES),
                subGenre1ImagePath = string.Format("{0}Unknown.png", FolderPaths.GENREIMAGES),
                subGenre2ImagePath = string.Format("{0}Unknown.png", FolderPaths.GENREIMAGES);

            if (screenplay != null)
            {
                // Updates the images representing the screenplay's genres.
                genreImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                    isActual ? screenplay.ActualGenre : screenplay.PredictedGenre);
                subGenre1ImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                    isActual ? screenplay.ActualSubGenre1 : screenplay.PredictedSubGenre1);
                subGenre2ImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                    isActual ? screenplay.ActualSubGenre2 : screenplay.PredictedSubGenre2);
            }

            Screenplay = screenplay;
            GenreImage = new BitmapImage(new Uri(genreImagePath));
            SubGenre1Image = new BitmapImage(new Uri(subGenre1ImagePath));
            SubGenre2Image = new BitmapImage(new Uri(subGenre2ImagePath));
        }
    }
}
