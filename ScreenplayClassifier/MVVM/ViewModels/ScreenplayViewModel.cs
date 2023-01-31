using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class ScreenplayViewModel : INotifyPropertyChanged
    {
        // Fields
        public string filePath, title, text;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilePath"));
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("text"));
            }
        }

        // Constructors
        public ScreenplayViewModel() { }

        // Methods
        public void Init(string filePath)
        {
            FilePath = filePath;
            Title = Path.GetFileNameWithoutExtension(filePath);
            Text = File.ReadAllText(filePath);
        }
    }
}
