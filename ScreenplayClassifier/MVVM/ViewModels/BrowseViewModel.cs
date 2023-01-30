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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class BrowseViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<BrowseModel> browsedScreenplays, checkedScreenplays;
        private int selectedScreenplay;
        private bool canBrowse, canClear, canChoose, canProceed;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public BrowseView BrowseView { get; private set; }

        public ObservableCollection<BrowseModel> BrowsedScreenplays
        {
            get { return browsedScreenplays; }
            set
            {
                browsedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowsedScreenplays"));
            }
        }

        public ObservableCollection<BrowseModel> CheckedScreenplays
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
                FeedbackViewModel feedbackViewModel = null;
                ScreenplayModel shownScreenplay = null;
                int relativeScreenplayOffset = -1;

                selectedScreenplay = value;

                if (ClassificationViewModel != null)
                {
                    feedbackViewModel = ClassificationViewModel.FeedbackViewModel;
                    if (feedbackViewModel.CheckedOffsets.Contains(selectedScreenplay))
                    {
                        relativeScreenplayOffset = feedbackViewModel.CheckedOffsets.IndexOf(selectedScreenplay);
                        shownScreenplay = ClassificationViewModel.ClassifiedScreenplays[relativeScreenplayOffset].Screenplay;
                        feedbackViewModel.RefreshView(shownScreenplay);
                    }
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanBrowse
        {
            get { return canBrowse; }
            set
            {
                canBrowse = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanBrowse"));
            }
        }

        public bool CanClear
        {
            get { return canClear; }
            set
            {
                canClear = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanClear"));
            }
        }

        public bool CanChoose
        {
            get { return canChoose; }
            set
            {
                canChoose = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanChoose"));
            }
        }

        public bool CanProceed
        {
            get { return canProceed; }
            set
            {
                canProceed = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanProceed"));
            }
        }

        // Constructors
        public BrowseViewModel() { RefreshView(); }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    DataGrid browsedScreenplaysDataGrid = (DataGrid)BrowseView.FindName("BrowsedScreenplaysDataGrid");

                    // Checks wether the user as activated deletion
                    if ((Keyboard.IsKeyDown(Key.Back)) || (Keyboard.IsKeyDown(Key.Delete)))
                        for (int i = 0; i < browsedScreenplaysDataGrid.Items.Count; i++)
                            if (browsedScreenplaysDataGrid.SelectedItems.Contains(browsedScreenplaysDataGrid.Items[i]))
                                BrowsedScreenplays.RemoveAt(i);

                    SelectedScreenplay = BrowsedScreenplays.Count > 0 ? 0 : SelectedScreenplay;
                    //CanBrowse = BrowsedScreenplays.Count < 5;
                    CanChoose = BrowsedScreenplays.Count > 0;
                    CanProceed = BrowsedScreenplays.Count > 0;
                });
            }
        }

        public Command BrowseScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    string screenplayPath = string.Empty;
                    OpenFileDialog openFileDialog = new OpenFileDialog()
                    {
                        Title = "Browse screenplays to classify",
                        DefaultExt = "txt",
                        Filter = "Text files (*.txt)|*.txt|Word Documents (*.docx)|*.docx|All files (*.*)|*.*",
                        Multiselect = true,
                        InitialDirectory = Environment.CurrentDirectory
                    };

                    openFileDialog.ShowDialog();

                    // Adds each browsed screenplay to collection
                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        //if (BrowsedScreenplays.Count == 5)
                        //    break;

                        screenplayPath = string.Format("\"{0}\"", openFileDialog.FileNames[i]); // for passing paths with spaces
                        BrowsedScreenplays.Add(new BrowseModel(screenplayPath));
                    }

                    SelectedScreenplay = BrowsedScreenplays.Count > 0 ? 0 : SelectedScreenplay;
                    //CanBrowse = BrowsedScreenplays.Count < 5;
                    CanChoose = BrowsedScreenplays.Count > 0;
                    CanProceed = BrowsedScreenplays.Count > 0;
                });
            }
        }

        public Command ClearScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    BrowsedScreenplays.Clear();
                    CheckedScreenplays.Clear();

                    SelectedScreenplay = -1;
                    CanBrowse = true;
                    CanChoose = false;
                    CanProceed = false;
                });
            }
        }

        public Command ProceedToClassificationCommand
        {
            get
            {
                return new Command(() =>
                {
                    CanBrowse = false;
                    CanClear = false;
                    CanChoose = false;
                    CanProceed = false;

                    CheckedScreenplays.Clear();
                    foreach (BrowseModel browsedScreenplay in BrowsedScreenplays)
                        if (browsedScreenplay.IsChecked)
                            CheckedScreenplays.Add(browsedScreenplay);

                    ClassificationViewModel.BrowseComplete = true;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        /// <param name="browseView">The view to obtain controls from.</param>
        public void Init(ClassificationViewModel classificationViewModel, BrowseView browseView)
        {
            ClassificationViewModel = classificationViewModel;
            BrowseView = browseView;
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            BrowsedScreenplays = new ObservableCollection<BrowseModel>();
            CheckedScreenplays = new ObservableCollection<BrowseModel>();
            SelectedScreenplay = -1;
            CanBrowse = true;
            CanClear = true;
            CanChoose = false;
            CanProceed = false;

            if (BrowseView != null)
                App.Current.Dispatcher.Invoke(() => BrowseView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (BrowseView != null)
                App.Current.Dispatcher.Invoke(() => BrowseView.Visibility = Visibility.Collapsed);
        }
    }
}