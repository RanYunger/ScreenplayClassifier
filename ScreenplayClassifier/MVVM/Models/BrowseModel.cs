using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class BrowseModel : INotifyPropertyChanged
    {
        // Fields
        private string screenplayFilePath, screenplayFileName;
        private bool isChecked;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
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
        public BrowseModel(string screenplayFilePath)
        {
            ScreenplayFilePath = screenplayFilePath;
            ScreenplayFileName = Path.GetFileNameWithoutExtension(screenplayFilePath);
            IsChecked = false;
        }
    }
}