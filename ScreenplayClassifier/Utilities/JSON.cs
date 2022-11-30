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
        public static ObservableCollection<UserModel> LoadedUsers;
        public static ObservableCollection<string> LoadedGenres;
        public static ObservableCollection<ClassificationModel> LoadedReports;

        // Methods
        public static void LoadUsers()
        {
            string usersJson = File.ReadAllText(FolderPaths.JSONS + "Users.json");
            List<UserModel> deserializedUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersJson);

            LoadedUsers = new ObservableCollection<UserModel>(deserializedUsers);
        }

        public static void LoadGenres()
        {
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");
            List<string> deserializedGenres = JsonConvert.DeserializeObject<List<string>>(genresJson);

            LoadedGenres = new ObservableCollection<string>(deserializedGenres);
        }

        public static void LoadReports()
        {
            string reportsJson = File.ReadAllText(FolderPaths.JSONS + "Reports.json");
            List<ClassificationModel> deserializedReports = JsonConvert.DeserializeObject<List<ClassificationModel>>(reportsJson);

            LoadedReports = new ObservableCollection<ClassificationModel>(deserializedReports);
        }

        public static void SaveUsers(ObservableCollection<UserModel> users)
        {
            LoadedUsers = users; // For signing out (reloading the file is unnecessary)

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
