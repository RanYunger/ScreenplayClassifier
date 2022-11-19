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
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        // Fields
        private Predicate<object> usernameFilter;

        private ObservableCollection<UserModel> authenticatedUsers;
        private int selectedUser;
        private bool isOldPasswordVisible, isNewPasswordVisible, canAdd, canRemove;
        private string oldPassword, newPassword, confirmedPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public SettingsView SettingsView { get; private set; }

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

        public int SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                CanRemove = selectedUser != -1;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
            }
        }

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

        public bool CanAdd
        {
            get { return canAdd; }
            set
            {
                canAdd = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanAdd"));
            }
        }

        public bool CanRemove
        {
            get { return canRemove; }
            set
            {
                canRemove = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanRemove"));
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
                    Regex passwordRegex = new Regex(JSON.PASSWORDPATTERN);
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

                    if (OldPassword.Equals(NewPassword))
                    {
                        MessageBoxHandler.Show("This is your current password", "Error", 3, MessageBoxImage.Error);
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

        public Command SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    TextBox usernameInputTextBox = (TextBox)SettingsView.FindName("UsernameInputTextBox");
                    ICollectionView usersCollectionView = CollectionViewSource.GetDefaultView(AuthenticatedUsers);
                    string usernameInput = usernameInputTextBox.Text;

                    usernameFilter = (o) => { return string.IsNullOrEmpty(usernameInput) ? true : ((UserModel)o).Username.Contains(usernameInput); };

                    usersCollectionView.Filter = (o) => { return usernameFilter.Invoke(o); };
                    usersCollectionView.Refresh();

                    CanAdd = true;
                    foreach (UserModel user in AuthenticatedUsers)
                    {
                        if (user.Username.Equals(usernameInput))
                        {
                            CanAdd = false;
                            break;
                        }
                    }
                });
            }
        }

        public Command AddMemberCommand
        {
            get
            {
                return new Command(() =>
                {
                    TextBox usernameInputTextBox = (TextBox)SettingsView.FindName("UsernameInputTextBox");
                    string usernameInput = usernameInputTextBox.Text;

                    AuthenticatedUsers.Add(new UserModel(usernameInput, UserModel.UserRole.MEMBER, "ABC123"));

                    MessageBoxHandler.Show(usernameInput + " added successfuly", "Success", 3, MessageBoxImage.Information);
                });
            }
        }

        public Command RemoveMemberCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult confirmResult = MessageBox.Show(string.Format("Are you sure you want to remove {0}?",
                        AuthenticatedUsers[SelectedUser].Username, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning));

                    if (confirmResult == MessageBoxResult.Yes)
                        AuthenticatedUsers.RemoveAt(SelectedUser);
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

            CanAdd = false;
            CanRemove = false;
        }
    }
}
