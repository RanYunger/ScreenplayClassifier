using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FolderViewModel
    {
        // Fields
        private string basePath = Environment.CurrentDirectory + @"\Media\Images\";

        // Properties
        public BitmapImage FolderImage { get; private set; }
        public BitmapImage GenreImage { get; private set; }
        public string ItemsCount { get; private set; }

        // Constructors
        public FolderViewModel() { }

        // Methods
        public void Init(string genreName, int itemsCount)
        {
            string folderImageFilePath = string.Format("{0}{1}", basePath, itemsCount > 0 ? "FullFolder.png" : "EmptyFolder.png"),
                genreImageFilePath = string.Format(@"{0}\Genres\{1}.png", basePath, genreName);

            FolderImage = new BitmapImage(new Uri(folderImageFilePath));
            GenreImage = new BitmapImage(new Uri(genreImageFilePath));
            ItemsCount = string.Format("{0} {1}", itemsCount, itemsCount == 1 ? "Screenplay" : "Screenplays");
        }
    }
}
