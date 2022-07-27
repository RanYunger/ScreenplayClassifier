using ScreenplayClassifier.MVVM.Views;
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
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ArchivesView ArchivesView { get; private set; }

        public Dictionary<string, int> GenresDictionary { get; private set; }

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ArchivesView archivesView, MainViewModel mainViewModel, Dictionary<string, int> genreNames)
        {          
            FolderView folderView = null;
            FolderViewModel folderViewModel = null;

            MainViewModel = mainViewModel;
            ArchivesView = archivesView;

            GenresDictionary = genreNames;

            foreach (KeyValuePair<string, int> genreRecord in GenresDictionary)
            {
                folderView = (FolderView)ArchivesView.FindName(genreRecord.Key + "FolderView");
                folderViewModel = (FolderViewModel)folderView.DataContext;
                folderViewModel.Init(genreRecord.Key, genreRecord.Value);
            }
        }
    }
}
