using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesFilterViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesFilterView ArchivesFilterView { get; private set; }

        // Constructors

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ArchivesFilterView == null)
                        return;

                    HideView();
                    ArchivesViewModel.ArchivesInspectionViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="archivesFilterView">The view to obtain controls from</param>
        /// <param name="archivesViewModel">The view model which manages the archives module</param>
        public void Init(ArchivesFilterView archivesFilterView, ArchivesViewModel archivesViewModel)
        {
            ArchivesFilterView = archivesFilterView;
            ArchivesViewModel = archivesViewModel;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ArchivesFilterView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesFilterView.Visibility = Visibility.Visible);
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
            if (ArchivesFilterView != null)
                App.Current.Dispatcher.Invoke(() => ArchivesFilterView.Visibility = Visibility.Collapsed);
        }
    }
}