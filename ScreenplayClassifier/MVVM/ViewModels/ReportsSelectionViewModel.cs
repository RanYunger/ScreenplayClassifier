﻿using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportsSelectionViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportsSelectionView ReportsSelectionView { get; private set; }
        public ReportsViewModel ReportsViewModel { get; private set; }
        public ScreenplaysSelectionViewModel ScreenplaysSelectionViewModel { get; private set; }

        // Constructors
        public ReportsSelectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ReportsSelectionView == null)
                        return;

                    HideView();
                    ReportsViewModel.ReportsInspectionViewModel.RefreshView(ScreenplaysSelectionViewModel.ClassifiedScreenplays);
                    ReportsViewModel.ReportsInspectionViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportsSelectionView">The view to obtain controls from</param>
        /// <param name="reportsViewModel">The view model which manages the reports module</param>
        public void Init(ReportsSelectionView reportsSelectionView, ReportsViewModel reportsViewModel)
        {
            ScreenplaysSelectionView screenplaysSelectionView = null;

            ReportsSelectionView = reportsSelectionView;
            screenplaysSelectionView = (ScreenplaysSelectionView)ReportsSelectionView.FindName("ScreenplaysSelectionView");
            ScreenplaysSelectionViewModel = (ScreenplaysSelectionViewModel)screenplaysSelectionView.DataContext;
            ScreenplaysSelectionViewModel.Init(screenplaysSelectionView);
            ReportsViewModel = reportsViewModel;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ReportsSelectionView != null)
                App.Current.Dispatcher.Invoke(() => ReportsSelectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView() { ScreenplaysSelectionViewModel.RefreshView(ReportsViewModel.Reports); }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ReportsSelectionView != null)
                App.Current.Dispatcher.Invoke(() => ReportsSelectionView.Visibility = Visibility.Collapsed);
        }
    }
}