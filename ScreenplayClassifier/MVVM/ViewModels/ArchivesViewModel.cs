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
        public ArchivesFilterViewModel ArchivesFilterViewModel { get; private set; }
        public ArchivesInspectionViewModel ArchivesInspectionViewModel { get; private set; }

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

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplays"));
            }
        }

        #region Commands
        public Command StopMusicCommand
        {
            get
            {
                return new Command(() =>
                {
                    // TODO: COMPLETE
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
            ArchivesFilterView archivesFilterView = null;
            ArchivesInspectionView archivesInspectionView = null;

            ArchivesView = archivesView;
            MainViewModel = mainViewModel;

            Genres = new ObservableCollection<string>(JSON.LoadedGenres);
            InitScreenplays();

            archivesFilterView = (ArchivesFilterView)ArchivesView.FindName("ArchivesFilterView");
            ArchivesFilterViewModel = (ArchivesFilterViewModel)archivesFilterView.DataContext;
            ArchivesFilterViewModel.Init(archivesFilterView, this);

            archivesInspectionView = (ArchivesInspectionView)ArchivesView.FindName("ArchivesInspectionView");
            ArchivesInspectionViewModel = (ArchivesInspectionViewModel)archivesInspectionView.DataContext;
            ArchivesInspectionViewModel.Init(archivesInspectionView, this);
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