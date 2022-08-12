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
        private TimeSpan selectedDuration;
        private int selectedClassifiedScreenplay;
        private bool isSelected;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ResultsView ResultsView { get; private set; }
        public GenresView GenresView { get; private set; }

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

        public TimeSpan SelectedDuration
        {
            get { return selectedDuration; }
            set
            {
                selectedDuration = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDuration"));
            }
        }
        public int SelectedClassifiedScreenplay
        {
            get { return selectedClassifiedScreenplay; }
            set
            {
                ScreenplayModel selectedScreenplay;
                string genreName = string.Empty, subGenre1Name = string.Empty, subGenre2Name = string.Empty;

                selectedClassifiedScreenplay = value;
                IsSelected = selectedClassifiedScreenplay != -1;

                if (selectedClassifiedScreenplay != -1)
                {
                    selectedScreenplay = ClassifiedScreenplays[selectedClassifiedScreenplay].Screenplay;
                    genreName = selectedScreenplay.PredictedGenre;
                    subGenre1Name = selectedScreenplay.PredictedSubGenre1;
                    subGenre2Name = selectedScreenplay.PredictedSubGenre2;

                    ((GenresViewModel)GenresView.DataContext).Init(selectedScreenplay, "Predicted", GenresView);
                    SelectedDuration = ClassifiedScreenplays[selectedClassifiedScreenplay].Duration;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedScreenplay"));
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
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
            GenresView = (GenresView)ResultsView.FindName("GenresView");
        }

        public void Set()
        {
            SelectedClassifiedScreenplay = 0;
        }

        public void Reset() 
        {
            ClassifiedScreenplays.Clear();
            SelectedClassifiedScreenplay = -1;
        }
    }
}
