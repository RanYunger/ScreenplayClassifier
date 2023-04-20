using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationBrowseViewModel : PropertyChangeNotifier
    {
        // Fields
        private ObservableCollection<SelectionEntryModel> browsedScreenplays, checkedScreenplays;
        private int selectedScreenplay;
        private bool canBrowse, canClear, canSelect, canClassify;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public ClassificationBrowseView ClassificationBrowseView { get; private set; }

        public ObservableCollection<SelectionEntryModel> BrowsedScreenplays
        {
            get { return browsedScreenplays; }
            set
            {
                browsedScreenplays = value;

                NotifyPropertyChange();
            }
        }

        public ObservableCollection<SelectionEntryModel> CheckedScreenplays
        {
            get { return checkedScreenplays; }
            set
            {
                checkedScreenplays = value;

                NotifyPropertyChange();
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if (ClassificationViewModel != null)
                    CheckSelectionCommand.Execute(null);

                NotifyPropertyChange();
            }
        }

        public bool CanBrowse
        {
            get { return canBrowse; }
            set
            {
                canBrowse = value;

                NotifyPropertyChange();
            }
        }

        public bool CanClear
        {
            get { return canClear; }
            set
            {
                canClear = value;

                NotifyPropertyChange();
            }
        }

        public bool CanSelect
        {
            get { return canSelect; }
            set
            {
                canSelect = value;

                NotifyPropertyChange();
            }
        }

        public bool CanClassify
        {
            get { return canClassify; }
            set
            {
                canClassify = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ClassificationBrowseViewModel() { }

        // Methods
        #region Commands
        public Command CheckSelectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectionEntryModel browsedScreenplay = null;

                    // Validation
                    if ((!CanBrowse) || (selectedScreenplay == -1) || (BrowsedScreenplays.Count == 0))
                        return;

                    browsedScreenplay = BrowsedScreenplays[selectedScreenplay];
                    if (CheckedScreenplays.Contains(browsedScreenplay))
                    {
                        browsedScreenplay.IsChecked = false;
                        CheckedScreenplays.Remove(browsedScreenplay);
                    }
                    else
                    {
                        browsedScreenplay.IsChecked = true;
                        CheckedScreenplays.Add(browsedScreenplay);
                    }

                    CanClassify = CheckedScreenplays.Count > 0;
                });
            }
        }

        public Command BrowseScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    string ownerName = ClassificationViewModel.MainViewModel.UserToolbarViewModel.User.Username, screenplayPath = string.Empty;
                    OpenFileDialog openFileDialog = new OpenFileDialog()
                    {
                        Title = "Screenplay Classifier",
                        DefaultExt = "txt",
                        Filter = "Text files (*.txt)|*.txt|Word Documents (*.docx)|*.docx|All files (*.*)|*.*",
                        Multiselect = true,
                        InitialDirectory = Environment.CurrentDirectory
                    };

                    openFileDialog.ShowDialog();

                    // Adds each browsed screenplay to collection
                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        screenplayPath = string.Format("\"{0}\"", openFileDialog.FileNames[i]); // for passing paths with spaces
                        BrowsedScreenplays.Add(new SelectionEntryModel(ownerName, screenplayPath));
                    }

                    CanClear = BrowsedScreenplays.Count > 0;
                    CanSelect = BrowsedScreenplays.Count > 0;
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
                    CanClear = false;
                    CanSelect = false;
                    CanClassify = false;
                });
            }
        }

        public Command ClassifyCommand
        {
            get
            {
                return new Command(() =>
                {
                    CanBrowse = false;
                    CanClear = false;
                    CanSelect = false;
                    CanClassify = false;

                    ClassificationViewModel.BrowseComplete = true;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="classificationBrowseView">The view to obtain controls from</param>
        /// <param name="classificationViewModel">The view model which manages the classification module</param>
        public void Init(ClassificationBrowseView classificationBrowseView, ClassificationViewModel classificationViewModel)
        {
            ClassificationBrowseView = classificationBrowseView;
            ClassificationViewModel = classificationViewModel;

            BrowsedScreenplays = new ObservableCollection<SelectionEntryModel>();
            CheckedScreenplays = new ObservableCollection<SelectionEntryModel>();

            RefreshView();
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (ClassificationBrowseView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationBrowseView.Visibility = System.Windows.Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            CheckedScreenplays.Clear();
            SelectedScreenplay = -1;
            CanBrowse = true;
            CanClear = BrowsedScreenplays.Count > 0;
            CanSelect = BrowsedScreenplays.Count > 0;
            CanClassify = CheckedScreenplays.Count > 0;
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (ClassificationBrowseView != null)
                App.Current.Dispatcher.Invoke(() => ClassificationBrowseView.Visibility = System.Windows.Visibility.Collapsed);
        }
    }
}