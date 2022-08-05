using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel : INotifyPropertyChanged
    {
        // Fields
        private static int IDGenerator = 0;

        private int screenplayID;
        private string name, filePath;
        private string[] designatedGenres, classifiedGenres;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public int ID
        {
            get { return screenplayID; }
            set
            {
                screenplayID = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
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

        public string DesignatedGenre
        {
            get { return designatedGenres[0]; }
            set
            {
                designatedGenres[0] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DesignatedGenre"));
            }
        }

        public string DesignatedSubGenre1
        {
            get { return designatedGenres[1]; }
            set
            {
                designatedGenres[1] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DesignatedSubGenre1"));
            }
        }

        public string DesignatedSubGenre2
        {
            get { return designatedGenres[2]; }
            set
            {
                designatedGenres[2] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DesignatedSubGenre2"));
            }
        }

        public string ClassifiedGenre
        {
            get { return classifiedGenres[0]; }
            set
            {
                classifiedGenres[0] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedGenre"));
            }
        }

        public string ClassifiedSubGenre1
        {
            get { return classifiedGenres[1]; }
            set
            {
                classifiedGenres[1] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedSubGenre1"));
            }
        }

        public string ClassifiedSubGenre2
        {
            get { return classifiedGenres[2]; }
            set
            {
                classifiedGenres[2] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedSubGenre2"));
            }
        }

        // Constructors
        public ScreenplayModel(string name, string filePath)
        {
            designatedGenres = classifiedGenres = new string[3];

            ID = IDGenerator++;
            Name = name;
            FilePath = filePath;
            DesignatedGenre = DesignatedSubGenre1 = DesignatedSubGenre2 = "Unknown";
            ClassifiedGenre = ClassifiedSubGenre1 = ClassifiedSubGenre2 = "Unknown";
        }
    }
}
