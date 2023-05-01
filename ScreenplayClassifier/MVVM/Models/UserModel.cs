using MongoDB.Bson;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class UserModel : PropertyChangeNotifier
    {
        // Enums
        public enum UserRole { GUEST, MEMBER, ADMIN };

        // Fields
        private string username, password;
        private UserRole role;

        // Properties
        //public ObjectId Id { get; set; }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;

                NotifyPropertyChange();
            }
        }

        public UserRole Role
        {
            get { return role; }
            set
            {
                role = value;

                NotifyPropertyChange();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public UserModel()
        {
            Username = "You";
            Role = UserRole.GUEST;
            Password = string.Empty;
        }

        public UserModel(string username, UserRole role, string password)
        {
            Username = username;
            Role = role;
            Password = password;
        }
    }
}