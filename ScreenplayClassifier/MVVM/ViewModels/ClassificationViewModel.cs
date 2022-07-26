using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationViewModel
    {
        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }

        public ObservableCollection<ScreenplayModel> BrowsedScreenplays { get; set; }
        public int SelectedBrowsedScreenplay { get; set; }

        public ObservableCollection<ScreenplayModel> ClassifiedScreenplays { get; set; }
        public int SelectedClassifiedScreenplay { get; set; }

        // Constructors
        public ClassificationViewModel()
        {
            BrowsedScreenplays = new ObservableCollection<ScreenplayModel>();
            SelectedBrowsedScreenplay = -1;

            ClassifiedScreenplays = new ObservableCollection<ScreenplayModel>();
            SelectedClassifiedScreenplay = -1;
        }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    DataGrid browsedScreenplaysDataGrid = (DataGrid)ClassificationView.FindName("BrowsedScreenplaysDataGrid");
                    Button proceedToClassificationButton = (Button)ClassificationView.FindName("ProceedToClassificationButton");

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
                    Button browseScreenplaysButton = (Button)ClassificationView.FindName("BrowseScreenplaysButton"),
                        proceedToClassificationButton = (Button)ClassificationView.FindName("ProceedToClassificationButton");

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
                    Button browseScreenplaysButton = (Button)ClassificationView.FindName("BrowseScreenplaysButton"),
                        proceedToClassificationButton = (Button)ClassificationView.FindName("ProceedToClassificationButton");

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
                    Button browseScreenplaysButton = (Button)ClassificationView.FindName("BrowseScreenplaysButton"),
                        clearScreenplaysButton = (Button)ClassificationView.FindName("ClearScreenplaysButton"),
                        proceedToClassificationButton = (Button)ClassificationView.FindName("ProceedToClassificationButton");

                    foreach (ScreenplayModel screenplay in BrowsedScreenplays)
                        ClassifiedScreenplays.Add(screenplay);

                    browseScreenplaysButton.IsEnabled = clearScreenplaysButton.IsEnabled = proceedToClassificationButton.IsEnabled = false;
                });
            }
        }
        #endregion

        public void Init(ClassificationView classificationView, MainViewModel mainViewModel)
        {
            ClassificationView = classificationView;
            MainViewModel = mainViewModel;
        }
    }
}
