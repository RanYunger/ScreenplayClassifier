using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ArchivesFilterViewModel ArchivesFilterViewModel { get; private set; }
        public ArchivesInspectionViewModel ArchivesInspectionViewModel { get; private set; }

        #region Commands
        public Command StopMusicCommand
        {
            get
            {
                return new Command(() =>
                {
                    // TODO: COMPLETE
                });
            }
        }
        #endregion

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="archivesView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(ArchivesView archivesView, MainViewModel mainViewModel)
        {
            ArchivesFilterView archivesFilterView = null;
            ArchivesInspectionView archivesInspectionView = null;

            ArchivesView = archivesView;
            MainViewModel = mainViewModel;

            archivesFilterView = (ArchivesFilterView)ArchivesView.FindName("ArchivesFilterView");
            ArchivesFilterViewModel = (ArchivesFilterViewModel)archivesFilterView.DataContext;
            ArchivesFilterViewModel.Init(archivesFilterView, this);

            archivesInspectionView = (ArchivesInspectionView)ArchivesView.FindName("ArchivesInspectionView");
            ArchivesInspectionViewModel = (ArchivesInspectionViewModel)archivesInspectionView.DataContext;
            ArchivesInspectionViewModel.Init(archivesInspectionView, this);
        }
    }
}