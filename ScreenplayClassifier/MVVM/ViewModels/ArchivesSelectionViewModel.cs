using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesSelectionViewModel
    {
        // Fields

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }

        // Constructors
        public ArchivesSelectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowArchivesByPercentCommand
        {
            get { return new Command(() => ArchivesViewModel.ShowView(ArchivesViewModel.ArchivesByPercentView)); }
        }

        public Command ShowArchivesByGenreCommand
        {
            get { return new Command(() => ArchivesViewModel.ShowView(ArchivesViewModel.ArchivesByGenreView)); }
        }
        #endregion

        public void Init(ArchivesViewModel archivesViewModel)
        {
            ArchivesViewModel = archivesViewModel;
        }
    }
}
