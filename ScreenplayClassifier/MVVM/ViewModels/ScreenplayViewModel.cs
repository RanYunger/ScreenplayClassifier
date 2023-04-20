using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ScreenplayViewModel : PropertyChangeNotifier
    {
        // Fields
        private ScreenplayModel screenplay;
        private string screenplayTitle;

        // Properties
        public ScreenplayView ScreenplayView { get; set; }
        public ScreenplayOverviewViewModel ScreenplayOverviewViewModel { get; private set; }
        public ScreenplayInspectionViewModel ScreenplayInspectionViewModel { get; private set; }

        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;

                NotifyPropertyChange();
            }
        }
        public string ScreenplayTitle
        {
            get { return screenplayTitle; }
            set
            {
                screenplayTitle = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ScreenplayViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="screenplayView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show in the ScreenplayView</param>
        /// <param name="canGiveFeedback">The indication whether the user can give feedback</param>
        public void Init(ScreenplayView screenplayView, ScreenplayModel screenplay, bool canGiveFeedback)
        {
            ScreenplayView = screenplayView;

            Screenplay = screenplay;
            ScreenplayTitle = screenplay.Title;

            ScreenplayOverviewView screenplayOverviewView = null;
            ScreenplayInspectionView screenplayInspectionView = null;

            screenplayOverviewView = (ScreenplayOverviewView)ScreenplayView.FindName("ScreenplayOverviewView");
            ScreenplayOverviewViewModel = (ScreenplayOverviewViewModel)screenplayOverviewView.DataContext;
            ScreenplayOverviewViewModel.Init(screenplayOverviewView, screenplay, canGiveFeedback, this);

            screenplayInspectionView = (ScreenplayInspectionView)ScreenplayView.FindName("ScreenplayInspectionView");
            ScreenplayInspectionViewModel = (ScreenplayInspectionViewModel)screenplayInspectionView.DataContext;
            ScreenplayInspectionViewModel.Init(screenplayInspectionView, screenplay, this);
        }
    }
}