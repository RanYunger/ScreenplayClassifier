using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

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
            Username = "Jim";
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
                    Role = Role == UserModel.UserRole.ADMIN ? UserModel.UserRole.MEMBER : UserModel.UserRole.ADMIN; ;

                    MessageBoxHandler.Show(string.Format("{0} has been {1} to {2}", Username,
                        Role == UserModel.UserRole.ADMIN ? "promoted" : "demoted", Role), "Success", 3, MessageBoxImage.Information);
                });
            }
        }
        #endregion
    }
}
