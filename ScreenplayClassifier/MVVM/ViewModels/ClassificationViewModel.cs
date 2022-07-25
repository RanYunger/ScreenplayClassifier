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
        public ClassificationView ClassificationView;
        public MainViewModel MainViewModel { get; private set; }

        public ObservableCollection<ScreenplayModel> BrowsedScreenplays { get; set; }
        public int SelectedScreenplay { get; set; }

        // Constructors
        public ClassificationViewModel()
        {
            BrowsedScreenplays = new ObservableCollection<ScreenplayModel>();
            SelectedScreenplay = -1;
        }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    DataGrid screenplaysDataGrid = (DataGrid)ClassificationView.FindName("ScreenplaysDataGrid");

                    if (Keyboard.IsKeyDown(Key.Back))
                        for (int i = 0; i < screenplaysDataGrid.Items.Count; i++)
                            if (screenplaysDataGrid.SelectedItems.Contains(screenplaysDataGrid.Items[i]))
                                BrowsedScreenplays.RemoveAt(i);
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
                    Button browseScreenplaysButton = (Button)ClassificationView.FindName("BrowseScreenplaysButton");

                    openFileDialog.Title = "Browse screenplays to classify";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|docx files (*.docx)|*.*";
                    openFileDialog.Multiselect = true;
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    openFileDialog.ShowDialog();

                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        if (BrowsedScreenplays.Count == 10)
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
                });
            }
        }
        public Command ClearScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    Button browseScreenplaysButton = (Button)ClassificationView.FindName("BrowseScreenplaysButton");

                    BrowsedScreenplays.Clear();

                    browseScreenplaysButton.IsEnabled = true;
                });
            }
        }
        public Command ClassifyScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    Button browseScreenplaysButton = (Button)ClassificationView.FindName("BrowseScreenplaysButton"),
                        clearScreenplaysButton = (Button)ClassificationView.FindName("ClearScreenplaysButton");

                    browseScreenplaysButton.IsEnabled = clearScreenplaysButton.IsEnabled = false;
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
