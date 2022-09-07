﻿using Newtonsoft.Json;
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

        public static void SaveReports(ObservableCollection<ClassificationModel> reports)
        {
            File.WriteAllText(FolderPaths.JSONS + "Reports.json", JsonConvert.SerializeObject(reports, Formatting.Indented));
        }
    }
}
