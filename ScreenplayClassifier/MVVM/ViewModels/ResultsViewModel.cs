using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ResultsViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ScreenplayModel> classifiedScreenplays;
        private int selectedClassifiedScreenplay;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ResultsView ResultsView { get; private set; }

        public ObservableCollection<ScreenplayModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }
        public int SelectedClassifiedScreenplay
        {
            get { return selectedClassifiedScreenplay; }
            set
            {
                selectedClassifiedScreenplay = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedClassifiedScreenplay"));
            }
        }

        // Constructors
        public ResultsViewModel()
        {
            ClassifiedScreenplays = new ObservableCollection<ScreenplayModel>();
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
