using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ScreenplayClassifier.Utilities
{
    public static class Storage
    {
        // Methods
        public static ObservableCollection<UserModel> LoadUsers()
        {
            string usersJson = File.ReadAllText(FolderPaths.JSONS + "Users.json");

            return new ObservableCollection<UserModel>(JsonSerializer.Deserialize<List<UserModel>>(usersJson));
        }

        public static Dictionary<string, ObservableCollection<ScreenplayModel>> LoadArchives()
        {
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");
            string[] genreNames = JsonSerializer.Deserialize<string[]>(genresJson);
            string screenplaysInGenreJson = string.Empty;
            List<ScreenplayModel> screenplaysInGenre = null;
            Dictionary<string, ObservableCollection<ScreenplayModel>> genresDictionary
                = new Dictionary<string, ObservableCollection<ScreenplayModel>>();

            foreach (string genreName in genreNames)
            {
                screenplaysInGenreJson = File.ReadAllText(FolderPaths.SCREENPLAYS + genreName + ".json");
                screenplaysInGenre = JsonSerializer.Deserialize<List<ScreenplayModel>>(screenplaysInGenreJson);
                genresDictionary[genreName] = new ObservableCollection<ScreenplayModel>(screenplaysInGenre);
            }

            return genresDictionary;
        }

        public static void SaveArchives(Dictionary<string, ObservableCollection<ScreenplayModel>> genresDictionary)
        {
            string currentJsonFile = string.Empty, screenplaysInGenreJson = string.Empty;

            foreach (string genreName in genresDictionary.Keys)
            {
                currentJsonFile = FolderPaths.SCREENPLAYS + genreName + ".json";
                screenplaysInGenreJson = JsonSerializer.Serialize(currentJsonFile);

                File.WriteAllText(currentJsonFile, screenplaysInGenreJson);
            }
        }
    }
}
