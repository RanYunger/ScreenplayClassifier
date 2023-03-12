using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class SelectionModel : INotifyPropertyChanged
    {
        // Fields
        private string ownerName, screenplayFilePath, screenplayFileName;
        private bool isChecked;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public string OwnerName
        {
            get { return ownerName; }
            set
            {
                ownerName = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OwnerName"));
            }
        }

        public string ScreenplayFilePath
        {
            get { return screenplayFilePath; }
            set
            {
                screenplayFilePath = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayFilePath"));
            }
        }

        public string ScreenplayFileName
        {
            get { return screenplayFileName; }
            set
            {
                screenplayFileName = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ScreenplayFileName"));
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        // Constructors
        public SelectionModel(string ownerName, string screenplayFilePath)
        {
            OwnerName = ownerName;
            ScreenplayFilePath = screenplayFilePath;
            ScreenplayFileName = Path.GetFileNameWithoutExtension(screenplayFilePath);
            IsChecked = false;
        }
    }
}