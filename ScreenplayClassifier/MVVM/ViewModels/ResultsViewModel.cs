using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
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
    public class ResultsViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private ImageSource selectedGenreImage, selectedSubGenre1Image, selectedSubGenre2Image;
        private int selectedClassifiedScreenplay;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ResultsView ResultsView { get; private set; }

        public ObservableCollection<ClassificationModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public ImageSource SelectedGenreImage
        {
            get { return selectedGenreImage; }
            set
            {
                selectedGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedGenreImage"));
            }
        }

        public ImageSource SelectedSubGenre1Image
        {
            get { return selectedSubGenre1Image; }
            set
            {
                selectedSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedSubGenre1Image"));
            }
        }
        public ImageSource SelectedSubGenre2Image
        {
            get { return selectedSubGenre2Image; }
            set
            {
                selectedSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedSubGenre2Image"));
            }
        }

        public int SelectedClassifiedScreenplay
        {
            get { return selectedClassifiedScreenplay; }
            set
            {
                string genreImagePath = string.Empty, subGenre1ImagePath = string.Empty, subGenre2ImagePath = string.Empty;

                selectedClassifiedScreenplay = value;

                if (selectedClassifiedScreenplay != -1)
                {
                    genreImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                        ClassifiedScreenplays[selectedClassifiedScreenplay].Screenplay.Genre);
                    subGenre1ImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                        ClassifiedScreenplays[selectedClassifiedScreenplay].Screenplay.SubGenre1);
                    subGenre2ImagePath = string.Format("{0}{1}.png", FolderPaths.GENREIMAGES,
                        ClassifiedScreenplays[selectedClassifiedScreenplay].Screenplay.SubGenre2);

                    SelectedGenreImage = new BitmapImage(new Uri(genreImagePath));
                    SelectedSubGenre1Image = new BitmapImage(new Uri(subGenre1ImagePath));
                    SelectedSubGenre2Image = new BitmapImage(new Uri(subGenre2ImagePath));
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedScreenplay"));
            }
        }

        // Constructors
        public ResultsViewModel()
        {
            ClassifiedScreenplays = new ObservableCollection<ClassificationModel>();
            SelectedClassifiedScreenplay = -1;
        }

        // Methods
        #region Commands
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, ResultsView resultsView)
        {
            ClassificationViewModel = classificationViewModel;
            ResultsView = resultsView;
        }
    }
}
