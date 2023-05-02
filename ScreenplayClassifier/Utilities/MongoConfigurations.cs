using ScreenplayClassifier.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public class MongoConfigurations
    {
        // Properties
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string GenresCollectionName { get; set; }
        public string ReportsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }


        // Constructors
        public MongoConfigurations(string connectionString, string databaseName,
            string genresCollectionName, string reportsCollectionName, string usersCollectionName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;

            GenresCollectionName = genresCollectionName;
            ReportsCollectionName = reportsCollectionName;
            UsersCollectionName = usersCollectionName;
        }
    }
}