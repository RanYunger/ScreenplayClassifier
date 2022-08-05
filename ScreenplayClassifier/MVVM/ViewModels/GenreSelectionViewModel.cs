using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenreSelectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private string affectedGenre, selectedGenre;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public GenreSelectionView GenreSelectionView { get; private set; }
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

        public string SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                selectedGenre = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedGenre"));
            }
        }

        // Constructors
        public GenreSelectionViewModel()
        {
            SelectedGenre = "Unknown";
        }

        // Methods
        #region Command
        public Command CheckSelectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedGenre = GetSelectedGenre();
                    switch (affectedGenre)
                    {
                        case "Genre":
                            Screenplay.DesignatedGenre = SelectedGenre;
                            break;
                        case "SubGenre1":
                            Screenplay.DesignatedSubGenre1 = SelectedGenre;
                            break;
                        case "SubGenre2":
                            Screenplay.DesignatedSubGenre2 = SelectedGenre;
                            break;
                    }

                    ((GenresViewModel)GenresView.DataContext).Init(Screenplay, "Designated", GenresView);
                    GenreSelectionView.Close();
                });
            }
        }
        #endregion

        public void Init(string affectedGenre, ScreenplayModel screenplay, GenreSelectionView genreSelectionView, GenresView genresView)
        {
            AffectedGenre = affectedGenre;
            Screenplay = screenplay;
            GenreSelectionView = genreSelectionView;
            GenresView = genresView;
        }

        public string GetSelectedGenre()
        {
            // TODO: FIX (find which image was pressed)
            MainView mainView = null;
            MainViewModel mainViewModel = null;
            Image image = null;
            List<string> genreNames = null;

            App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
            App.Current.Dispatcher.Invoke(() => mainViewModel = (MainViewModel)mainView.DataContext);

            genreNames = new List<string>(mainViewModel.GenresDictionary.Keys);

            /*for (int i = 0; i < genreNames.Count; i++)
            {
                image = (Image)GenreSelectionView.FindName(genreNames[i] + "Image");
                if (image.IsFocused)
                    return genreNames[i];
            }*/

            return "Horror"; //"Unknown";
        }
    }
}
