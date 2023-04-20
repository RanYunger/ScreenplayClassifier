using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationInspectionViewModel : PropertyChangeNotifier
    {
        // Fields
        private ObservableCollection<ScreenplayModel> classifiedScreenplays;
        private string titleText;
        private double accuracyPercent;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ClassificationInspectionView ClassificationInspectionView { get; private set; }

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

        public double AccuracyPercent
        {
            get { return accuracyPercent; }
            set
            {
                accuracyPercent = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ClassificationInspectionViewModel() { }

        // Methods
        #region Commands
        public Command ShowOverviewViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Validation
                    if (ClassificationInspectionView == null)
                        return;

                    HideView();
                    ClassificationViewModel.ClassificationOverviewViewModel.ShowView();
                });
            }
        }

        public Command RerunClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.ClassificationOverviewViewModel.RerunClassificationCommand.Execute(null)); }
        }

        public Command CompleteClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.ClassificationOverviewViewModel.CompleteClassificationCommand.Execute(null)); }
        }
        #endregion

        /// <summary>
        /// Inits the model.
        /// </summary>
        /// <param name="classificationInspectionView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(ClassificationInspectionView classificationInspectionView, ClassificationViewModel classificationViewModel)
        {
            ClassificationInspectionView = classificationInspectionView;
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
            if (ClassificationInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationInspectionView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ClassifiedScreenplays = ClassificationViewModel.ClassificationFeedbackViewModel.FeedbackedScreenplays;
            AccuracyPercent = ClassificationViewModel.ClassificationOverviewViewModel.AccuracyPercent;
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationInspectionView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationInspectionView.Visibility = Visibility.Collapsed);
        }
    }
}