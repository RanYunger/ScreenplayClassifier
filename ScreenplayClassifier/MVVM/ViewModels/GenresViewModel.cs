using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class GenresViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public GenresView GenresView { get; private set; }
        public GenresOverviewViewModel GenresOverviewViewModel { get; private set; }
        public GenresInspectionViewModel GenresInspectionViewModel { get; private set; }

        // Constructors
        public GenresViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Inits the model.
        /// </summary>
        /// <param name="genresView">The view to obtain controls from</param>
        /// <param name="screenplay">The screenplay to show the genres of</param>
        /// <param name="canGiveFeedback">The indication whether the user can give feedback</param>
        public void Init(GenresView genresView, ScreenplayModel screenplay, bool canGiveFeedback)
        {
            GenresOverviewView genresOverviewView = null;
            GenresInspectionView genresInspectionView = null;

            GenresView = genresView;

            genresOverviewView = (GenresOverviewView)GenresView.FindName("GenresOverviewView");
            GenresOverviewViewModel = (GenresOverviewViewModel)genresOverviewView.DataContext;
            GenresOverviewViewModel.Init(genresOverviewView, screenplay, canGiveFeedback, this);

            genresInspectionView = (GenresInspectionView)GenresView.FindName("GenresInspectionView");
            GenresInspectionViewModel = (GenresInspectionViewModel)genresInspectionView.DataContext;
            GenresInspectionViewModel.Init(genresInspectionView, screenplay, this);
        }
    }
}