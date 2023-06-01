using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationOverviewViewModel : PropertyChangeNotifier
    {
        // Fields
        private ObservableCollection<ScreenplayModel> classifiedScreenplays;
        private string titleText, accuracyColor;
        private double accuracyPercent;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ClassificationOverviewView ClassificationOverviewView { get; private set; }

        public ObservableCollection<ScreenplayModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (classifiedScreenplays.Count > 0)
                    TitleText = string.Format("{0} Review", ClassifiedScreenplays.Count == 1 ? ClassifiedScreenplays[0].Title : "Batch");

                NotifyPropertyChange();
            }
        }

        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;

                NotifyPropertyChange();
            }
        }

        public string AccuracyColor
        {
            get { return accuracyColor; }
            set
            {
                accuracyColor = value;

                NotifyPropertyChange();
            }
        }

        public double AccuracyPercent
        {
            get { return accuracyPercent; }
            set
            {
                accuracyPercent = value;

                if ((accuracyPercent >= 0) && (AccuracyPercent < 33.3))
                    AccuracyColor = "Red";
                else if ((accuracyPercent >= 33.3) && (AccuracyPercent < 66.6))
                    AccuracyColor = "Orange";
                else if ((accuracyPercent >= 66.6) && (AccuracyPercent <= 100))
                    AccuracyColor = "Green";

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ClassificationOverviewViewModel() { }

        // Methods
        #region Commands
        public Command ShowInspectionViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ClassificationOverviewView == null)
                        return;

                    HideView();
                    ClassificationViewModel.ClassificationInspectionViewModel.ShowView();
                });
            }
        }

        public Command RerunClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.ClassificationComplete = false); }
        }

        public Command CompleteClassificationCommand
        {
            get
            {
                return new Command(() =>
                {
                    UpdateModules();
                    ClassificationViewModel.ClassificationComplete = true;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="classificationOverviewView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(ClassificationOverviewView classificationOverviewView, ClassificationViewModel classificationViewModel)
        {
            ClassificationOverviewView = classificationOverviewView;
            ClassificationViewModel = classificationViewModel;

            ClassifiedScreenplays = new ObservableCollection<ScreenplayModel>();
            TitleText = string.Empty;
            AccuracyColor = string.Empty;
            AccuracyPercent = 0.0;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ClassificationOverviewView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationOverviewView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ClassificationInspectionView inspectionView = ClassificationViewModel.ClassificationInspectionViewModel.ClassificationInspectionView;
            int correctClassifications;

            ClassifiedScreenplays = ClassificationViewModel.ClassificationFeedbackViewModel.FeedbackedScreenplays;

            correctClassifications = new List<ScreenplayModel>(ClassifiedScreenplays).FindAll(s => s.IsClassifiedCorrectly).Count;
            AccuracyPercent = double.Parse(((100.0 * correctClassifications) / ClassifiedScreenplays.Count).ToString("0.00"));

            ClassificationViewModel.ClassificationInspectionViewModel.Init(inspectionView, ClassificationViewModel);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationOverviewView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationOverviewView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Notifies other modules of the completed classification process.
        /// </summary>
        private void UpdateModules()
        {
            UserModel owner = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User;
            ScreenplaysSelectionViewModel screenplaysSelectionViewModel = ClassificationViewModel.ClassificationBrowseViewModel.ScreenplaysSelectionViewModel;
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            List<ReportModel> createdReports = new List<ReportModel>();
            List<SelectionEntryModel> removedEntries = new List<SelectionEntryModel>();
            FileModel screenplayFile;

            // Removes classified screenplays from selection options
            foreach (SelectionEntryModel selectionEntry in screenplaysSelectionViewModel.SelectionEntries)
                if (selectionEntry.IsChecked)
                    removedEntries.Add(selectionEntry);

            foreach (SelectionEntryModel selectionEntry in removedEntries)
                screenplaysSelectionViewModel.RemoveEntry(selectionEntry);

            // Creates a report for each classified screenplay
            foreach (ScreenplayModel classifiedScreenplay in ClassifiedScreenplays)
            {
                screenplayFile = new FileModel(classifiedScreenplay.Title, File.ReadAllText(classifiedScreenplay.FilePath));

                // Guests activities do not affect database
                if (owner.Role != UserModel.UserRole.GUEST)
                    classifiedScreenplay.FileId = MONGODB.AddScreenplay(screenplayFile);

                createdReports.Add(new ReportModel(owner, classifiedScreenplay));
                reportsViewModel.Reports.Add(createdReports[createdReports.Count - 1]);
            }

            // Guests activities do not affect database
            if (owner.Role != UserModel.UserRole.GUEST)
                MONGODB.AddReports(createdReports);

            reportsViewModel.Reports = reportsViewModel.Reports; // triggers PropertyChanged events
        }
    }
}