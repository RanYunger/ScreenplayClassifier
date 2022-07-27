using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ReportsViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ReportsView ReportsView { get; private set; }

        // Constructors
        public ReportsViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ReportsView reportsView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ReportsView = reportsView;
        }
    }
}
