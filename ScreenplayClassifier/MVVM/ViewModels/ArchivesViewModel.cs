using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesViewModel
    {
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

            ArchivesView = archivesView;
            MainViewModel = mainViewModel;
            GenresDictionary = genreNames;

            foreach (KeyValuePair<string, int> genreRecord in GenresDictionary)
            {
                App.Current.Dispatcher.Invoke(() => folderView = (FolderView)ArchivesView.FindName(genreRecord.Key + "FolderView"));
                App.Current.Dispatcher.Invoke(() => folderViewModel = (FolderViewModel)folderView.DataContext);
                App.Current.Dispatcher.Invoke(() => folderViewModel.Init(folderView, genreRecord.Key, genreRecord.Value));
            }
        }
    }
}
