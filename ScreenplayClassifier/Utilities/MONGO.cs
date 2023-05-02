﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class MONGO
    {
        // Fields
        public static MongoConfigurations CONFIGURATIONS;
        public static IMongoDatabase DATABASE;

        // Fields
        public static List<string> Genres;
        public static IMongoQueryable<UserModel> Users;

        // Methods
        /// <summary>
        /// Initiates the MongoDB client.
        /// </summary>
        public static void Init()
        {
            string configurationsJson = File.ReadAllText(FolderPaths.JSONS + "MongoDB.json");
            MongoClientSettings settings;

            CONFIGURATIONS = JsonConvert.DeserializeObject<MongoConfigurations>(configurationsJson);
            settings = MongoClientSettings.FromConnectionString(CONFIGURATIONS.ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            DATABASE = new MongoClient(settings).GetDatabase(CONFIGURATIONS.DatabaseName);
        }

        /// <summary>
        /// Loads the genres collection from the server.
        /// </summary>
        public static void LoadGenres()
        {
            IMongoQueryable<BsonDocument> genreDocuments = DATABASE.GetCollection<BsonDocument>(CONFIGURATIONS.GenresCollectionName).AsQueryable();

            Genres = new List<string>();
            foreach (BsonDocument document in genreDocuments)
                Genres.Add(document.GetValue("genre").ToString());
        }

        #region Reports
        /// <summary>
        /// Loads the reports collection (according to the signed in user's role) from the server.
        /// <param name="user">The signed in user</param>
        /// </summary>
        /// <returns>The collection of reports loaded from the server.</returns>
        public static ObservableCollection<ReportModel> LoadReports(UserModel user)
        {
            IMongoQueryable<ReportModel> reports = null;

            // Guests are invisible to the database - their actions are not saved
            if (user.Role == UserModel.UserRole.GUEST)
                return new ObservableCollection<ReportModel>();

            // Admins can see all reports; Members can see only their reports
            reports = DATABASE.GetCollection<ReportModel>(CONFIGURATIONS.ReportsCollectionName).AsQueryable();

            if (user.Role == UserModel.UserRole.MEMBER)
                reports = reports.Where(report => report.Owner.Username.Equals(user.Username));

            return new ObservableCollection<ReportModel>(reports);
        }

        /// <summary>
        /// Adds reports to the reports collection.
        /// </summary>
        /// <param name="reports">The reports to add</param>
        public static void AddReports(List<ReportModel> reports)
        {
            DATABASE.GetCollection<ReportModel>(CONFIGURATIONS.ReportsCollectionName).InsertMany(reports);
        }
        #endregion

        #region Users
        /// <summary>
        /// Loads the users collection from server.
        /// </summary>
        public static void LoadUsers()
        {
            Users = DATABASE.GetCollection<UserModel>(CONFIGURATIONS.UsersCollectionName).AsQueryable();
        }

        /// <summary>
        /// Adds a user to the users collection.
        /// </summary>
        /// <param name="user">The user to add</param>
        public static void AddUser(UserModel user)
        {
            DATABASE.GetCollection<UserModel>(CONFIGURATIONS.UsersCollectionName).InsertOne(user);
        }

        /// <summary>
        /// Updates a user in the users collection.
        /// </summary>
        /// <param name="user">The user to update</param>
        public static void UpdateUser(UserModel user)
        {
            FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
            UpdateDefinition<UserModel> update = Builders<UserModel>.Update
                .Set("Role", user.Role)
                .Set("Password", user.Password);

            DATABASE.GetCollection<UserModel>(CONFIGURATIONS.UsersCollectionName).UpdateOne(u => u.Id.Equals(user.Id), update);
        }

        /// <summary>
        /// Removes a user to the users collection.
        /// </summary>
        /// <param name="user">The user to remove</param>
        public static void RemoveUser(UserModel user)
        {
            FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("Id", user.Id);

            DATABASE.GetCollection<UserModel>(CONFIGURATIONS.UsersCollectionName).DeleteOne(filter);
        }
        #endregion
    }
}