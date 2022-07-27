using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> activeClassifications, inactiveClassifications;
        private int selectedActiveClassification, selectedInactiveClassification;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ProgressView ProgressView { get; private set; }

        public ObservableCollection<ClassificationModel> ActiveClassifications
        {
            get { return activeClassifications; }
            set
            {
                activeClassifications = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActiveClassifications"));
            }
        }

        public int SelectedActiveClassification
        {
            get { return selectedActiveClassification; }
            set
            {
                selectedActiveClassification = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedActiveClassification"));
            }
        }

        public ObservableCollection<ClassificationModel> InactiveClassifications
        {
            get { return inactiveClassifications; }
            set
            {
                inactiveClassifications = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("InactiveClassifications"));
            }
        }

        public int SelectedInactiveClassification
        {
            get { return selectedInactiveClassification; }
            set
            {
                selectedInactiveClassification = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedInactiveClassification"));
            }
        }

        // Constructors
        public ProgressViewModel()
        {
            ActiveClassifications = new ObservableCollection<ClassificationModel>();
            SelectedActiveClassification = -1;

            InactiveClassifications = new ObservableCollection<ClassificationModel>();
            SelectedInactiveClassification = -1;
        }

        // Methods
        #region Commands
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, ProgressView progressView)
        {
            ClassificationViewModel = classificationViewModel;
            ProgressView = progressView;
        }
    }
}
