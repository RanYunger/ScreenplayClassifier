using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<string> ownerGenreOptions, ownerSubGenre1Options, ownerSubGenre2Options;
        private ImageSource modelGenreImage, modelSubGenre1Image, modelSubGenre2Image;
        private ImageSource ownerGenreImage, ownerSubGenre1Image, ownerSubGenre2Image;
        private string selectedOwnerGenre, selectedOwnerSubGenre1, selectedOwnerSubGenre2;
        private bool canChoose;

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

        public ObservableCollection<string> OwnerGenreOptions
        {
            get { return ownerGenreOptions; }
            set
            {
                ownerGenreOptions = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerGenreOptions"));
            }
        }

        public ObservableCollection<string> OwnerSubGenre1Options
        {
            get { return ownerSubGenre1Options; }
            set
            {
                ownerSubGenre1Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre1Options"));
            }
        }

        public ObservableCollection<string> OwnerSubGenre2Options
        {
            get { return ownerSubGenre2Options; }
            set
            {
                ownerSubGenre2Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre2Options"));
            }
        }

        public ImageSource ModelGenreImage
        {
            get { return modelGenreImage; }
            set
            {
                modelGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModelGenreImage"));
            }
        }

        public ImageSource ModelSubGenre1Image
        {
            get { return modelSubGenre1Image; }
            set
            {
                modelSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModelSubGenre1Image"));
            }
        }

        public ImageSource ModelSubGenre2Image
        {
            get { return modelSubGenre2Image; }
            set
            {
                modelSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModelSubGenre2Image"));
            }
        }

        public ImageSource OwnerGenreImage
        {
            get { return ownerGenreImage; }
            set
            {
                ownerGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerGenreImage"));
            }
        }

        public ImageSource OwnerSubGenre1Image
        {
            get { return ownerSubGenre1Image; }
            set
            {
                ownerSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerSubGenre1Image"));
            }
        }

        public ImageSource OwnerSubGenre2Image
        {
            get { return ownerSubGenre2Image; }
            set
            {
                ownerSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ownerSubGenre2Image"));
            }
        }

        public string SelectedOwnerGenre
        {
            get { return selectedOwnerGenre; }
            set
            {
                selectedOwnerGenre = value;

                if (selectedOwnerGenre != null)
                {
                    Screenplay.OwnerGenre = selectedOwnerGenre;

                    // Refreshes the genre image
                    OwnerGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerGenre)));

                    // Restores options to their default collection
                    OwnerSubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                    OwnerSubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerSubGenre1Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre1Options.Remove(selectedOwnerSubGenre2);
                    OwnerSubGenre1Options = OwnerSubGenre1Options;

                    OwnerSubGenre2Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre2Options.Remove(selectedOwnerSubGenre1);
                    OwnerSubGenre2Options = OwnerSubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOwnerGenre"));
            }
        }

        public string SelectedOwnerSubGenre1
        {
            get { return selectedOwnerSubGenre1; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                selectedOwnerSubGenre1 = value;

                if (selectedOwnerSubGenre1 != null)
                {
                    Screenplay.OwnerSubGenre1 = selectedOwnerSubGenre1;

                    // Refreshes the genre image
                    OwnerSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerSubGenre1)));

                    // Restores options to their default collection
                    OwnerGenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    OwnerSubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre1);
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre2);
                    OwnerGenreOptions = OwnerGenreOptions;

                    OwnerSubGenre2Options.Remove(selectedOwnerSubGenre1);
                    OwnerSubGenre2Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre2Options = OwnerSubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOwnerSubGenre1"));
            }
        }

        public string SelectedOwnerSubGenre2
        {
            get { return selectedOwnerSubGenre2; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                selectedOwnerSubGenre2 = value;

                if (selectedOwnerSubGenre2 != null)
                {
                    Screenplay.OwnerSubGenre2 = selectedOwnerSubGenre2;

                    // Refreshes the genre image
                    OwnerSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.OwnerSubGenre2)));

                    // Restores options to their default collection
                    OwnerGenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    OwnerSubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre2);
                    OwnerGenreOptions.Remove(selectedOwnerSubGenre1);
                    OwnerGenreOptions = OwnerGenreOptions;

                    OwnerSubGenre1Options.Remove(selectedOwnerSubGenre2);
                    OwnerSubGenre1Options.Remove(selectedOwnerGenre);
                    OwnerSubGenre1Options = OwnerSubGenre1Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOwnerSubGenre2"));
            }
        }

        public bool CanChoose
        {
            get { return canChoose; }
            set
            {
                canChoose = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanChoose"));
            }
        }

        // Constructors
        public GenresViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="genresView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show the genres of</param>
        /// <param name="canChoose">The indication whether the user can choose genres</param>
        public void Init(GenresView genresView, ScreenplayModel screenplay, bool canChoose)
        {
            GenresView = genresView;
            Screenplay = screenplay;
            CanChoose = canChoose;

            ModelGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.ModelGenre)));
            ModelSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.ModelSubGenre1)));
            ModelSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, Screenplay.ModelSubGenre2)));

            OwnerGenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanChoose ? "Unknown"
                : Screenplay.OwnerGenre)));
            OwnerSubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanChoose ? "Unknown"
                : Screenplay.OwnerSubGenre1)));
            OwnerSubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, CanChoose ? "Unknown"
                : Screenplay.OwnerSubGenre2)));

            if (CanChoose)
            {
                SelectedOwnerGenre = screenplay.OwnerGenre;
                SelectedOwnerSubGenre1 = screenplay.OwnerSubGenre1;
                SelectedOwnerSubGenre2 = screenplay.OwnerSubGenre2;
            }
        }
    }
}