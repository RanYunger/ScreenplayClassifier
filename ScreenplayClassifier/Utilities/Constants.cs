using ScreenplayClassifier.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public class Constants
    {
        // Properties
        #region MongoDB
        public string ServerConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ReportsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        #endregion

        #region Application
        public string UsernamePattern { get; set; }
        public string PasswordPattern { get; set; }
        public string DefaultPassword { get; set; }
        #endregion

        // Constructors
        public Constants(string connectionString, string databaseName, string reportsCollectionName, string usersCollectionName,
            string usernamePattern, string passwordPattern, string defaultPassword)
        {
            ServerConnectionString = connectionString;
            DatabaseName = databaseName;
            ReportsCollectionName = reportsCollectionName;
            UsersCollectionName = usersCollectionName;

            UsernamePattern = usernamePattern;
            PasswordPattern = passwordPattern;
            DefaultPassword = defaultPassword;
        }
    }
}