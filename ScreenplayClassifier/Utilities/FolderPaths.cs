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

        public static string IMAGES = MEDIA + @"\Images\";
        public static string GENREIMAGES = IMAGES + @"\Genres\";

        public static string VIDEOS = MEDIA + @"\Videos\";
        #endregion

        #region Configuration
        public static string GENRES = Environment.CurrentDirectory + @"\Genres\";
        #endregion

        #region Storage
        public static string SCREENPLAYS = Environment.CurrentDirectory + @"\Screenplays\";
        #endregion

        #region Authentication
        public static string USERS = Environment.CurrentDirectory + @"\Users\";
        public static string ADMINS = USERS + @"\Admins\";
        public static string MEMBERS = USERS + @"\Members\";
        #endregion
    }
}
