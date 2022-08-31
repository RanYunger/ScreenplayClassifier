using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        // Fields
        private ClassificationModel classificationReport;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ReportView ReportView { get; set; }
        public GenresViewModel GenresViewModel { get; private set; }

        public ClassificationModel ClassificationReport
        {
            get { return classificationReport; }
            set
            {
                classificationReport = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassificationReport"));
            }
        }

        // Constructors
        public ReportViewModel() { }

        // Methods
        #region Commands
        #endregion

        public void Init(ClassificationModel classificationReport, ReportView reportView)
        {
            GenresView genresView;

            ClassificationReport = classificationReport;
            ReportView = reportView;

            genresView = (GenresView)ReportView.FindName("GenresView");
            GenresViewModel = (GenresViewModel)genresView.DataContext;
            GenresViewModel.Init(genresView);

            GenresViewModel.RefreshView(ClassificationReport.Screenplay, "Actual");
        }
    }
}
