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
        private Dictionary<string, List<int>> concordance;
        private Dictionary<string, int> wordAppearances;
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

        public Dictionary<string, List<int>> Concordance
        {
            get { return concordance; }
            set
            {
                concordance = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Concordance"));
            }
        }
        public Dictionary<string, int> WordAppearances
        {
            get { return wordAppearances; }
            set
            {
                wordAppearances = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WordAppearances"));
            }
        }

        // Constructors
        public ClassificationModel(UserModel owner, ScreenplayModel screenplay,
            Dictionary<string, List<int>> concordance, Dictionary<string, int> wordAppearances)
        {
            Owner = owner;
            Screenplay = screenplay;
            Concordance = concordance;
            WordAppearances = wordAppearances;
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
                    ReportView reportView;
                    ReportViewModel reportViewModel;

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
                                view.Close();
                                break;
                            }
                        }

                    reportView = new ReportView();
                    ((ReportViewModel)reportView.DataContext).Init(this, reportView);

                    reportView.Show();
                });
            }
        }
        #endregion
    }
}
