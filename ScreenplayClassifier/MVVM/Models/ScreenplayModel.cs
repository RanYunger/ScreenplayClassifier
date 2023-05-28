using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel : PropertyChangeNotifier
    {
        // Fields
        private ObjectId fileId;
        private string title = null, filePath = null;
        private Dictionary<string, float> genrePercentages;
        private string modelGenre, modelSubGenre1, modelSubGenre2;
        private string ownerGenre, ownerSubGenre1, ownerSubGenre2;

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

        [BsonIgnore]
        public string Title
        {
            get
            {
                // title initialization from the associated screenplay 
                if (title == null)
                    Title = MONGODB.Screenplays.First(file => file.Id.Equals(FileId)).Title;

                return title;
            }

            set
            {
                title = value;

                NotifyPropertyChange();
            }
        }

        [BsonIgnore]
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;

                NotifyPropertyChange();
            }
        }

        public Dictionary<string, float> GenrePercentages
        {
            get { return genrePercentages; }
            set
            {
                genrePercentages = value;

                NotifyPropertyChange();
            }
        }

        public string ModelGenre
        {
            get { return modelGenre; }
            set
            {
                modelGenre = value;

                NotifyPropertyChange();
            }
        }

        public string ModelSubGenre1
        {
            get { return modelSubGenre1; }
            set
            {
                modelSubGenre1 = value;

                NotifyPropertyChange();
            }
        }

        public string ModelSubGenre2
        {
            get { return modelSubGenre2; }
            set
            {
                modelSubGenre2 = value;

                NotifyPropertyChange();
            }
        }

        public string OwnerGenre
        {
            get { return ownerGenre; }
            set
            {
                ownerGenre = value;

                NotifyPropertyChange();
            }
        }

        public string OwnerSubGenre1
        {
            get { return ownerSubGenre1; }
            set
            {
                ownerSubGenre1 = value;

                NotifyPropertyChange();
            }
        }

        public string OwnerSubGenre2
        {
            get { return ownerSubGenre2; }
            set
            {
                ownerSubGenre2 = value;

                NotifyPropertyChange();
            }
        }

        public bool Isfeedbacked
        {
            get { return (OwnerGenre != "Unknown") && (OwnerSubGenre1 != "Unknown") && (OwnerSubGenre2 != "Unknown"); }
        }

        public bool IsGenreClassifiedCorrectly
        {
            get { return (ModelGenre != "Unknown") && (ModelGenre == OwnerGenre); }
        }

        public bool IsSubGenre1ClassifiedCorrectly
        {
            get { return (ModelSubGenre1 != "Unknown") && (ModelSubGenre1 == OwnerSubGenre1); }
        }

        public bool IsSubGenre2ClassifiedCorrectly
        {
            get { return (ModelSubGenre2 != "Unknown") && (ModelSubGenre2 == OwnerSubGenre2); }
        }

        public bool IsClassifiedCorrectly
        {
            get { return (IsGenreClassifiedCorrectly) && (IsSubGenre1ClassifiedCorrectly) && (IsSubGenre2ClassifiedCorrectly); }
        }

        // Constructors
        public ScreenplayModel(string filePath, string title, Dictionary<string, float> genrePercentages)
        {
            List<string> predictedGenres = new List<string>(genrePercentages.Keys);

            FilePath = filePath;
            Title = title;

            GenrePercentages = genrePercentages;

            ModelGenre = predictedGenres[0];
            ModelSubGenre1 = predictedGenres[1];
            ModelSubGenre2 = predictedGenres[2];

            OwnerGenre = "Unknown";
            OwnerSubGenre1 = "Unknown";
            OwnerSubGenre2 = "Unknown";
        }
    }
}