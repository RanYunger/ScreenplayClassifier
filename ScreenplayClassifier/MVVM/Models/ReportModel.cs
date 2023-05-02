using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ReportModel : PropertyChangeNotifier
    {
        // Fields
        private UserModel owner;
        private ScreenplayModel screenplay;

        // Properties
        public ObjectId Id { get; set; }

        public UserModel Owner
        {
            get { return owner; }
            set
            {
                owner = value;

                NotifyPropertyChange();
            }
        }

        public ScreenplayModel Screenplay
        {
            get { return screenplay; }
            set
            {
                screenplay = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public ReportModel(UserModel owner, ScreenplayModel screenplay)
        {
            Owner = owner;
            Screenplay = screenplay;
        }
    }
}