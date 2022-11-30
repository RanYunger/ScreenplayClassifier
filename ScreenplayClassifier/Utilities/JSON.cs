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
        public static string USERNAMEPATTERN = "([A-Z]{1}[a-z]+){2,3}"; // E.G. RanYunger, ShyOhevZion
        public static string PASSWORDPATTERN = "[A-Z]{2,3}[0-9]{5,6}"; // E.G. RY120696, SHZ101098

        // Fields
        public static List<UserModel> LoadedUsers;
        public static List<string> LoadedGenres;
        public static List<ClassificationModel> LoadedReports;

        // Methods
        public static void LoadUsers()
        {
            string usersJson = File.ReadAllText(FolderPaths.JSONS + "Users.json");

            LoadedUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersJson);
        }

        public static void LoadGenres()
        {
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");

            LoadedGenres = JsonConvert.DeserializeObject<List<string>>(genresJson);
        }

        public static void LoadReports()
        {
            string reportsJson = File.ReadAllText(FolderPaths.JSONS + "Reports.json");

            LoadedReports = JsonConvert.DeserializeObject<List<ClassificationModel>>(reportsJson);
        }

        public static void SaveUsers(ObservableCollection<UserModel> users)
        {
            LoadedUsers = new List<UserModel>(users); // For signing out (reloading the file is unnecessary)

            File.WriteAllText(FolderPaths.JSONS + "Users.json", JsonConvert.SerializeObject(LoadedUsers, Formatting.Indented));
        }

        public static void SaveReports(ObservableCollection<ClassificationModel> reports)
        {
            // Prevents duplications
            foreach (ClassificationModel addedReport in reports)
                if (!LoadedReports.Contains(addedReport))
                    LoadedReports.Add(addedReport);

            File.WriteAllText(FolderPaths.JSONS + "Reports.json", JsonConvert.SerializeObject(LoadedReports, Formatting.Indented));
        }
    }
}
