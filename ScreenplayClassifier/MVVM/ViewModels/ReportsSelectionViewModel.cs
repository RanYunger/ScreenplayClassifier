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
        private Predicate<object> titleFilter;

        private ObservableCollection<SelectionModel> classifiedScreenplays, checkedScreenplays;
        private int selectedScreenplay;
        private bool canInspect;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportsViewModel ReportsViewModel { get; private set; }
        public ReportsSelectionView ReportsSelectionView { get; private set; }

        public ObservableCollection<SelectionModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public ObservableCollection<SelectionModel> CheckedScreenplays
        {
            get { return checkedScreenplays; }
            set
            {
                checkedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedScreenplays"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if (ReportsViewModel != null)
                    CheckSelectionCommand.Execute(null);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanInspect
        {
            get { return canInspect; }
            set
            {
                canInspect = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanInspect"));
            }
        }

        // Constructors
        public ReportsSelectionViewModel() { }

        // Methods
        #region Commands
        public Command EnterTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string usernameInput = string.Empty;

                    // Validation
                    if (ReportsSelectionView == null)
                        return;

                    titleTextBox = (TextBox)ReportsSelectionView.FindName("TitleTextBox");
                    usernameInput = titleTextBox.Text;

                    if (string.Equals(usernameInput, "Search by name"))
                    {
                        titleTextBox.Foreground = Brushes.Black;
                        titleTextBox.Text = string.Empty;
                    }
                });
            }
        }

        public Command LeaveTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string titleInput = string.Empty;

                    if (ReportsSelectionView == null)
                        return;

                    titleTextBox = (TextBox)ReportsSelectionView.FindName("TitleTextBox");
                    titleInput = titleTextBox.Text;

                    if (string.IsNullOrEmpty(titleInput))
                    {
                        titleTextBox.Foreground = Brushes.Gray;
                        titleTextBox.Text = "Search by name";
                    }
                });
            }
        }

        public Command SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    TextBox titleTextBox = (TextBox)ReportsSelectionView.FindName("TitleTextBox");
                    ICollectionView screenplaysCollectionView = CollectionViewSource.GetDefaultView(ClassifiedScreenplays);
                    string titleInput = titleTextBox.Text;

                    // Updates and activates the filter
                    titleFilter = (o) =>
                    {
                        return (string.IsNullOrEmpty(titleInput.Trim())) || (string.Equals(titleInput, "Search by name"))
                            ? true : ((SelectionModel)o).ScreenplayFileName.Contains(titleInput);
                    };
                    screenplaysCollectionView.Filter = (o) => { return titleFilter.Invoke(o); };
                    screenplaysCollectionView.Refresh();
                });
            }
        }

        public Command CheckSelectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectionModel chosenScreenplay = null;

                    // Validation
                    if ((SelectedScreenplay == -1) || (ClassifiedScreenplays.Count == 0))
                        return;

                    chosenScreenplay = ClassifiedScreenplays[SelectedScreenplay];
                    if (CheckedScreenplays.Contains(chosenScreenplay))
                    {
                        chosenScreenplay.IsChecked = false;
                        CheckedScreenplays.Remove(chosenScreenplay);
                    }
                    else
                    {
                        chosenScreenplay.IsChecked = true;
                        CheckedScreenplays.Add(chosenScreenplay);
                    }

                    CanInspect = CheckedScreenplays.Count > 0;
                });
            }
        }

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
                    ReportsViewModel.ReportsInspectionViewModel.RefreshView(ClassifiedScreenplays);
                    ReportsViewModel.ReportsInspectionViewModel.ShowView();
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportsSelectionView">The view to obtain controls from</param>
        /// <param name="reportsViewModel">The view model who manages the reports module</param>
        public void Init(ReportsSelectionView reportsSelectionView, ReportsViewModel reportsViewModel)
        {
            TextBox titleTextBox = null;

            ReportsSelectionView = reportsSelectionView;
            ReportsViewModel = reportsViewModel;

            ClassifiedScreenplays = new ObservableCollection<SelectionModel>();
            CheckedScreenplays = new ObservableCollection<SelectionModel>();

            titleTextBox = (TextBox)ReportsSelectionView.FindName("TitleTextBox");
            titleTextBox.Foreground = Brushes.Gray;
            titleTextBox.Text = "Search by name";
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
        public void RefreshView()
        {
            ClassifiedScreenplays.Clear();
            foreach (ReportModel report in ReportsViewModel.Reports)
                ClassifiedScreenplays.Add(new SelectionModel(report.Owner.Username, report.Screenplay.FilePath));

            CheckedScreenplays.Clear();

            SelectedScreenplay = -1;
            CanInspect = false;
        }

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