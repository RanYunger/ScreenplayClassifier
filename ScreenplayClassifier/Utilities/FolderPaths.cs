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

        public static string IMAGES = MEDIA + @"\Images\";
        public static string GENREIMAGES = IMAGES + @"\Genres\";

        public static string VIDEOS = MEDIA + @"\Videos\";
        #endregion

        public static string GENRES = Environment.CurrentDirectory + @"\Genres\";
        public static string SCREENPLAYS = Environment.CurrentDirectory + @"\Screenplays\";

        public static string AUTHENTICATED = Environment.CurrentDirectory + @"\Authenticated\";
        public static string ADMINS = AUTHENTICATED + @"\Admins\";
        public static string MEMBERS = AUTHENTICATED + @"\Members\";
    }
}
