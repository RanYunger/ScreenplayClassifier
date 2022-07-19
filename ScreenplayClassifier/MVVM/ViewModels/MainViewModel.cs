using ScreenplayClassifier.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    class MainViewModel
    {
        // Properties
        public UserModel user;

        // Constructors
        public MainViewModel(UserModel userProfile)
        {
            user = userProfile;
        }

        // Methods
        #region Commands
        #endregion
    }
}
