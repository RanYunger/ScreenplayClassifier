using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public class Configurations
    {
        // Fields
        private string connectionString, databaseName;

        // Properties

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        // Constructors
        public Configurations()
        {
            ConnectionString = string.Empty;
            DatabaseName = string.Empty;
        }

        public Configurations(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }
    }
}