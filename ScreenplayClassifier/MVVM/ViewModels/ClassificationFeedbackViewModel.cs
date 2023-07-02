using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationFeedbackViewModel : PropertyChangeNotifier
    {
        // Fields
        private ObservableCollection<ScreenplayModel> feedbackedScreenplays;
        private ObservableCollection<string> screenplayTitles;
        private int screenplayOffset;
        private string nextTitle, nextButtonText;

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

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<string> ScreenplayTitles
        {
            get { return screenplayTitles; }
            set
            {
                screenplayTitles = value;

                NotifyPropertyChange();
            }
        }

        public int ScreenplayOffset
        {
            get { return screenplayOffset; }
            set
            {
                screenplayOffset = value;

                NotifyPropertyChange();
            }
        }

        public string NextTitle
        {
            get { return nextTitle; }
            set
            {
                nextTitle = value;

                NotifyPropertyChange();
            }
        }

        public string NextButtonText
        {
            get { return nextButtonText; }
            set
            {
                nextButtonText = value;

                NotifyPropertyChange();
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

                    proceedConfirmation = MessageBox.ShowQuestion("Are you sure you want to proceed?" +
                        "\nOnce confirmed, your classification cannot be changed.", true);
                    if (proceedConfirmation)
                    {
                        screenplayView = (ScreenplayView)ClassificationFeedbackView.FindName("ScreenplayView");

                        ClassificationViewModel.FeedbackComplete = ++ScreenplayOffset == FeedbackedScreenplays.Count;
                        if (!ClassificationViewModel.FeedbackComplete)
                        {
                            if (ScreenplayOffset == ScreenplayTitles.Count - 1)
                            {
                                NextTitle = "-";
                                NextButtonText = "Finish";
                            }
                            else
                            {
                                NextTitle = ScreenplayTitles[ScreenplayOffset + 1];
                                NextButtonText = "Next Title";
                            }

                            ScreenplayViewModel.Init(screenplayView, FeedbackedScreenplays[ScreenplayOffset], true);
                        }
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

            NextButtonText = "Next Title";
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
            NextTitle = ScreenplayOffset == ScreenplayTitles.Count - 1 ? "-" : ScreenplayTitles[screenplayOffset + 1];
            NextButtonText = "Next Title";

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