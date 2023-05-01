using Newtonsoft.Json;
using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class JSON
    {
        // Constants
        public static string USERNAMEPATTERN = "[A-Z]{1}[a-z]+(.[A-Z]{1}[a-z]+){1,2}";  // E.G. Ran.Yunger, Shy.Ohev.Zion
        public static string PASSWORDPATTERN = "[A-Z]{2,3}[0-9]{5,6}";                  // E.G. RY120696, SHZ101098

        // Fields
        public static List<UserModel> LoadedUsers;
        public static List<string> LoadedGenres;
        public static List<ReportModel> LoadedReports;

        // Methods
        /// <summary>
        /// Loads the authenticated users saved in json file.
        /// </summary>
        public static void LoadUsers()
        {
            string usersJson = File.ReadAllText(FolderPaths.JSONS + "Users.json");

            LoadedUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersJson);
        }

        /// <summary>
        /// Loads the genre labels saved in json file.
        /// </summary>
        public static void LoadGenres()
        {
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");

            LoadedGenres = JsonConvert.DeserializeObject<List<string>>(genresJson);
        }

        /// <summary>
        /// Loads the reports saved in json file.
        /// </summary>
        public static void LoadReports()
        {
            string reportsJson = File.ReadAllText(FolderPaths.JSONS + "Reports.json");

            LoadedReports = JsonConvert.DeserializeObject<List<ReportModel>>(reportsJson);
        }

        /// <summary>
        /// Saves the authenticated users to json file.
        /// </summary>
        /// <param name="users">Authenticated users to be saved</param>
        public static void SaveUsers(ObservableCollection<UserModel> users)
        {
            LoadedUsers = new List<UserModel>(users); // For signing out (reloading the file is unnecessary)

            File.WriteAllText(FolderPaths.JSONS + "Users.json", JsonConvert.SerializeObject(LoadedUsers, Formatting.Indented));
        }

        /// <summary>
        /// Saves classification reports to json file.
        /// </summary>
        /// <param name="reports"></param>
        public static void SaveReports(ObservableCollection<ReportModel> reports)
        {
            // Prevents duplications
            foreach (ReportModel addedReport in reports)
                if (!LoadedReports.Contains(addedReport))
                    LoadedReports.Add(addedReport);

            File.WriteAllText(FolderPaths.JSONS + "Reports.json", JsonConvert.SerializeObject(LoadedReports, Formatting.Indented));
        }
    }
}