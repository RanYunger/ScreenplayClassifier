using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private int selectedScreenplay;
        private bool browseComplete, progressComplete, classificationComplete;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        public BrowseViewModel BrowseViewModel { get; private set; }
        public ProgressViewModel ProgressViewModel { get; private set; }
        public FeedbackViewModel FeedbackViewModel { get; private set; }

        public ObservableCollection<ClassificationModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if ((selectedScreenplay != -1) && (ClassifiedScreenplays.Count > 0))
                    FeedbackViewModel.RefreshView(ClassifiedScreenplays[selectedScreenplay].Screenplay);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool BrowseComplete
        {
            get { return browseComplete; }
            set
            {
                browseComplete = value;

                if (browseComplete)
                    ProgressViewModel.ShowView(BrowseViewModel.BrowsedScreenplays);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowseComplete"));
            }
        }

        public bool ProgressComplete
        {
            get { return progressComplete; }
            set
            {
                progressComplete = value;

                if (progressComplete)
                {
                    ProgressViewModel.HideView();
                    FeedbackViewModel.ShowView();

                    BrowseViewModel.CanChoose = true;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProgressComplete"));
            }
        }

        public bool ClassificationComplete
        {
            get { return classificationComplete; }
            set
            {
                classificationComplete = value;

                if (classificationComplete)
                {
                    BrowseViewModel.HideView();
                    FeedbackViewModel.HideView();

                    PlayVideoCommand.Execute(null);
                }
                else if (FeedbackViewModel != null)
                {
                    BrowseComplete = false;
                    ProgressComplete = false;

                    FeedbackViewModel.HideView();
                    BrowseViewModel.RefreshView();
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationComplete"));
            }
        }

        // Constructors
        public ClassificationViewModel()
        {
            ClassifiedScreenplays = new ObservableCollection<ClassificationModel>();
            SelectedScreenplay = -1;
            BrowseComplete = false;
            ProgressComplete = false;
            ClassificationComplete = false;
        }

        // Methods
        #region Commands
        public Command PlayVideoCommand
        {
            get
            {
                return new Command(() =>
                {
                    Timer videoTimer = new Timer(21500);
                    MediaElement mediaElement = (MediaElement)ClassificationView.FindName("MediaElement");

                    mediaElement.Source = new Uri(FolderPaths.VIDEOS + "It's Over. Go Home.mp4");
                    mediaElement.Play();
              
                    videoTimer.Elapsed += (sender, e) => VideoTimer_Elapsed(sender, e, videoTimer, mediaElement);
                    videoTimer.Start();
                });
            }
        }

        private void VideoTimer_Elapsed(object sender, ElapsedEventArgs e, Timer videoTimer, MediaElement mediaElement)
        {
            App.Current.Dispatcher.Invoke(() => videoTimer.Stop());
            App.Current.Dispatcher.Invoke(() => mediaElement.Visibility = Visibility.Collapsed);
            App.Current.Dispatcher.Invoke(() => ((UserToolbarViewModel)MainViewModel.UserToolbarView.DataContext).HomeCommand.Execute(null));
        }
        #endregion

        public void Init(ClassificationView classificationView, MainViewModel mainViewModel)
        {
            BrowseView browseView;
            ProgressView progressView;
            FeedbackView feedbackView;

            MainViewModel = mainViewModel;
            ClassificationView = classificationView;

            browseView = (BrowseView)ClassificationView.FindName("BrowseView");
            BrowseViewModel = (BrowseViewModel)browseView.DataContext;
            BrowseViewModel.Init(this, browseView);

            progressView = (ProgressView)ClassificationView.FindName("ProgressView");
            ProgressViewModel = (ProgressViewModel)progressView.DataContext;
            ProgressViewModel.Init(this, progressView);

            feedbackView = (FeedbackView)ClassificationView.FindName("FeedbackView");
            FeedbackViewModel = (FeedbackViewModel)feedbackView.DataContext;
            FeedbackViewModel.Init(this, feedbackView);
        }
    }
}
