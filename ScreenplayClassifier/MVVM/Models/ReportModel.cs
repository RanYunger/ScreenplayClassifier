using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ReportModel : PropertyChangeNotifier
    {
        // Fields
        private ObjectId ownerId;
        [BsonIgnore]
        private string ownerName = null;
        private ScreenplayModel screenplay;

        // Properties
        public ObjectId Id { get; set; }

        public ObjectId OwnerId
        {
            get { return ownerId; }
            set
            {
                ownerId = value;

                NotifyPropertyChange();
            }
        }

        public string OwnerName
        {
            get
            {
                // title initialization from the associated screenplay 
                if (ownerName == null)
                    ownerName = MONGODB.Users.First(file => file.Id.Equals(ownerId)).Username;

                return ownerName;
            }

            set
            {
                ownerName = value;

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
            OwnerId = owner.Id;
            OwnerName = owner.Username;
            Screenplay = screenplay;
        }

        public ReportModel(ObjectId ownerId, string ownerName, ScreenplayModel screenplay)
        {
            OwnerId = ownerId;
            OwnerName = ownerName;
            Screenplay = screenplay;
        }
    }
}