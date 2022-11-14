using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesByPercentViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesByPercentView ArchivesByPercentView { get; private set; }

        // Constructors
        public ArchivesByPercentViewModel() { }

        // Methods
        #region Commands
        public Command BackCommand
        {
            get { return new Command(() => ArchivesViewModel.ShowView(ArchivesViewModel.ArchivesSelectionView)); }
        }
        #endregion

        public void Init(ArchivesByPercentView archivesByPercentView, ArchivesViewModel archivesViewModel)
        {
            ArchivesViewModel = archivesViewModel;
            ArchivesByPercentView = archivesByPercentView;
        }
    }
}
