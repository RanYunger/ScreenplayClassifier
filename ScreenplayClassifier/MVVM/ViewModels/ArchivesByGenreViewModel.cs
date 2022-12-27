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
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="archivesByGenreView">The view to obtain controls from</param>
        public void Init(ArchivesByGenreView archivesByGenreView)
        {
            ArchivesByGenreView = archivesByGenreView;

            Archives = new Dictionary<string, ObservableCollection<ScreenplayModel>>();
        }

        /// <summary>
        /// Refreshes the archives
        /// </summary>
        /// <param name="genres">List of genre labels</param>
        /// <param name="screenplays">List of screenplays to be categorized into genres</param>
        public void RefreshArchives(ObservableCollection<string> genres, List<ScreenplayModel> screenplays)
        {
            List<ScreenplayModel> genreScreenplays;

            foreach (string genreName in genres)
            {
                genreScreenplays = screenplays.FindAll(screenplay => screenplay.ActualGenre == genreName);
                Archives[genreName] = new ObservableCollection<ScreenplayModel>(genreScreenplays);
            }

            RefreshTicketViews();
        }

        /// <summary>
        /// Refreshes the TicketViews.
        /// </summary>
        private void RefreshTicketViews()
        {
            TicketView ticketView = null;
            TicketViewModel ticketViewModel = null;

            foreach (string genreName in Archives.Keys)
            {
                ticketView = (TicketView)ArchivesByGenreView.FindName(genreName + "TicketView");
                ticketViewModel = (TicketViewModel)ticketView.DataContext;
                ticketViewModel.Init(genreName, Archives[genreName]);
            }
        }
    }
}
