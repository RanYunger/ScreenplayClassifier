using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ReportsViewModel
    {
        // Properties
        public ReportsView ReportsView { get; private set; }
        public MainViewModel MainViewModel { get; private set; }


        // Constructors
        public ReportsViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ReportsView reportsView, MainViewModel mainViewModel)
        {
            ReportsView = reportsView;
            MainViewModel = mainViewModel;
        }
    }
}
