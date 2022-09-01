using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
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
        private ObservableCollection<ClassificationModel> reports;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ReportsView ReportsView { get; private set; }

        public ObservableCollection<ClassificationModel> Reports
        {
            get { return reports; }
            set
            {
                reports = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationReports"));
            }
        }

        // Constructors
        public ReportsViewModel() { }

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
            Reports = Storage.LoadReports();
        }
    }
}
