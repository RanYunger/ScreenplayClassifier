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
        public Dictionary<string, int> GenresDictionary { get; private set; }

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        public void Init(Dictionary<string, int> genreNames)
        {
            MainView mainView = null;
            ArchivesView archivesView = null;
            FolderView folderView = null;
            FolderViewModel folderViewModel = null;

            App.Current.Dispatcher.Invoke(() => mainView = (MainView)App.Current.MainWindow);
            App.Current.Dispatcher.Invoke(() => archivesView = ((MainViewModel)mainView.DataContext).ArchivesView);

            GenresDictionary = genreNames;
            foreach (KeyValuePair<string, int> genreRecord in GenresDictionary)
            {
                App.Current.Dispatcher.Invoke(() => folderView = (FolderView)archivesView.FindName(genreRecord.Key + "FolderView"));
                App.Current.Dispatcher.Invoke(() => folderViewModel = (FolderViewModel)folderView.DataContext);
                App.Current.Dispatcher.Invoke(() => folderViewModel.Init(genreRecord.Key, genreRecord.Value));
            }
        }
    }
}
