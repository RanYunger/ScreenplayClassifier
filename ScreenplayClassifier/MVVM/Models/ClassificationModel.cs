using ScreenplayClassifier.MVVM.ViewModels;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ClassificationModel : INotifyPropertyChanged
    {
        // Fields
        private UserModel owner;
        private ScreenplayModel screenplay;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties

        public UserModel Owner
        {
            get { return owner; }
            set
            {
                owner = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Owner"));
            }
        }

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

        // Constructors
        public ClassificationModel(UserModel owner, ScreenplayModel screenplay)
        {
            Owner = owner;
            Screenplay = screenplay;
        }

        // Methods
        #region Commands
        [IgnoreDataMember]
        public Command ShowReportViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    ReportView reportView = null;
                    ReportViewModel reportViewModel = null;

                    // Finds an existing ReportView (if there's one)
                    foreach (Window view in App.Current.Windows)
                        if (view is ReportView)
                        {
                            reportViewModel = (ReportViewModel)view.DataContext;

                            if (reportViewModel.ClassificationReport.Equals(this))
                            {
                                view.Focus();
                                return;
                            }
                            else
                            {
                                // Closes the child ScreenplayView (if there's one)
                                foreach (Window childView in App.Current.Windows)
                                    if (childView is ScreenplayView)
                                    {
                                        childView.Close();
                                        break;
                                    }

                                view.Close();
                                break;
                            }
                        }

                    // Shows the report in a new ReportView
                    reportView = new ReportView();
                    ((ReportViewModel)reportView.DataContext).Init(this, reportView);

                    reportView.Show();
                });
            }
        }
        #endregion
    }
}