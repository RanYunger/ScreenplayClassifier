using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class BrowseViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ScreenplayModel> browsedScreenplays;
        private int selectedBrowsedScreenplay;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public BrowseView BrowseView { get; private set; }

        public ObservableCollection<ScreenplayModel> BrowsedScreenplays
        {
            get { return browsedScreenplays; }
            set
            {
                browsedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowsedScreenplays"));
            }
        }

        public int SelectedBrowsedScreenplay
        {
            get { return selectedBrowsedScreenplay; }
            set
            {
                selectedBrowsedScreenplay = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedBrowsedScreenplay"));
            }
        }

        // Constructors
        public BrowseViewModel()
        {
            BrowsedScreenplays = new ObservableCollection<ScreenplayModel>();
            SelectedBrowsedScreenplay = -1;
        }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    DataGrid browsedScreenplaysDataGrid = (DataGrid)BrowseView.FindName("BrowsedScreenplaysDataGrid");
                    Button proceedToClassificationButton = (Button)BrowseView.FindName("ProceedToClassificationButton");

                    if (Keyboard.IsKeyDown(Key.Back))
                        for (int i = 0; i < browsedScreenplaysDataGrid.Items.Count; i++)
                            if (browsedScreenplaysDataGrid.SelectedItems.Contains(browsedScreenplaysDataGrid.Items[i]))
                                BrowsedScreenplays.RemoveAt(i);

                    proceedToClassificationButton.IsEnabled = BrowsedScreenplays.Count > 0;
                });
            }
        }
        public Command BrowseScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    string screenplayName = string.Empty;
                    Button browseScreenplaysButton = (Button)BrowseView.FindName("BrowseScreenplaysButton"),
                        proceedToClassificationButton = (Button)BrowseView.FindName("ProceedToClassificationButton");

                    openFileDialog.Title = "Browse screenplays to classify";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|docx files (*.docx)|*.*";
                    openFileDialog.Multiselect = true;
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    openFileDialog.ShowDialog();

                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        if (BrowsedScreenplays.Count == 20)
                        {
                            browseScreenplaysButton.IsEnabled = false;
                            break;
                        }
                        else
                        {
                            screenplayName = Path.GetFileNameWithoutExtension(openFileDialog.FileNames[i]);
                            BrowsedScreenplays.Add(new ScreenplayModel(screenplayName, openFileDialog.FileNames[i]));
                        }
                    }

                    proceedToClassificationButton.IsEnabled = BrowsedScreenplays.Count > 0;
                });
            }
        }
        public Command ClearScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    Button browseScreenplaysButton = (Button)BrowseView.FindName("BrowseScreenplaysButton"),
                        proceedToClassificationButton = (Button)BrowseView.FindName("ProceedToClassificationButton");

                    BrowsedScreenplays.Clear();

                    browseScreenplaysButton.IsEnabled = true;
                    proceedToClassificationButton.IsEnabled = false;
                });
            }
        }
        public Command ProceedToClassificationCommand
        {
            get
            {
                return new Command(() =>
                {
                    DataGrid browsedScreenplaysDataGrid = (DataGrid)BrowseView.FindName("BrowsedScreenplaysDataGrid");
                    Button browseScreenplaysButton = (Button)BrowseView.FindName("BrowseScreenplaysButton"),
                        clearScreenplaysButton = (Button)BrowseView.FindName("ClearScreenplaysButton"),
                        proceedToClassificationButton = (Button)BrowseView.FindName("ProceedToClassificationButton");

                    browsedScreenplaysDataGrid.IsEnabled = false;
                    browseScreenplaysButton.IsEnabled = clearScreenplaysButton.IsEnabled = proceedToClassificationButton.IsEnabled = false;

                    ClassificationViewModel.BrowseComplete = true;
                });
            }
        }
        #endregion

        public void Init(ClassificationViewModel classificationViewModel, BrowseView browseView)
        {
            ClassificationViewModel = classificationViewModel;
            BrowseView = browseView;
        }
    }
}
