using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private string screenplayText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportView ReportView { get; set; }
        public GenresViewModel GenresViewModel { get; private set; }

        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Screenplay"));
            }
        }

        public string ScreenplayText
        {
            get { return screenplayText; }
            set
            {
                screenplayText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("screenplayText"));
            }
        }

        // Constructors
        public ReportViewModel() { }

        // Methods
        #region Commands
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="reportView">The view to obtain controls from</param>
        /// <param name="report">The report to represent in the ReportView</param>
        /// <param name="canGiveFeedback">The indication whether the user can give feedback</param>
        public void Init(ReportView reportView, ScreenplayModel screenplay, bool canGiveFeedback)
        {
            GenresView genresView;

            ReportView = reportView;

            Screenplay = screenplay;
            ScreenplayText = File.ReadAllText(screenplay.FilePath);

            genresView = (GenresView)ReportView.FindName("GenresView");
            GenresViewModel = (GenresViewModel)genresView.DataContext;
            GenresViewModel.Init(genresView, Screenplay, canGiveFeedback);
        }
    }
}