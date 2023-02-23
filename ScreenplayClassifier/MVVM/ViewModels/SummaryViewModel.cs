using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SummaryViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<ReportModel> classifiedScreenplays;
        private string titleText, classificationsText, genreClassificationsText, subGenre1ClassificationsText, subGenre2ClassificationsText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public SummaryView SummaryView { get; private set; }
        public ClassificationViewModel ClassificationViewModel { get; private set; }

        public ObservableCollection<ReportModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (classifiedScreenplays.Count > 0)
                    TitleText = string.Format("{0} Review", ClassifiedScreenplays.Count == 1 ? ClassifiedScreenplays[0].Screenplay.Title : "Batch");

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TitleText"));
            }
        }

        public string ClassificationsText
        {
            get { return classificationsText; }
            set
            {
                classificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationsText"));
            }
        }

        public string GenreClassificationsText
        {
            get { return genreClassificationsText; }
            set
            {
                genreClassificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GenreClassificationsText"));
            }
        }

        public string SubGenre1ClassificationsText
        {
            get { return subGenre1ClassificationsText; }
            set
            {
                subGenre1ClassificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre1ClassificationsText"));
            }
        }

        public string SubGenre2ClassificationsText
        {
            get { return subGenre2ClassificationsText; }
            set
            {
                subGenre2ClassificationsText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SubGenre2ClassificationsText"));
            }
        }

        // Constructors
        public SummaryViewModel() { }

        // Methods
        #region Commands
        public Command RepeatClassificationCommand
        {
            get { return new Command(() => ClassificationViewModel.ClassificationComplete = false); }
        }
        public Command CompleteClassificationCommand
        {
            get
            {
                return new Command(() =>
                {
                    UpdateModules();
                    ClassificationViewModel.ClassificationComplete = true;
                });
            }
        }
        #endregion

        public void Init(SummaryView summaryView, ClassificationViewModel classificationViewModel)
        {
            SummaryView = summaryView;
            ClassificationViewModel = classificationViewModel;

            ClassifiedScreenplays = new ObservableCollection<ReportModel>();
            TitleText = string.Empty;
            ClassificationsText = string.Empty;
            GenreClassificationsText = string.Empty;
            SubGenre1ClassificationsText = string.Empty;
            SubGenre2ClassificationsText = string.Empty;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void ShowView()
        {
            if (SummaryView != null)
                App.Current.Dispatcher.Invoke(() => SummaryView.Visibility = Visibility.Visible);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            List<ScreenplayModel> screenplays = new List<ScreenplayModel>();
            Predicate<ScreenplayModel>[] queries = { s => s.IsClassifiedCorrectly, s => s.ModelGenre == s.OwnerGenre,
                        s => s.ModelSubGenre1 == s.OwnerSubGenre1, s => s.ModelSubGenre2 == s.OwnerSubGenre2 };
            int correctClassifications;
            string formattedText;
            double percentage;

            ClassifiedScreenplays = ClassificationViewModel.FeedbackViewModel.FeedbackedScreenplays;
            foreach (ReportModel reportModel in ClassifiedScreenplays)
                screenplays.Add(reportModel.Screenplay);

            for (int i = 0; i < queries.Length; i++)
            {
                correctClassifications = screenplays.FindAll(queries[i]).Count;
                percentage = (100.0 * correctClassifications) / screenplays.Count;
                formattedText = string.Format("{0}/{1} ({2}%)", correctClassifications, screenplays.Count, percentage.ToString("0.00"));

                switch (i)
                {
                    case 0: ClassificationsText = formattedText; break;
                    case 1: GenreClassificationsText = formattedText; break;
                    case 2: SubGenre1ClassificationsText = formattedText; break;
                    case 3: SubGenre2ClassificationsText = formattedText; break;
                }
            }
        }

        /// <summary>
        /// Hides the view.
        /// </summary>
        public void HideView()
        {
            if (SummaryView != null)
                App.Current.Dispatcher.Invoke(() => SummaryView.Visibility = Visibility.Collapsed);
        }

        /// <summary>
        /// Notifies other modules of the completed classification process.
        /// </summary>
        private void UpdateModules()
        {
            BrowseViewModel browseViewModel = ClassificationViewModel.BrowseViewModel;
            ReportsViewModel reportsViewModel = (ReportsViewModel)ClassificationViewModel.MainViewModel.ReportsView.DataContext;
            ArchivesViewModel archivesViewModel = (ArchivesViewModel)ClassificationViewModel.MainViewModel.ArchivesView.DataContext;

            foreach (BrowseModel checkedScreenplay in browseViewModel.CheckedScreenplays)
                browseViewModel.BrowsedScreenplays.Remove(checkedScreenplay);

            foreach (ReportModel report in ClassifiedScreenplays)
            {
                reportsViewModel.Reports.Add(report);
                archivesViewModel.Screenplays.Add(report.Screenplay);
            }

            // Triggers PropertyChanged events
            reportsViewModel.Reports = reportsViewModel.Reports;
            archivesViewModel.Screenplays = archivesViewModel.Screenplays;
        }
    }
}