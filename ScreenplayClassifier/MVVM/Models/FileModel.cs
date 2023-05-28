using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenplayClassifier.MVVM.Models
{
    public class FileModel: PropertyChangeNotifier
    {
        // Fields
        private string title, text;

        // Properties
        [BsonId]
        public ObjectId Id { get; set; }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;

                NotifyPropertyChange();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public FileModel(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}