using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ScreenplaysSelectionViewModel : INotifyPropertyChanged
    {
        // Fields
        private Predicate<object> titleFilter;

        private ObservableCollection<SelectionModel> classifiedScreenplays, checkedScreenplays;
        private int selectedScreenplay;
        private bool canInspect;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ScreenplaysSelectionView ScreenplaysSelectionView { get; private set; }

        public ObservableCollection<SelectionModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public ObservableCollection<SelectionModel> CheckedScreenplays
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
                selectedScreenplay = value;

                if (selectedScreenplay != -1)
                    CheckSelectionCommand.Execute(null);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanInspect
        {
            get { return canInspect; }
            set
            {
                canInspect = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanInspect"));
            }
        }

        // Constructors
        public ScreenplaysSelectionViewModel() { }

        // Methods
        #region Commands
        public Command EnterTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string usernameInput = string.Empty;

                    // Validation
                    if (ScreenplaysSelectionView == null)
                        return;

                    titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
                    usernameInput = titleTextBox.Text;

                    if (string.Equals(usernameInput, "Search screenplay by title"))
                    {
                        titleTextBox.Foreground = Brushes.Black;
                        titleTextBox.Text = string.Empty;
                    }
                });
            }
        }

        public Command LeaveTitleTextboxCommand
        {
            get
            {
                TextBox titleTextBox = null;

                return new Command(() =>
                {
                    string titleInput = string.Empty;

                    if (ScreenplaysSelectionView == null)
                        return;

                    titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
                    titleInput = titleTextBox.Text;

                    if (string.IsNullOrEmpty(titleInput))
                    {
                        titleTextBox.Foreground = Brushes.Gray;
                        titleTextBox.Text = "Search screenplay by title";
                    }
                });
            }
        }

        public Command SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    TextBox titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
                    ICollectionView screenplaysCollectionView = CollectionViewSource.GetDefaultView(ClassifiedScreenplays);
                    string titleInput = titleTextBox.Text;

                    // Updates and activates the filter
                    titleFilter = (o) =>
                    {
                        return (string.IsNullOrEmpty(titleInput.Trim())) || (string.Equals(titleInput, "Search screenplay by title"))
                            ? true : ((SelectionModel)o).ScreenplayFileName.Contains(titleInput);
                    };
                    screenplaysCollectionView.Filter = (o) => { return titleFilter.Invoke(o); };
                    screenplaysCollectionView.Refresh();
                });
            }
        }

        public Command CheckSelectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    SelectionModel chosenScreenplay = null;

                    // Validation
                    if ((SelectedScreenplay == -1) || (ClassifiedScreenplays.Count == 0))
                        return;

                    chosenScreenplay = ClassifiedScreenplays[SelectedScreenplay];
                    if (CheckedScreenplays.Contains(chosenScreenplay))
                    {
                        chosenScreenplay.IsChecked = false;
                        CheckedScreenplays.Remove(chosenScreenplay);
                    }
                    else
                    {
                        chosenScreenplay.IsChecked = true;
                        CheckedScreenplays.Add(chosenScreenplay);
                    }

                    CanInspect = CheckedScreenplays.Count > 0;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// <param name="screenplaySelectionView">The view to obtain controls from</param>
        /// </summary>
        public void Init(ScreenplaysSelectionView screenplaysSelectionView)
        {
            ScreenplaysSelectionView = screenplaysSelectionView;
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView(ObservableCollection<ReportModel> reports)
        {
            TextBox titleTextBox = null;

            ClassifiedScreenplays = new ObservableCollection<SelectionModel>();
            foreach (ReportModel report in reports)
                ClassifiedScreenplays.Add(new SelectionModel(report.Owner.Username, report.Screenplay.FilePath));

            CheckedScreenplays = new ObservableCollection<SelectionModel>();

            SelectedScreenplay = -1;
            CanInspect = false;

            titleTextBox = (TextBox)ScreenplaysSelectionView.FindName("TitleTextBox");
            titleTextBox.Foreground = Brushes.Gray;
            titleTextBox.Text = "Search screenplay by title";
        }
    }
}