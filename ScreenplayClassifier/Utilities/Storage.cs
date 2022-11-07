using Newtonsoft.Json;
using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class Storage
    {
        // Constants
        public static string USERNAMEPATTERN = "([A-Z]{1}[a-z]+){2,3}"; // E.G. RanYunger, ShyOhevZion
        public static string PASSWORDPATTERN = "[A-Z]{2,3}[0-9]{5,6}"; // E.G. RY120696, SHZ101098

        // Methods
        public static ObservableCollection<UserModel> LoadUsers()
        {
            string usersJson = File.ReadAllText(FolderPaths.JSONS + "Users.json");

            return new ObservableCollection<UserModel>(JsonConvert.DeserializeObject<List<UserModel>>(usersJson));
        }

        public static List<string> LoadGenres()
        {
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");

            return JsonConvert.DeserializeObject<List<string>>(genresJson);
        }

        public static ObservableCollection<ClassificationModel> LoadReports()
        {
            string reportsJson = File.ReadAllText(FolderPaths.JSONS + "Reports.json");

            return new ObservableCollection<ClassificationModel>(JsonConvert.DeserializeObject<List<ClassificationModel>>(reportsJson));
        }

        public static void SaveUsers(ObservableCollection<UserModel> users)
        {
            File.WriteAllText(FolderPaths.JSONS + "Users.json", JsonConvert.SerializeObject(users, Formatting.Indented));
        }

        public static void SaveReports(ObservableCollection<ClassificationModel> reports)
        {
            File.WriteAllText(FolderPaths.JSONS + "Reports.json", JsonConvert.SerializeObject(reports, Formatting.Indented));
        }
    }
}
