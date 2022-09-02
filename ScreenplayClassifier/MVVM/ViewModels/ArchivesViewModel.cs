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
        private List<string> genres;
        private List<ScreenplayModel> screenplays;
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

        public List<string> Genres
        {
            get { return genres; }
            set
            {
                genres = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Genres"));
            }
        }

        public List<ScreenplayModel> Screenplays
        {
            get { return screenplays; }
            set
            {
                screenplays = value;

                RefreshArchives();

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplays"));
            }
        }

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ArchivesView archivesView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ArchivesView = archivesView;

            Archives = new Dictionary<string, ObservableCollection<ScreenplayModel>>();
            Genres = Storage.LoadGenres();
            Screenplays = Storage.LoadArchives();

            RefreshFolderViews();
        }

        public void RefreshArchives()
        {
            List<ScreenplayModel> genreScreenplays;

            foreach (string genreName in Genres)
            {
                genreScreenplays = Screenplays.FindAll(screenplay => screenplay.ActualGenre == genreName);
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
                folderView = (FolderView)ArchivesView.FindName(genreName + "FolderView");
                folderViewModel = (FolderViewModel)folderView.DataContext;
                folderViewModel.Init(genreName, Archives[genreName]);
            }
        }
    }
}
