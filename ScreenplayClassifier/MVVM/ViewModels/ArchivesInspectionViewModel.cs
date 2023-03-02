using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesInspectionView ArchivesInspectionView { get; private set; }

        // Constructors

        // Methods
        #region Commands
        public Command ShowFilterViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ArchivesInspectionView == null)
                        return;

                    HideView();
                    ArchivesViewModel.ArchivesFilterViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="archivesInspectionView">The view to obtain controls from</param>
        /// <param name="archivesViewModel">The view model which manages the archives module</param>
        public void Init(ArchivesInspectionView archivesInspectionView, ArchivesViewModel archivesViewModel)
        {
            ArchivesInspectionView = archivesInspectionView;
            ArchivesViewModel = archivesViewModel;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ArchivesInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ArchivesInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesInspectionView.Visibility = Visibility.Collapsed);
        }
    }
}