using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
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
        #region Commands
        public Command SelectActionGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Action"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectAdventureGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Adventure"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectComedyGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Comedy"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectCrimeGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Crime"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectDramaGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Drama"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectFamilyGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Family"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectFantasyGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Fantasy"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectHorrorGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Horror"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectRomanceGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Romance"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectSciFiGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "SciFi"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectThrillerGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "Thriller"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command SelectWarGenreCommand
        {
            get { return new Command(() => { SelectedGenre = "War"; ChangeGenreCommand.Execute(null); }); }
        }

        public Command ChangeGenreCommand
        {
            get
            {
                return new Command(() =>
                {
                    switch (affectedGenre)
                    {
                        case "Genre":
                            {
                                if ((Screenplay.ActualSubGenre1 == SelectedGenre) || (Screenplay.ActualSubGenre2 == SelectedGenre))
                                    MessageBoxHandler.Show(SelectedGenre + " genre already selected", "Error", 3, MessageBoxImage.Error);
                                else
                                {
                                    Screenplay.ActualGenre = SelectedGenre;
                                    ((GenresViewModel)GenresView.DataContext).Init(Screenplay, "Actual", GenresView);
                                    GenreSelectionView.Close();
                                }
                            }
                            break;
                        case "SubGenre1":
                            {
                                if ((Screenplay.ActualGenre == SelectedGenre) || (Screenplay.ActualSubGenre2 == SelectedGenre))
                                    MessageBoxHandler.Show(SelectedGenre + " genre already selected", "Error", 3, MessageBoxImage.Error);
                                else
                                {
                                    Screenplay.ActualSubGenre1 = SelectedGenre;
                                    ((GenresViewModel)GenresView.DataContext).Init(Screenplay, "Actual", GenresView);
                                    GenreSelectionView.Close();
                                }
                            }
                            break;
                        case "SubGenre2":
                            {
                                if ((Screenplay.ActualGenre == SelectedGenre) || (Screenplay.ActualSubGenre1 == SelectedGenre))
                                    MessageBoxHandler.Show(SelectedGenre + " genre already selected", "Error", 3, MessageBoxImage.Error);
                                else
                                {
                                    Screenplay.ActualSubGenre2 = SelectedGenre;
                                    ((GenresViewModel)GenresView.DataContext).Init(Screenplay, "Actual", GenresView);
                                    GenreSelectionView.Close();
                                }
                            }
                            break;
                    }


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
    }
}
