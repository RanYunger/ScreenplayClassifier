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
    public class ReportModel : INotifyPropertyChanged
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
        public ReportModel(UserModel owner, ScreenplayModel screenplay)
        {
            Owner = owner;
            Screenplay = screenplay;
        }

        // Methods
        #region Commands
        #endregion
    }
}