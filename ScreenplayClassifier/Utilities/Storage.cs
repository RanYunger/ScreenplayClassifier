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
            string reports = File.ReadAllText(FolderPaths.JSONS + "Reports.json");

            return new ObservableCollection<ClassificationModel>(JsonConvert.DeserializeObject<List<ClassificationModel>>(reports));
        }

        public static void SaveReports(ObservableCollection<ClassificationModel> reports)
        {
            string reportsJsonFile = FolderPaths.JSONS + "Reports.json";
            string reportsJson = string.Empty, reportJson = string.Empty;

            foreach (ClassificationModel report in reports)
            {
                reportJson = JsonConvert.SerializeObject(report);
                reportJson = reportJson.Substring(1, reportJson.Length - 2); // Trim "[", "]" from json
                reportsJson += reportJson;
            }

            File.WriteAllText(reportsJsonFile, "[" + reportsJson + "]");
        }

        public static Dictionary<string, ObservableCollection<ScreenplayModel>> LoadArchives()
        {
            string archivesJson = File.ReadAllText(FolderPaths.JSONS + "Archives.json");
            List<ScreenplayModel> genreScreenplays, allScreenplays = JsonConvert.DeserializeObject<List<ScreenplayModel>>(archivesJson);
            List<string> genreNames = LoadGenres();
            Dictionary<string, ObservableCollection<ScreenplayModel>> archives = new Dictionary<string, ObservableCollection<ScreenplayModel>>();

            foreach (string genreName in genreNames)
            {
                genreScreenplays = allScreenplays.FindAll(screenplay => screenplay.ActualGenre == genreName);
                archives[genreName] = new ObservableCollection<ScreenplayModel>(genreScreenplays);
            }

            return archives;
        }

        public static void SaveArchives(Dictionary<string, ObservableCollection<ScreenplayModel>> archives)
        {
            string archivesJsonFile = FolderPaths.JSONS + "Archives.json";
            string archivesJson = string.Empty, screenplaysInGenreJson = string.Empty;

            foreach (string genreName in archives.Keys)
            {
                screenplaysInGenreJson = JsonConvert.SerializeObject(archives[genreName]);
                screenplaysInGenreJson = screenplaysInGenreJson.Substring(1, screenplaysInGenreJson.Length - 2); // Trim "[", "]" from json
                archivesJson += screenplaysInGenreJson;
            }

            File.WriteAllText(archivesJsonFile, "[" + archivesJson + "]");
        }
    }
}
