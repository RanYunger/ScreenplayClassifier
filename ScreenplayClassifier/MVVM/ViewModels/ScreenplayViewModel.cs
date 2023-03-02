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
    public class ScreenplayViewModel : INotifyPropertyChanged
    {
        // Fields
        private ScreenplayModel screenplay;
        private string screenplayContent;
        private bool isScreenplayContentVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ScreenplayView ScreenplayView { get; set; }
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

        public string ScreenplayContent
        {
            get { return screenplayContent; }
            set
            {
                screenplayContent = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayContent"));
            }
        }

        public bool IsScreenplayContentVisible
        {
            get { return isScreenplayContentVisible; }
            set
            {
                isScreenplayContentVisible = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsScreenplayContentVisible"));
            }
        }

        // Constructors
        public ScreenplayViewModel() { }

        // Methods
        #region Commands
        public Command ToggleContentVisibilityCommand
        {
            get { return new Command(() => IsScreenplayContentVisible = !IsScreenplayContentVisible); }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="screenplayView">The view to obtain controls from</param>
        /// <param name="report">The report to represent in the ReportView</param>
        /// <param name="canGiveFeedback">The indication whether the user can give feedback</param>
        public void Init(ScreenplayView screenplayView, ScreenplayModel screenplay, bool canGiveFeedback)
        {
            GenresView genresView;

            ScreenplayView = screenplayView;

            Screenplay = screenplay;
            ScreenplayContent = File.ReadAllText(screenplay.FilePath);
            IsScreenplayContentVisible = false;

            genresView = (GenresView)ScreenplayView.FindName("GenresView");
            GenresViewModel = (GenresViewModel)genresView.DataContext;
            GenresViewModel.Init(genresView, Screenplay, canGiveFeedback);
        }
    }
}