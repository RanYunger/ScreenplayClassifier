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
    class ArchivesByGenreViewModel : INotifyPropertyChanged
    {
        // Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        private Dictionary<string, ObservableCollection<ScreenplayModel>> archives;
        public ArchivesViewModel ArchivesViewModel { get; private set; }
        public ArchivesByGenreView ArchivesByGenreView { get; private set; }

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
        public ArchivesByGenreViewModel() { }

        // Methods
        #region Commands
        public Command BackCommand
        {
            get { return new Command(() => ArchivesViewModel.ShowView(ArchivesViewModel.ArchivesSelectionView)); }
        }
        #endregion

        public void Init(ArchivesByGenreView archivesByGenreView, ArchivesViewModel archivesViewModel)
        {
            ArchivesViewModel = archivesViewModel;
            ArchivesByGenreView = archivesByGenreView;

            Archives = new Dictionary<string, ObservableCollection<ScreenplayModel>>();
        }

        public void RefreshArchives(List<string> genres, List<ScreenplayModel> screenplays)
        {
            List<ScreenplayModel> genreScreenplays;

            foreach (string genreName in genres)
            {
                genreScreenplays = screenplays.FindAll(screenplay => screenplay.ActualGenre == genreName);
                Archives[genreName] = new ObservableCollection<ScreenplayModel>(genreScreenplays);
            }

            RefreshFolderViews();
        }

        private void RefreshFolderViews()
        {
            FolderView folderView = null;
            FolderViewModel folderViewModel = null;

            foreach (string genreName in Archives.Keys)
            {
                folderView = (FolderView)ArchivesByGenreView.FindName(genreName + "FolderView");
                folderViewModel = (FolderViewModel)folderView.DataContext;
                folderViewModel.Init(genreName, Archives[genreName]);
            }
        }
    }
}
