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

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportsInspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> inspectedReports;
        private string screenplayTitle, screenplayOwner;
        private int selectedScreenplay;
        private bool canGoToFirst, canGoToPrevious, canGoToNext, canGoToLast;

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

        public string ScreenplayTitle
        {
            get { return screenplayTitle; }
            set
            {
                screenplayTitle = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayTitle"));
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

        public bool CanGoToFirst
        {
            get { return canGoToFirst; }
            set
            {
                canGoToFirst = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToFirst"));
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

        public bool CanGoToLast
        {
            get { return canGoToLast; }
            set
            {
                canGoToLast = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanGoToLast"));
            }
        }

        // Constructors
        public ReportsInspectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowSelectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ReportsInspectionView == null)
                        return;

                    HideView();
                    ReportsViewModel.ReportsSelectionViewModel.ShowView();
                });
            }
        }

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
                        ScreenplayTitle = InspectedReports[SelectedScreenplay].Screenplay.Title;
                        ScreenplayOwner = InspectedReports[SelectedScreenplay].Owner.Username;
                    }
                });
            }
        }

        public Command GoToFirstCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedScreenplay = 0;

                    CanGoToFirst = false;
                    CanGoToPrevious = false;
                    CanGoToNext = InspectedReports.Count > 1;
                    CanGoToLast = InspectedReports.Count > 1;
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

                    if (SelectedScreenplay == 0)
                    {
                        CanGoToFirst = false;
                        CanGoToPrevious = false;
                    }
                    CanGoToNext = InspectedReports.Count > 1;
                    CanGoToLast = InspectedReports.Count > 1;
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

                    CanGoToFirst = InspectedReports.Count > 1;
                    CanGoToPrevious = InspectedReports.Count > 1;
                    if (SelectedScreenplay == InspectedReports.Count - 1)
                    {
                        CanGoToNext = false;
                        CanGoToLast = false;
                    }
                });
            }
        }

        public Command GoToLastCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectedScreenplay = InspectedReports.Count - 1;

                    CanGoToFirst = InspectedReports.Count > 1;
                    CanGoToPrevious = InspectedReports.Count > 1;
                    CanGoToNext = false;
                    CanGoToLast = false;
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
            ScreenplayTitle = string.Empty;
            ScreenplayOwner = string.Empty;
            SelectedScreenplay = -1;

            RefreshView();

            CanGoToFirst = false;
            CanGoToPrevious = false;
            CanGoToNext = true;
            CanGoToLast = true;
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
        /// </summary>
        public void RefreshView()
        {
            ObservableCollection<BrowseModel> classifiedScreenplays = ReportsViewModel.ReportsSelectionViewModel.ClassifiedScreenplays;

            InspectedReports.Clear();

            for (int i = 0; i < classifiedScreenplays.Count; i++)
                if (classifiedScreenplays[i].IsChecked)
                    InspectedReports.Add(ReportsViewModel.Reports[i]);

            GoToFirstCommand.Execute(null);
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