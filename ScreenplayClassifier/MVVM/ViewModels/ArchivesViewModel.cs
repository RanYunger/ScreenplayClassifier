using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ArchivesViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<string> genres;
        private List<ScreenplayModel> screenplays;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ArchivesByGenreView ArchivesByGenreView { get; private set; }
        public ArchivesByPercentView ArchivesByPercentView { get; private set; }

        public ObservableCollection<string> Genres
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

                ((ArchivesByGenreViewModel)ArchivesByGenreView.DataContext).RefreshArchives(Genres, screenplays);
                ((ArchivesByPercentViewModel)ArchivesByPercentView.DataContext).RefreshArchives(screenplays);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplays"));
            }
        }

        #region Commands
        public Command StopMusicCommand
        {
            get
            {
                ArchivesByGenreViewModel archivesByGenreViewModel = null;

                return new Command(() =>
                {
                    // Validation
                    if (ArchivesView == null)
                        return;

                    archivesByGenreViewModel = (ArchivesByGenreViewModel)ArchivesByGenreView.DataContext;
                    archivesByGenreViewModel.StopMusicCommand.Execute(null);
                });
            }
        }
        #endregion

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="archivesView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        public void Init(ArchivesView archivesView, MainViewModel mainViewModel)
        {
            ArchivesView = archivesView;
            MainViewModel = mainViewModel;

            ArchivesByGenreView = (ArchivesByGenreView)ArchivesView.FindName("ArchivesByGenreView");
            ArchivesByPercentView = (ArchivesByPercentView)ArchivesView.FindName("ArchivesByPercentView");

            ((ArchivesByGenreViewModel)ArchivesByGenreView.DataContext).Init(ArchivesByGenreView);
            ((ArchivesByPercentViewModel)ArchivesByPercentView.DataContext).Init(ArchivesByPercentView);

            Genres = new ObservableCollection<string>(JSON.LoadedGenres);
            InitScreenplays();

            ((ArchivesByGenreViewModel)ArchivesByGenreView.DataContext).RefreshArchives(Genres, Screenplays);
        }

        /// <summary>
        /// Initiates the screenplays collection.
        /// </summary>
        private void InitScreenplays()
        {
            UserModel user = MainViewModel.UserToolbarViewModel.User;
            ReportsViewModel reportsViewModel = (ReportsViewModel)MainViewModel.ReportsView.DataContext;
            ObservableCollection<ReportModel> reports = reportsViewModel.Reports;

            Screenplays = new List<ScreenplayModel>();
            if (user.Role != UserModel.UserRole.GUEST)
            {
                foreach (ReportModel report in reports)
                    Screenplays.Add(report.Screenplay);

                Screenplays = Screenplays; // Triggers PropertyChanged event
            }
        }
    }
}