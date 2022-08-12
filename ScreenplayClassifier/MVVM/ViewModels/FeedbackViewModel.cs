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
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private ImageSource selectedPredictedGenreImage, selectedPredictedSubGenre1Image, selectedPredictedSubGenre2Image;
        private ImageSource selectedActualGenreImage, selectedActualSubGenre1Image, selectedActualSubGenre2Image;
        private int selectedClassifiedScreenplay;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresView PredictedGenresView { get; private set; }
        public GenresView ActualGenresView { get; private set; }

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

        public ImageSource SelectedPredictedGenreImage
        {
            get { return selectedPredictedGenreImage; }
            set
            {
                selectedPredictedGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPredictedGenreImage"));
            }
        }

        public ImageSource SelectedPredictedSubGenre1Image
        {
            get { return selectedPredictedSubGenre1Image; }
            set
            {
                selectedPredictedSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPredictedSubGenre1Image"));
            }
        }
        public ImageSource SelectedPredictedSubGenre2Image
        {
            get { return selectedPredictedSubGenre2Image; }
            set
            {
                selectedPredictedSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPredictedSubGenre2Image"));
            }
        }

        public ImageSource SelectedActualGenreImage
        {
            get { return selectedActualGenreImage; }
            set
            {
                selectedActualGenreImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedActualGenreImage"));
            }
        }

        public ImageSource SelectedActualSubGenre1Image
        {
            get { return selectedActualSubGenre1Image; }
            set
            {
                selectedActualSubGenre1Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedActualSubGenre1Image"));
            }
        }

        public ImageSource SelectedActualSubGenre2Image
        {
            get { return selectedActualSubGenre2Image; }
            set
            {
                selectedActualSubGenre2Image = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedActualSubGenre2Image"));
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

                    ((GenresViewModel)PredictedGenresView.DataContext).Init(selectedScreenplay, "Predicted", PredictedGenresView);
                    ((GenresViewModel)ActualGenresView.DataContext).Init(selectedScreenplay, "Actual", ActualGenresView);
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
        #region Commands
        public Command SubmitFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult startOver;

                    if (!CanSubmit())
                        MessageBoxHandler.Show("Complete feedback for all screenplays", "Error", 3, MessageBoxImage.Error);
                    else
                    {
                        //MessageBoxHandler.Show("Feedback submitted successfuly", "Success", 3, MessageBoxImage.Information);

                        startOver = MessageBox.Show("Would you like to start over?", "something", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        ClassificationViewModel.FeedbackComplete = startOver == MessageBoxResult.No;
                    }
                });
            }
        }
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, FeedbackView feedbackView)
        {
            ClassificationViewModel = classificationViewModel;
            FeedbackView = feedbackView;
            PredictedGenresView = (GenresView)FeedbackView.FindName("PredictedGenresView");
            ActualGenresView = (GenresView)FeedbackView.FindName("ActualGenresView");
        }

        public void Set(ObservableCollection<ClassificationModel> classifiedScreenplays)
        {
            ClassifiedScreenplays = classifiedScreenplays;
            SelectedClassifiedScreenplay = 0;
        }

        public void Reset()
        {
            ClassifiedScreenplays.Clear();
            SelectedClassifiedScreenplay = -1;
        }

        private bool CanSubmit()
        {
            ScreenplayModel currentScreenplay = null;

            for (int i = 0; i < ClassifiedScreenplays.Count; i++)
            {
                currentScreenplay = ClassifiedScreenplays[i].Screenplay;
                if ((currentScreenplay.ActualGenre == "Unknown") || (currentScreenplay.ActualSubGenre1 == "Unknown")
                    || (currentScreenplay.ActualSubGenre2 == "Unknown"))
                    return false;
            }

            return true;
        }
    }
}
