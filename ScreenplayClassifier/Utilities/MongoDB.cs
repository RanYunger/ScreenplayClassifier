using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class MongoDB
    {
        // Fields
        public static IMongoDatabase DATABASE;
        public static MongoClient CLIENT;

        // Methods
        public static void InitMongoDBServer()
        {
            string configurationsJson = File.ReadAllText(FolderPaths.JSONS + "MongoDB.json");
            Configurations mongoDB = JsonConvert.DeserializeObject<Configurations>(configurationsJson);
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(mongoDB.ConnectionString);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            CLIENT = new MongoClient(settings);
            DATABASE = CLIENT.GetDatabase(mongoDB.DatabaseName);
        }

        public static void LoadGenres()
        {
            // Reading collection from database
            IMongoQueryable<BsonDocument> genres = DATABASE.GetCollection<BsonDocument>("genres").AsQueryable();
            foreach (BsonDocument genre in genres)
            {
                string x = genre.GetValue("genre").ToString();
                Console.WriteLine(x);
            }
        }

        public static void LoadUsers()
        {
            IMongoQueryable<UserModel> loadedUsers = DATABASE.GetCollection<UserModel>("users").AsQueryable();
            foreach (UserModel user in loadedUsers)
            {
                Console.WriteLine(user.Role);
            }
        }
    }
}