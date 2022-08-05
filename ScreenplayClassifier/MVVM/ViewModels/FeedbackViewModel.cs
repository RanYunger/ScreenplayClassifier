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
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private ImageSource selectedClassifiedGenreImage, selectedClassifiedSubGenre1Image, selectedClassifiedSubGenre2Image;
        private ImageSource selectedDesignatedGenreImage, selectedDesignatedSubGenre1Image, selectedDesignatedSubGenre2Image;
        private int selectedClassifiedScreenplay;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresView ClassifiedGenresView { get; private set; }
        public GenresView DesignatedGenresView { get; private set; }

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

        public ImageSource SelectedClassifiedGenreImage
        {
            get { return selectedClassifiedGenreImage; }
            set
            {
                selectedClassifiedGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedGenreImage"));
            }
        }

        public ImageSource SelectedClassifiedSubGenre1Image
        {
            get { return selectedClassifiedSubGenre1Image; }
            set
            {
                selectedClassifiedSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedSubGenre1Image"));
            }
        }
        public ImageSource SelectedClassifiedSubGenre2Image
        {
            get { return selectedClassifiedSubGenre2Image; }
            set
            {
                selectedClassifiedSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedSubGenre2Image"));
            }
        }

        public ImageSource SelectedDesignatedGenreImage
        {
            get { return selectedDesignatedGenreImage; }
            set
            {
                selectedDesignatedGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDesignatedGenreImage"));
            }
        }

        public ImageSource SelectedDesignatedSubGenre1Image
        {
            get { return selectedDesignatedSubGenre1Image; }
            set
            {
                selectedDesignatedSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDesignatedSubGenre1Image"));
            }
        }

        public ImageSource SelectedDesignatedSubGenre2Image
        {
            get { return selectedDesignatedSubGenre2Image; }
            set
            {
                selectedDesignatedSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDesignatedSubGenre2Image"));
            }
        }

        public int SelectedClassifiedScreenplay
        {
            get { return selectedClassifiedScreenplay; }
            set
            {
                ScreenplayModel selectedScreenplay = null;

                selectedClassifiedScreenplay = value;

                if (selectedClassifiedScreenplay != -1)
                {
                    selectedScreenplay = ClassifiedScreenplays[selectedClassifiedScreenplay].Screenplay;

                    ((GenresViewModel)ClassifiedGenresView.DataContext).Init(selectedScreenplay, "Classified", ClassifiedGenresView);
                    ((GenresViewModel)DesignatedGenresView.DataContext).Init(selectedScreenplay, "Classified", DesignatedGenresView);
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedScreenplay"));
            }
        }

        // Constructors
        public FeedbackViewModel()
        {
            ClassifiedScreenplays = new ObservableCollection<ClassificationModel>();

            SelectedClassifiedScreenplay = -1;
        }

        // Methods
        public void Init(ClassificationViewModel classificationViewModel, FeedbackView feedbackView)
        {
            ClassificationViewModel = classificationViewModel;
            FeedbackView = feedbackView;
            ClassifiedGenresView = (GenresView)FeedbackView.FindName("ClassifiedGenresView");
            DesignatedGenresView = (GenresView)FeedbackView.FindName("DesignatedGenresView");
        }
    }
}
