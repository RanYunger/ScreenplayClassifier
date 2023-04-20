using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class SelectionEntryModel : PropertyChangeNotifier
    {
        // Fields
        private string ownerName, screenplayFilePath, screenplayFileName;
        private bool isChecked;

        // Properties
        public string OwnerName
        {
            get { return ownerName; }
            set
            {
                ownerName = value;

                NotifyPropertyChange();
            }
        }

        public string ScreenplayFilePath
        {
            get { return screenplayFilePath; }
            set
            {
                screenplayFilePath = value;

                NotifyPropertyChange();
            }
        }

        public string ScreenplayFileName
        {
            get { return screenplayFileName; }
            set
            {
                screenplayFileName = value;

                NotifyPropertyChange();
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                // Validation - value change will cause a cascade of changes
                if (isChecked == value)
                    return;

                isChecked = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public SelectionEntryModel(string ownerName, string screenplayFilePath)
        {
            OwnerName = ownerName;
            ScreenplayFilePath = screenplayFilePath;
            ScreenplayFileName = Path.GetFileNameWithoutExtension(screenplayFilePath);
            IsChecked = false;
        }
    }
}