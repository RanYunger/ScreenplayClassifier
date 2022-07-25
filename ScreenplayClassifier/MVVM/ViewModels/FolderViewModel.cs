using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FolderViewModel
    {
        // Fields
        private string basePath = Environment.CurrentDirectory + @"\Media\Images\";

        // Properties
        public FolderView FolderView { get; private set; }
        public string ItemsCount { get; private set; }

        // Constructors
        public FolderViewModel() { }

        // Methods
        public void Init(FolderView folderView, string genreName, int itemsCount)
        {
            // TODO: FIX (textblock not showing in runtime)
            Image folderImage = null, genreImage = null;
            TextBlock screenplaysCountTextBlock = null;
            string folderImageFilePath = string.Format("{0}{1}", basePath, itemsCount > 0 ? "FullFolder.png" : "EmptyFolder.png"),
                genreImageFilePath = string.Format(@"{0}\Genres\{1}.png", basePath, genreName);

            FolderView = folderView;
            folderImage = (Image)FolderView.FindName("FolderImage");
            genreImage = (Image)FolderView.FindName("GenreImage");
            screenplaysCountTextBlock = (TextBlock)FolderView.FindName("ScreenplaysCountTextBlock");

            folderImage.Source = new BitmapImage(new Uri(folderImageFilePath));
            genreImage.Source = new BitmapImage(new Uri(genreImageFilePath));
            screenplaysCountTextBlock.Text = string.Format("{0} {1}", itemsCount, itemsCount == 1 ? "Screenplay" : "Screenplays");
        }
    }
}
