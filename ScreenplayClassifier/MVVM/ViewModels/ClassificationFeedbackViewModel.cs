using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationFeedbackViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ScreenplayModel> feedbackedScreenplays;
        private ObservableCollection<string> screenplayTitles;
        private int screenplayOffset;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ClassificationFeedbackView ClassificationFeedbackView { get; private set; }
        public ScreenplayViewModel ScreenplayViewModel { get; private set; }

        public ObservableCollection<ScreenplayModel> FeedbackedScreenplays
        {
            get { return feedbackedScreenplays; }
            set
            {
                feedbackedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("feedbackedScreenplays"));
            }
        }

        public ObservableCollection<string> ScreenplayTitles
        {
            get { return screenplayTitles; }
            set
            {
                screenplayTitles = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayTitles"));
            }
        }

        public int ScreenplayOffset
        {
            get { return screenplayOffset; }
            set
            {
                screenplayOffset = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayOffset"));
            }
        }

        // Constructors
        public ClassificationFeedbackViewModel() { }

        // Methods
        #region Commands
        public Command GoToNextCommand
        {
            get
            {
                ScreenplayView screenplayView = null;

                return new Command(() =>
                {
                    bool proceedConfirmation;

                    // Validation
                    if (ClassificationFeedbackView == null)
                        return;

                    if ((ScreenplayOffset >= 0) && (!FeedbackedScreenplays[ScreenplayOffset].Isfeedbacked))
                    {
                        MessageBox.ShowError("Complete feedback before proceeding");
                        return;
                    }

                    proceedConfirmation = MessageBox.ShowQuestion("Are you sure?\nOnce confirmed, your classification cannot be changed.");
                    if (proceedConfirmation)
                    {
                        screenplayView = (ScreenplayView)ClassificationFeedbackView.FindName("ScreenplayView");

                        ClassificationViewModel.FeedbackComplete = ++ScreenplayOffset == FeedbackedScreenplays.Count;
                        if (!ClassificationViewModel.FeedbackComplete)
                            ScreenplayViewModel.Init(screenplayView, FeedbackedScreenplays[ScreenplayOffset], true);
                    }
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model
        /// </summary>
        /// <param name="classificationFeedbackView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(ClassificationFeedbackView classificationFeedbackView, ClassificationViewModel classificationViewModel)
        {
            ScreenplayView screenplayView = null;

            ClassificationFeedbackView = classificationFeedbackView;
            ClassificationViewModel = classificationViewModel;

            FeedbackedScreenplays = new ObservableCollection<ScreenplayModel>();
            ScreenplayTitles = new ObservableCollection<string>();

            screenplayView = (ScreenplayView)ClassificationFeedbackView.FindName("ScreenplayView");
            ScreenplayViewModel = (ScreenplayViewModel)screenplayView.DataContext;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ClassificationFeedbackView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationFeedbackView.Visibility = System.Windows.Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            ScreenplayView screenplayView = (ScreenplayView)ClassificationFeedbackView.FindName("ScreenplayView");

            ScreenplayTitles.Clear();
            foreach (ScreenplayModel feedbackedScreenplay in FeedbackedScreenplays)
                ScreenplayTitles.Add(feedbackedScreenplay.Title);

            ScreenplayOffset = 0;

            ((ScreenplayViewModel)screenplayView.DataContext).Init(screenplayView, FeedbackedScreenplays[ScreenplayOffset], true);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationFeedbackView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationFeedbackView.Visibility = System.Windows.Visibility.Collapsed);
        }
    }
}