using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class CONFIGURATIONS
    {
        // Fields
        public static Constants CONSTANTS;

        public static List<string> Genres;

        // Methods
        public static void Init()
        {
            string configurationsJson = File.ReadAllText(FolderPaths.JSONS + "Configurations.json");
            string genresJson = File.ReadAllText(FolderPaths.JSONS + "Genres.json");

            CONSTANTS = JsonConvert.DeserializeObject<Constants>(configurationsJson);

            Genres = JsonConvert.DeserializeObject<List<string>>(genresJson);
        }
    }
}
