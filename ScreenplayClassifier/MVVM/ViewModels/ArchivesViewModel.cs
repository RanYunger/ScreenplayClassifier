using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesViewModel : INotifyPropertyChanged
    {
        // Fields
        private Dictionary<string, ObservableCollection<ScreenplayModel>> archives;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ArchivesView ArchivesView { get; private set; }

        public Dictionary<string, ObservableCollection<ScreenplayModel>> Archives
        {
            get { return archives; }
            set
            {
                archives = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Archives"));
            }
        }

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ArchivesView archivesView, MainViewModel mainViewModel)
        {
            FolderView folderView = null;
            FolderViewModel folderViewModel = null;

            MainViewModel = mainViewModel;
            ArchivesView = archivesView;

            Archives = Storage.LoadArchives();

            foreach (string genreName in Archives.Keys)
            {
                folderView = (FolderView)ArchivesView.FindName(genreName + "FolderView");
                folderViewModel = (FolderViewModel)folderView.DataContext;
                folderViewModel.Init(genreName, Archives[genreName]);
            }
        }
    }
}
