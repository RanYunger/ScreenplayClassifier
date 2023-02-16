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
        private ObservableCollection<string> genreOptions, subGenre1Options, subGenre2Options;
        private ImageSource genreImage, subGenre1Image, subGenre2Image;
        private string selectedGenre, selectedSubGenre1, selectedSubGenre2, genresAffiliation;
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

        public ObservableCollection<string> GenreOptions
        {
            get { return genreOptions; }
            set
            {
                genreOptions = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreOptions"));
            }
        }

        public ObservableCollection<string> SubGenre1Options
        {
            get { return subGenre1Options; }
            set
            {
                subGenre1Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1Options"));
            }
        }

        public ObservableCollection<string> SubGenre2Options
        {
            get { return subGenre2Options; }
            set
            {
                subGenre2Options = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2Options"));
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

        public string SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                selectedGenre = value;

                if (selectedGenre != null)
                {
                    if (GenresAffiliation == "Model")
                        Screenplay.ModelGenre = selectedGenre;
                    else
                        Screenplay.UserGenre = selectedGenre;

                    // Refreshes the genre image
                    GenreImage = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, GenresAffiliation == "Model" ?
                        Screenplay.ModelGenre : Screenplay.UserGenre)));

                    // Restores options to their default collection
                    SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    SubGenre1Options.Remove(selectedGenre);
                    SubGenre1Options.Remove(selectedSubGenre2);
                    SubGenre1Options = SubGenre1Options;

                    SubGenre2Options.Remove(selectedGenre);
                    SubGenre2Options.Remove(selectedSubGenre1);
                    SubGenre2Options = SubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedGenre"));
            }
        }

        public string SelectedSubGenre1
        {
            get { return selectedSubGenre1; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                selectedSubGenre1 = value;

                if (selectedSubGenre1 != null)
                {
                    if (GenresAffiliation == "Model")
                        Screenplay.ModelSubGenre1 = selectedSubGenre1;
                    else
                        Screenplay.UserSubGenre1 = selectedSubGenre1;

                    // Refreshes the genre image
                    SubGenre1Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, GenresAffiliation == "Model" ?
                        Screenplay.ModelSubGenre1 : Screenplay.UserSubGenre1)));

                    // Restores options to their default collection
                    GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre2Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    GenreOptions.Remove(selectedSubGenre1);
                    GenreOptions.Remove(selectedSubGenre2);
                    GenreOptions = GenreOptions;

                    SubGenre2Options.Remove(selectedSubGenre1);
                    SubGenre2Options.Remove(selectedGenre);
                    SubGenre2Options = SubGenre2Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedSubGenre1"));
            }
        }

        public string SelectedSubGenre2
        {
            get { return selectedSubGenre2; }
            set
            {
                ObservableCollection<string> selectedOptions = new ObservableCollection<string>(JSON.LoadedGenres);

                selectedSubGenre2 = value;

                if (selectedSubGenre2 != null)
                {
                    if (GenresAffiliation == "Model")
                        Screenplay.ModelSubGenre2 = selectedSubGenre2;
                    else
                        Screenplay.UserSubGenre2 = selectedSubGenre2;

                    // Refreshes the genre image
                    SubGenre2Image = new BitmapImage(new Uri(string.Format("{0}{1}.png", FolderPaths.GENREIMAGES, GenresAffiliation == "Model" ?
                        Screenplay.ModelSubGenre2 : Screenplay.UserSubGenre2)));

                    // Restores options to their default collection
                    GenreOptions = new ObservableCollection<string>(JSON.LoadedGenres);
                    SubGenre1Options = new ObservableCollection<string>(JSON.LoadedGenres);

                    // Removes selected option from other collections to prevent duplications
                    GenreOptions.Remove(selectedSubGenre2);
                    GenreOptions.Remove(selectedSubGenre1);
                    GenreOptions = GenreOptions;

                    SubGenre1Options.Remove(selectedSubGenre2);
                    SubGenre1Options.Remove(selectedGenre);
                    SubGenre1Options = SubGenre1Options;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedSubGenre2"));
            }
        }

        public string GenresAffiliation
        {
            get { return genresAffiliation; }
            set
            {
                genresAffiliation = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenresAffiliation"));
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
        public void Init(GenresView genresView, ScreenplayModel screenplay, string affiliation)
        {
            GenresView = genresView;
            Screenplay = screenplay;
            GenresAffiliation = affiliation;

            SelectedGenre = affiliation == "Model" ? screenplay.ModelGenre : screenplay.UserGenre;
            SelectedSubGenre1 = affiliation == "Model" ? screenplay.ModelSubGenre1 : screenplay.UserSubGenre1;
            SelectedSubGenre2 = affiliation == "Model" ? screenplay.ModelSubGenre2 : screenplay.UserSubGenre2;
        }
    }
}