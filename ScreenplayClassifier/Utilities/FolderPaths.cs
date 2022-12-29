using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class FolderPaths
    {
        // Constants
        #region Media
        public static string MEDIA = Environment.CurrentDirectory + @"\Media\";

        public static string AUDIOS = MEDIA + @"\Audios\";
        public static string GENREAUDIOS = AUDIOS + @"\Genres\";

        public static string GIFS = MEDIA + @"\GIFs\";
        public static string GENREGIFS = GIFS + @"\Genres\";

        public static string IMAGES = MEDIA + @"\Images\";
        public static string GENREIMAGES = IMAGES + @"\Genres\";

        public static string VIDEOS = MEDIA + @"\Videos\";
        #endregion

        public static string CLASSIFIER = Environment.CurrentDirectory + @"\Classifier\";

        public static string JSONS = Environment.CurrentDirectory + @"\Jsons\";
    }
}
