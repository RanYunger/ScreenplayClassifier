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

        public static string GIFS = MEDIA + @"\Gifs\";
        public static string GENREGIFS = GIFS + @"\Genres\";

        public static string IMAGES = MEDIA + @"\Images\";
        public static string GENREIMAGES = IMAGES + @"\Genres\";

        public static string VIDEOS = MEDIA + @"\Videos\";
        #endregion

        #region Classifier
        public static string PYTHON = Environment.CurrentDirectory + @"\Python\";
        #endregion

        #region Storage
        public static string JSONS = Environment.CurrentDirectory + @"\Jsons\";
        #endregion
    }
}
