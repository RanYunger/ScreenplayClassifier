using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class FolderViewModel
    {
        // Properties
        public FolderView FolderView { get; private set; }

        // Constructors
        public FolderViewModel() { }

        // Methods
        public void Init(FolderView folderView, string genreName, int itemsCount)
        {
            Image folderImage = null, genreImage = null;
            TextBlock screenplaysCountTextBlock = null;
            string folderImageFilePath = string.Format("{0}{1}", FolderPaths.IMAGES, itemsCount > 0 ? "FullFolder.png" : "EmptyFolder.png"),
                genreImageFilePath = string.Format(@"{0}{1}.png", FolderPaths.GENREIMAGES, genreName);

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
