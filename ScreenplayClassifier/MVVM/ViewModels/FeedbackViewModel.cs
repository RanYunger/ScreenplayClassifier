using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> classifiedScreenplays;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public FeedbackView FeedbackView { get; private set; }
        public GenresViewModel PredictedGenresViewModel { get; private set; }
        public GenresViewModel ActualGenresViewModel { get; private set; }

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

        // Constructors
        public FeedbackViewModel() { }

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

                        startOver = MessageBox.Show("Would you like to start over?", "something",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                        ClassificationViewModel.ClassificationComplete = startOver == MessageBoxResult.No;
                    }
                });
            }
        }
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, FeedbackView feedbackView)
        {
            GenresView predictedGenresView, actualGenresView;

            ClassificationViewModel = classificationViewModel;
            FeedbackView = feedbackView;

            predictedGenresView = (GenresView)FeedbackView.FindName("PredictedGenresView");
            PredictedGenresViewModel = (GenresViewModel)predictedGenresView.DataContext;
            PredictedGenresViewModel.Init(predictedGenresView);

            actualGenresView = (GenresView)FeedbackView.FindName("ActualGenresView");
            ActualGenresViewModel = (GenresViewModel)actualGenresView.DataContext;
            ActualGenresViewModel.Init(actualGenresView);
        }

        public void ShowView()
        {
            RefreshView(ClassificationViewModel.ClassifiedScreenplays[0].Screenplay);
            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Visible);
        }

        public void RefreshView(ScreenplayModel selectedScreenplay)
        {
            PredictedGenresViewModel.RefreshView(selectedScreenplay, "Predicted");
            ActualGenresViewModel.RefreshView(selectedScreenplay, "Actual");
        }

        public void HideView()
        {
            App.Current.Dispatcher.Invoke(() => FeedbackView.Visibility = Visibility.Collapsed);
        }
        private bool CanSubmit()
        {
            ScreenplayModel currentScreenplay = null;

            for (int i = 0; i < ClassificationViewModel.ClassifiedScreenplays.Count; i++)
            {
                currentScreenplay = ClassificationViewModel.ClassifiedScreenplays[i].Screenplay;
                if ((currentScreenplay.ActualGenre == "Unknown") || (currentScreenplay.ActualSubGenre1 == "Unknown")
                    || (currentScreenplay.ActualSubGenre2 == "Unknown"))
                    return false;
            }

            return true;
        }
    }
}