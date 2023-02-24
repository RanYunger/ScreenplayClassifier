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
    public class InspectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ScreenplayModel> classifiedScreenplays;
        private string titleText;
        private double accuracyPercent;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public InspectionView InspectionView { get; private set; }
        public ClassificationViewModel ClassificationViewModel { get; private set; }

        public ObservableCollection<ScreenplayModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (classifiedScreenplays.Count > 0)
                    TitleText = string.Format("Inspect {0}", ClassifiedScreenplays.Count == 1 ? ClassifiedScreenplays[0].Title : "Batch");

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TitleText"));
            }
        }

        public double AccuracyPercent
        {
            get { return accuracyPercent; }
            set
            {
                accuracyPercent = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AccuracyPercent"));
            }
        }

        // Constructors
        public InspectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowOverviewViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (InspectionView == null)
                        return;

                    HideView();
                    ClassificationViewModel.OverviewViewModel.ShowView();
                });
            }
        }

        public Command RerunClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.OverviewViewModel.RerunClassificationCommand.Execute(null)); }
        }

        public Command CompleteClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.OverviewViewModel.CompleteClassificationCommand.Execute(null)); }
        }
        #endregion

        /// <summary>
        /// Inits the model.
        /// </summary>
        /// <param name="inspectionView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(InspectionView inspectionView, ClassificationViewModel classificationViewModel)
        {
            InspectionView = inspectionView;
            ClassificationViewModel = classificationViewModel;

            ClassifiedScreenplays = new ObservableCollection<ScreenplayModel>();
            TitleText = string.Empty;
            AccuracyPercent = 0.0;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (InspectionView != null)
                App.Current.Dispatcher.Invoke(() => InspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            int correctClassifications;

            ClassifiedScreenplays = ClassificationViewModel.FeedbackViewModel.FeedbackedScreenplays;

            correctClassifications = new List<ScreenplayModel>(ClassifiedScreenplays).FindAll(s => s.IsClassifiedCorrectly).Count;
            AccuracyPercent = (100.0 * correctClassifications) / ClassifiedScreenplays.Count;
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (InspectionView != null)
                App.Current.Dispatcher.Invoke(() => InspectionView.Visibility = Visibility.Collapsed);
        }
    }
}
