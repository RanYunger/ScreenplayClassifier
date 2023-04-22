﻿using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationBrowseViewModel : PropertyChangeNotifier
    {
        // Fields

        // Properties
        public ClassificationBrowseView ClassificationBrowseView { get; private set; }
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ScreenplaysSelectionViewModel ScreenplaysSelectionViewModel { get; private set; }

        // Constructors
        public ClassificationBrowseViewModel() { }

        // Methods
        #region Commands
        public Command BrowseScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    string ownerName = string.Empty, screenplayPath = string.Empty;
                    ObservableCollection<SelectionEntryModel> selectionEntries = null;
                    OpenFileDialog openFileDialog;

                    // Validation
                    if (ClassificationBrowseView == null)
                        return;

                    ownerName = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User.Username;
                    selectionEntries = new ObservableCollection<SelectionEntryModel>();

                    openFileDialog = new OpenFileDialog()
                    {
                        Title = "Screenplay Classifier",
                        DefaultExt = "txt",
                        Filter = "Text files (*.txt)|*.txt|Word Documents (*.docx)|*.docx|All files (*.*)|*.*",
                        Multiselect = true,
                        InitialDirectory = Environment.CurrentDirectory
                    };
                    openFileDialog.ShowDialog();

                    // Adds a selection entry for each browsed screenplay
                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        screenplayPath = string.Format("\"{0}\"", openFileDialog.FileNames[i]); // for passing paths with spaces
                        selectionEntries.Add(new SelectionEntryModel(ownerName, screenplayPath));
                    }

                    ScreenplaysSelectionViewModel.RefreshView(selectionEntries, "classify", string.Empty);
                });
            }
        }

        public Command ClearScreenplaysCommand
        {
            get { return new Command(() => ScreenplaysSelectionViewModel.ClearEntries()); }
        }

        public Command ClassifyCommand
        {
            get { return new Command(() => ClassificationViewModel.BrowseComplete = true); }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationBrowseView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(ClassificationBrowseView classificationBrowseView, ClassificationViewModel classificationViewModel)
        {
            ScreenplaysSelectionView screenplaysSelectionView = null;

            ClassificationBrowseView = classificationBrowseView;
            ClassificationViewModel = classificationViewModel;

            screenplaysSelectionView = (ScreenplaysSelectionView)classificationBrowseView.FindName("ScreenplaysSelectionView");
            ScreenplaysSelectionViewModel = (ScreenplaysSelectionViewModel)screenplaysSelectionView.DataContext;
            ScreenplaysSelectionViewModel.Init(screenplaysSelectionView);
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ClassificationBrowseView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationBrowseView.Visibility = System.Windows.Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationBrowseView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationBrowseView.Visibility = System.Windows.Visibility.Collapsed);
        }
    }
}