using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ScreenplayClassifier.Utilities
{
    public static class Storage
    {
        // Methods
        public static ObservableCollection<UserModel> ReadAuthenticatedUsers()
        {
            ObservableCollection<UserModel> authenticatedUsers = new ObservableCollection<UserModel>();

            authenticatedUsers.Add(new UserModel("RanYunger", UserModel.UserRole.ADMIN, "RY120696"));
            authenticatedUsers.Add(new UserModel("ShyOhevZion", UserModel.UserRole.MEMBER, "SHZ12345"));

            return authenticatedUsers;
        }

        public static Dictionary<string, ObservableCollection<ScreenplayModel>> ReadGenresDictionary()
        {
            string[] genreNames = ReadGenresName();
            Dictionary<string, ObservableCollection<ScreenplayModel>> genresDictionary
                = new Dictionary<string, ObservableCollection<ScreenplayModel>>();

            foreach (string genreName in genreNames)
                genresDictionary[genreName] = ReadScreenplaysInGenre(genreName);

            return genresDictionary;
        }

        public static string[] ReadGenresName()
        {
            List<string> genreNames = new List<string>();

            genreNames.AddRange(new string[] { "Action", "Adventure", "Comedy", "Crime" });
            genreNames.AddRange(new string[] { "Drama", "Family", "Fantasy", "Horror" });
            genreNames.AddRange(new string[] { "Romance", "SciFi", "Thriller", "War" });

            return genreNames.ToArray();
        }

        public static ObservableCollection<ScreenplayModel> ReadScreenplaysInGenre(string genreName)
        {
            ObservableCollection<ScreenplayModel> screenplaysInGenre = new ObservableCollection<ScreenplayModel>();

            if (genreName == "Horror")
                screenplaysInGenre.Add(new ScreenplayModel("Saw", "bla.txt"));

            return screenplaysInGenre;
        }
    }
}
