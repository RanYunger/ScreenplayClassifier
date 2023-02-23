using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class InspectionViewModel : INotifyPropertyChanged
    {
        // Fields

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public InspectionView InspectionView { get; private set; }

        // Constructors
        public InspectionViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Inits the model.
        /// </summary>
        /// <param name="inspectionView">The view to obtain controls from</param>
        /// <param name="screenplays">The screenplays to inspect</param>
        public void Init(InspectionView inspectionView, List<ScreenplayModel> screenplays)
        {
            InspectionView = inspectionView;
        }
    }
}
