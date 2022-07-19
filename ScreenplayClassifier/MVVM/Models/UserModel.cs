using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class UserModel
    {
        public enum UserRole { GUEST, MEMBER, ADMIN };

        // Properties
        public string Username { get; private set; }
        public UserRole Role { get; private set; }
        public string Password
        { get; private set; }

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
    }
}
