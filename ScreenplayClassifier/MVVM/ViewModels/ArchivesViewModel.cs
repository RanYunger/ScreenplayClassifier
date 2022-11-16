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
        private List<string> genres;
        private List<ScreenplayModel> screenplays;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ArchivesView ArchivesView { get; private set; }
        public ArchivesSelectionView ArchivesSelectionView { get; private set; }
        public ArchivesByGenreView ArchivesByGenreView { get; private set; }
        public ArchivesByPercentView ArchivesByPercentView { get; private set; }

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

                ((ArchivesByGenreViewModel)ArchivesByGenreView.DataContext).RefreshArchives(Genres, screenplays);
                ((ArchivesByPercentViewModel)ArchivesByPercentView.DataContext).RefreshArchives(screenplays);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplays"));
            }
        }

        // Constructors
        public ArchivesViewModel() { }

        // Methods
        public void Init(ArchivesView archivesView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ArchivesView = archivesView;

            ArchivesSelectionView = (ArchivesSelectionView)ArchivesView.FindName("ArchivesSelectionView");
            ArchivesByGenreView = (ArchivesByGenreView)ArchivesView.FindName("ArchivesByGenreView");
            ArchivesByPercentView = (ArchivesByPercentView)ArchivesView.FindName("ArchivesByPercentView");

            ((ArchivesSelectionViewModel)ArchivesSelectionView.DataContext).Init(ArchivesSelectionView, this);
            ((ArchivesByGenreViewModel)ArchivesByGenreView.DataContext).Init(ArchivesByGenreView, this);
            ((ArchivesByPercentViewModel)ArchivesByPercentView.DataContext).Init(ArchivesByPercentView, this);

            Genres = Storage.LoadGenres();
            InitScreenplays();

            ((ArchivesByGenreViewModel)ArchivesByGenreView.DataContext).RefreshArchives(Genres, Screenplays);
        }

        protected void InitScreenplays()
        {
            UserModel user = MainViewModel.UserToolbarViewModel.User;
            ReportsViewModel reportsViewModel = (ReportsViewModel)MainViewModel.ReportsView.DataContext;
            ObservableCollection<ClassificationModel> reports = reportsViewModel.Reports;

            Screenplays = new List<ScreenplayModel>();
            if (user.Role != UserModel.UserRole.GUEST)
            {
                foreach (ClassificationModel report in reports)
                    Screenplays.Add(report.Screenplay);

                Screenplays = Screenplays; // Triggers PropertyChanged event
            }
        }

        public void ShowView(UserControl viewToShow)
        {
            UserControl[] views = { ArchivesSelectionView, ArchivesByPercentView, ArchivesByGenreView };

            if (viewToShow.Visibility == Visibility.Visible)
            {
                viewToShow.Focus();
                return;
            }

            foreach (UserControl view in views)
                view.Visibility = view == viewToShow ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}