using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ReportsViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> classificationReports;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ReportsView ReportsView { get; private set; }

        public ObservableCollection<ClassificationModel> ClassificationReports
        {
            get { return classificationReports; }
            set
            {
                classificationReports = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationReports"));
            }
        }

        // Constructors
        public ReportsViewModel()
        {
            ClassificationReports = new ObservableCollection<ClassificationModel>();
        }

        // Methods
        #region Commands
        public Command FilterReportsCommand
        {
            get
            {
                return new Command(() =>
                {
                });
            }
        }
        public Command ClearFilterCommand
        {
            get
            {
                return new Command(() =>
                {
                });
            }
        }
        #endregion

        public void Init(ReportsView reportsView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ReportsView = reportsView;
        }
    }
}
