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
        public static ObservableCollection<UserModel> loadedUsers;
        public static ObservableCollection<string> loadedGenres;
        public static ObservableCollection<ClassificationModel> loadedReports;

        // Methods
        public static ObservableCollection<UserModel> LoadUsers()
        {
            string usersJson = File.ReadAllText(FolderPaths.JSONS + "Users.json");
            List<UserModel> deserializedUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersJson);

            loadedUsers = new ObservableCollection<UserModel>(deserializedUsers);

            return loadedUsers;
        }

        public static ObservableCollection<string> LoadGenres()
        {
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");
            List<string> deserializedGenres = JsonConvert.DeserializeObject<List<string>>(genresJson);

            loadedGenres = new ObservableCollection<string>(deserializedGenres);

            return loadedGenres;
        }

        public static ObservableCollection<ClassificationModel> LoadReports()
        {
            string reportsJson = File.ReadAllText(FolderPaths.JSONS + "Reports.json");
            List<ClassificationModel> deserializedReports = JsonConvert.DeserializeObject<List<ClassificationModel>>(reportsJson);

            loadedReports = new ObservableCollection<ClassificationModel>(deserializedReports);

            return loadedReports;
        }

        public static void SaveUsers(ObservableCollection<UserModel> users)
        {
            // TODO: FIX (no change seems to be saved)
            File.WriteAllText(FolderPaths.JSONS + "Users.json", JsonConvert.SerializeObject(users, Formatting.Indented));
        }

        public static void SaveReports(ObservableCollection<ClassificationModel> reports)
        {
            // Prevents duplications
            foreach (ClassificationModel addedReport in reports)
                if (!loadedReports.Contains(addedReport))
                    loadedReports.Add(addedReport);

            File.WriteAllText(FolderPaths.JSONS + "Reports.json", JsonConvert.SerializeObject(loadedReports, Formatting.Indented));
        }
    }
}
