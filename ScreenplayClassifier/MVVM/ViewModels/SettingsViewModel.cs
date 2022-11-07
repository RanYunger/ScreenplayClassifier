using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        // Fields
        private bool isOldPasswordVisible, isNewPasswordVisible;
        private string oldPassword, newPassword, confirmedPassword;
        private ObservableCollection<UserModel> authenticatedUsers;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public SettingsView SettingsView { get; private set; }

        public bool IsOldPasswordVisible
        {
            get { return isOldPasswordVisible; }
            set
            {
                isOldPasswordVisible = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsOldPasswordVisible"));
            }
        }

        public bool IsNewPasswordVisible
        {
            get { return isNewPasswordVisible; }
            set
            {
                isNewPasswordVisible = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsNewPasswordVisible"));
            }
        }

        public string OldPassword
        {
            get { return oldPassword; }
            set
            {
                oldPassword = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OldPassword"));
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NewPassword"));
            }
        }

        public string ConfirmedPassword
        {
            get { return confirmedPassword; }
            set
            {
                confirmedPassword = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ConfirmedPassword"));
            }
        }

        public ObservableCollection<UserModel> AuthenticatedUsers
        {
            get { return authenticatedUsers; }
            set
            {
                authenticatedUsers = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AuthenticatedUsers"));
            }
        }

        // Constructors
        public SettingsViewModel() { }

        // Methods
        #region Commands
        public Command ToggleOldPasswordVisibilityCommand
        {
            get
            {
                return new Command(() =>
                {
                    PasswordBox oldPasswordBox = (PasswordBox)SettingsView.FindName("OldPasswordBox");

                    IsOldPasswordVisible = !IsOldPasswordVisible;
                    oldPasswordBox.Visibility = IsOldPasswordVisible ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }

        public Command ToggleNewPasswordVisibilityCommand
        {
            get
            {
                return new Command(() =>
                {
                    PasswordBox newPasswordBox = (PasswordBox)SettingsView.FindName("NewPasswordBox");
                    TextBox newPasswordTextBox = (TextBox)SettingsView.FindName("NewPasswordTextBox");

                    newPasswordBox.Password = NewPassword = IsNewPasswordVisible ? newPasswordTextBox.Text : newPasswordBox.Password;

                    IsNewPasswordVisible = !IsNewPasswordVisible;
                    newPasswordBox.Visibility = IsNewPasswordVisible ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }

        public Command ChangePasswordCommand
        {
            get
            {
                return new Command(() =>
                {
                    Regex passwordRegex = new Regex(Storage.PASSWORDPATTERN);
                    PasswordBox confirmPasswordBox = (PasswordBox)SettingsView.FindName("ConfirmPasswordBox");

                    // Validations
                    if (string.IsNullOrEmpty(NewPassword))
                    {
                        MessageBoxHandler.Show("Enter new password", "Error", 3, MessageBoxImage.Error);
                        return;
                    }

                    if (!passwordRegex.IsMatch(NewPassword))
                    {
                        MessageBoxHandler.Show("New password is invalid", "Error", 3, MessageBoxImage.Error);
                        return;
                    }

                    ConfirmedPassword = confirmPasswordBox.Password;
                    if (!string.Equals(NewPassword, ConfirmedPassword))
                    {
                        MessageBoxHandler.Show("New password must be confirmed", "Error", 3, MessageBoxImage.Error);
                        return;
                    }

                    MainViewModel.UserToolbarViewModel.User.Password = NewPassword;
                    MessageBoxHandler.Show("Password changed successfuly", "Success", 3, MessageBoxImage.Information);
                });
            }
        }
        #endregion

        public void Init(SettingsView settingsView, MainViewModel mainViewModel, ObservableCollection<UserModel> authenticatedUsers)
        {
            PasswordBox oldPasswordBox, newPasswordBox, confirmPasswordBox;

            MainViewModel = mainViewModel;
            SettingsView = settingsView;

            AuthenticatedUsers = authenticatedUsers;
            IsOldPasswordVisible = false;
            IsNewPasswordVisible = false;

            oldPasswordBox = (PasswordBox)SettingsView.FindName("OldPasswordBox");
            oldPasswordBox.Password = OldPassword = MainViewModel.UserToolbarViewModel.User.Password;

            newPasswordBox = (PasswordBox)SettingsView.FindName("NewPasswordBox");
            newPasswordBox.Password = NewPassword = string.Empty;

            confirmPasswordBox = (PasswordBox)SettingsView.FindName("ConfirmPasswordBox");
            confirmPasswordBox.Password = ConfirmedPassword = string.Empty;
        }
    }
}
