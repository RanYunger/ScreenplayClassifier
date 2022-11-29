﻿using Microsoft.Win32;
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
        private ObservableCollection<string> browsedScreenplaysTitles, browsedScreenplaysPaths;
        private ImageSource playImage;
        private int selectedScreenplay;
        private bool canBrowse, canClear, canChoose, canProceed;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ClassificationViewModel ClassificationViewModel { get; private set; }
        public BrowseView BrowseView { get; private set; }

        public ObservableCollection<string> BrowsedScreenplaysTitles
        {
            get { return browsedScreenplaysTitles; }
            set
            {
                browsedScreenplaysTitles = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowsedScreenplaysTitles"));
            }
        }

        public ObservableCollection<string> BrowsedScreenplaysPaths
        {
            get { return browsedScreenplaysPaths; }
            set
            {
                browsedScreenplaysPaths = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowsedScreenplaysPaths"));
            }
        }

        public ImageSource PlayImage
        {
            get { return playImage; }
            set
            {
                playImage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PlayImage"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;
                if (ClassificationViewModel != null)
                    ClassificationViewModel.SelectedScreenplay = selectedScreenplay;

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

                if (!canBrowse)
                    PlayImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PlayPressed.png"));

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

                if ((canChoose) && (!canBrowse))
                    PlayImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PlayUnpressed.png"));

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
                    ListView browsedScreenplaysListView = (ListView)BrowseView.FindName("BrowsedScreenplaysListView");

                    // Validation
                    if (!CanBrowse)
                        return;

                    if ((Keyboard.IsKeyDown(Key.Back)) || (Keyboard.IsKeyDown(Key.Delete)))
                        for (int i = 0; i < browsedScreenplaysListView.Items.Count; i++)
                            if (browsedScreenplaysListView.SelectedItems.Contains(browsedScreenplaysListView.Items[i]))
                            {
                                BrowsedScreenplaysTitles.RemoveAt(i);
                                browsedScreenplaysPaths.RemoveAt(i);
                            }

                    SelectedScreenplay = browsedScreenplaysTitles.Count > 0 ? 0 : SelectedScreenplay;
                    CanChoose = BrowsedScreenplaysTitles.Count > 0;
                    CanProceed = BrowsedScreenplaysTitles.Count > 0;
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
                    string screenplayPath = string.Empty;

                    openFileDialog.Title = "Browse screenplays to classify";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Multiselect = true;
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    openFileDialog.ShowDialog();

                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        BrowsedScreenplaysTitles.Add(Path.GetFileNameWithoutExtension(openFileDialog.FileNames[i]));
                        screenplayPath = string.Format("\"{0}\"", openFileDialog.FileNames[i]); // for passing paths as arguments
                        BrowsedScreenplaysPaths.Add(screenplayPath);
                    }

                    SelectedScreenplay = browsedScreenplaysTitles.Count > 0 ? 0 : SelectedScreenplay;
                    CanChoose = BrowsedScreenplaysTitles.Count > 0;
                    CanProceed = BrowsedScreenplaysTitles.Count > 0;
                });
            }
        }

        public Command ClearScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    BrowsedScreenplaysTitles.Clear();
                    browsedScreenplaysPaths.Clear();

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

        public void RefreshView()
        {
            BrowsedScreenplaysTitles = new ObservableCollection<string>();
            BrowsedScreenplaysPaths = new ObservableCollection<string>();
            SelectedScreenplay = -1;
            CanBrowse = true;
            CanClear = true;
            CanChoose = false;
            CanProceed = false;

            PlayImage = new BitmapImage(new Uri(FolderPaths.IMAGES + "PlayUnpressed.png"));

            if (BrowseView != null)
                App.Current.Dispatcher.Invoke(() => BrowseView.Visibility = Visibility.Visible);
        }

        public void HideView()
        {
            if (BrowseView != null)
                App.Current.Dispatcher.Invoke(() => BrowseView.Visibility = Visibility.Collapsed);
        }
    }
}
