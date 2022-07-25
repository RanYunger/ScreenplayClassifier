using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ScreenplayModel
    {
        private static int IDGenerator = 1;

        // Properties
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string FilePath { get; private set; }

        // Constructors
        public ScreenplayModel(string name, string filePath)
        {
            ID = IDGenerator++;
            Name = name;
            FilePath = filePath;
        }
    }
}
