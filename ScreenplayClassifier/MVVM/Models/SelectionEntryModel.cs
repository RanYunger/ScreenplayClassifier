using MongoDB.Bson;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class SelectionEntryModel : PropertyChangeNotifier
    {
        // Fields
        private ObjectId fileId;
        private string ownerName, filePath, fileName;
        private bool isChecked;

        // Properties
        public ObjectId FileId
        {
            get { return fileId; }
            set
            {
                fileId = value;

                NotifyPropertyChange();
            }
        }

        public string OwnerName
        {
            get { return ownerName; }
            set
            {
                ownerName = value;

                NotifyPropertyChange();
            }
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;

                NotifyPropertyChange();
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;

                NotifyPropertyChange();
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                // Validation - property changes only if a different value is assigned
                if (isChecked == value)
                    return;

                isChecked = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public SelectionEntryModel(ObjectId fileId, string ownerName, string filePath, string fileName)
        {
            FileId = fileId;
            OwnerName = ownerName;
            FilePath = filePath;
            FileName = fileName;
            IsChecked = false;
        }

        public SelectionEntryModel(string ownerName, string filePath)
        {
            FileId = ObjectId.Empty;
            OwnerName = ownerName;
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(FilePath.Replace("\"", string.Empty));
            IsChecked = false;
        }
    }
}