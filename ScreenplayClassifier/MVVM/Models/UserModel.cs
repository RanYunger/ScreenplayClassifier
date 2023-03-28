using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        // Enums
        public enum UserRole { GUEST, MEMBER, ADMIN };

        // Fields
        private string username, password;
        private UserRole role;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public string Username
        {
            get { return username; }
            set
            {
                username = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        public UserRole Role
        {
            get { return role; }
            set
            {
                role = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Role"));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        // Constructors
        public UserModel()
        {
            Username = "You";//"Jim";
            Role = UserRole.GUEST;
            Password = string.Empty;
        }

        public UserModel(string username, UserRole role, string password)
        {
            Username = username;
            Role = role;
            Password = password;
        }

        // Methods
        #region Commands
        [IgnoreDataMember]
        public Command ChangeRoleCommand
        {
            get
            {
                return new Command(() =>
                {
                    UserRole newRole = Role == UserRole.ADMIN ? UserRole.MEMBER : UserRole.ADMIN;
                    bool changeConfirmation = MessageBox.ShowQuestion(string.Format("Are you sure you want to change {0}'s role?", Username));

                    // Changes the user's role (if confirmation is given)
                    if (changeConfirmation)
                    {
                        Role = newRole;
                        MessageBox.ShowInformation(string.Format("{0} has been changed to {1}.", Username, Role));
                    }
                });
            }
        }
        #endregion
    }
}
