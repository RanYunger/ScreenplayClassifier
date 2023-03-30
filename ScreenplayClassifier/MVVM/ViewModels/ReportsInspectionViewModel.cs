using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportsInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> inspectedReports;
        private string parentView, screenplayOwner;
        private int selectedScreenplay;
        private bool canFlutter, canGoToPrevious, canGoToNext;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportsViewModel ReportsViewModel { get; private set; }
        public ReportsInspectionView ReportsInspectionView { get; private set; }

        public ObservableCollection<ReportModel> InspectedReports
        {
            get { return inspectedReports; }
            set
            {
                inspectedReports = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("InspectedReports"));
            }
        }

        public string ParentView
        {
            get { return parentView; }
            set
            {
                parentView = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ParentView"));
            }
        }

        public string ScreenplayOwner
        {
            get { return screenplayOwner; }
            set
            {
                screenplayOwner = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("screenplayOwner"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if (selectedScreenplay != -1)
                    RefreshScreenplayCommand.Execute(null);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanFlutter
        {
            get { return canFlutter; }
            set
            {
                canFlutter = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanFlutter"));
            }
        }

        public bool CanGoToPrevious
        {
            get { return canGoToPrevious; }
            set
            {
                canGoToPrevious = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToPrevious"));
            }
        }

        public bool CanGoToNext
        {
            get { return canGoToNext; }
            set
            {
                canGoToNext = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToNext"));
            }
        }

        // Constructors
        public ReportsInspectionViewModel() { }

        // Methods
        #region Commands
        public Command RefreshScreenplayCommand
        {
            get
            {
                ScreenplayView screenplayView = (ScreenplayView)ReportsInspectionView.FindName("ScreenplayView");

                return new Command(() =>
                {
                    if (InspectedReports.Count > 0)
                    {
                        ((ScreenplayViewModel)screenplayView.DataContext).Init(screenplayView,
                            InspectedReports[SelectedScreenplay].Screenplay, false);
                        ScreenplayOwner = InspectedReports[SelectedScreenplay].Owner.Username;
                    }
                });
            }
        }

        public Command GoToPreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedScreenplay = SelectedScreenplay - 1 <= 0 ? 0 : SelectedScreenplay - 1;

                    CanGoToPrevious = (InspectedReports.Count > 1) && (SelectedScreenplay > 0);
                    CanGoToNext = (InspectedReports.Count > 1) && (SelectedScreenplay < InspectedReports.Count - 1);
                });
            }
        }

        public Command ShowParentViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    MainViewModel mainViewModel = null;
                    ArchivesViewModel archivesViewModel = null;

                    // Validation
                    if (ReportsInspectionView == null)
                        return;

                    HideView();

                    if (ParentView == "Reports")
                    {
                        ReportsViewModel.ReportsSelectionViewModel.RefreshView();
                        ReportsViewModel.ReportsSelectionViewModel.ShowView();
                    }
                    else if (ParentView == "Archives")
                    {
                        mainViewModel = ReportsViewModel.MainViewModel;
                        archivesViewModel = (ArchivesViewModel)mainViewModel.ArchivesView.DataContext;

                        archivesViewModel.ArchivesFilterViewModel.HideView();
                        archivesViewModel.ArchivesInspectionViewModel.RefreshView();
                        archivesViewModel.ArchivesInspectionViewModel.ShowView();
                        mainViewModel.ShowView(mainViewModel.ArchivesView);
                    }
                });
            }
        }

        public Command GoToNextCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedScreenplay = SelectedScreenplay + 1 >= InspectedReports.Count - 1
                        ? InspectedReports.Count - 1 : SelectedScreenplay + 1;

                    CanGoToPrevious = (InspectedReports.Count > 1) && (SelectedScreenplay > 0);
                    CanGoToNext = (InspectedReports.Count > 1) && (SelectedScreenplay < InspectedReports.Count - 1);
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportsInspectionView">The view to obtain controls from</param>
        /// <param name="reportsViewModel">The view model who manages the reports module</param>
        public void Init(ReportsInspectionView reportsInspectionView, ReportsViewModel reportsViewModel)
        {
            ReportsInspectionView = reportsInspectionView;
            ReportsViewModel = reportsViewModel;

            InspectedReports = new ObservableCollection<ReportModel>();
            ScreenplayOwner = string.Empty;
            SelectedScreenplay = -1;

            CanFlutter = false;
            CanGoToPrevious = false;
            CanGoToNext = false;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ReportsInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ReportsInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// <param name="selectedReports">The reports selected for inspection</param>
        /// <param name="parent">The view from which the ReportsInspectionView was called</param>
        /// </summary>
        public void RefreshView(ObservableCollection<SelectionModel> selectedReports, string parent)
        {
            InspectedReports.Clear();
            foreach (SelectionModel selectedReport in selectedReports)
                if (selectedReport.IsChecked)
                    InspectedReports.Add(ReportsViewModel.FindReportByTitle(selectedReport.ScreenplayFileName));

            SelectedScreenplay = -1;
            ParentView = parent;
            CanFlutter = InspectedReports.Count > 1;

            GoToNextCommand.Execute(null);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ReportsInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ReportsInspectionView.Visibility = Visibility.Collapsed);
        }
    }
}