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
                    TextBox oldPasswordTextBox = (TextBox)SettingsView.FindName("OldPasswordTextBox");

                    // Changes the password's visibility
                    IsOldPasswordVisible = !IsOldPasswordVisible;

                    if (IsOldPasswordVisible)
                    {
                        oldPasswordTextBox.Text = oldPasswordBox.Password;
                        oldPasswordTextBox.Visibility = Visibility.Visible;
                        oldPasswordBox.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        oldPasswordBox.Password = oldPasswordTextBox.Text;
                        oldPasswordTextBox.Visibility = Visibility.Collapsed;
                        oldPasswordBox.Visibility = Visibility.Visible;
                    }
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

                    // Changes the password's visibility
                    IsNewPasswordVisible = !IsNewPasswordVisible;

                    if (IsNewPasswordVisible)
                    {
                        newPasswordTextBox.Text = newPasswordBox.Password;
                        newPasswordTextBox.Visibility = Visibility.Visible;
                        newPasswordBox.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        newPasswordBox.Password = newPasswordTextBox.Text;
                        newPasswordTextBox.Visibility = Visibility.Collapsed;
                        newPasswordBox.Visibility = Visibility.Visible;
                    }
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
                    TextBox oldPasswordTextBox = (TextBox)SettingsView.FindName("OldPasswordTextBox"),
                        newPasswordTextBox = (TextBox)SettingsView.FindName("NewPasswordTextBox");
                    PasswordBox oldPasswordBox = (PasswordBox)SettingsView.FindName("OldPasswordBox"),
                        newPasswordBox = (PasswordBox)SettingsView.FindName("NewPasswordBox"),
                        confirmPasswordBox = (PasswordBox)SettingsView.FindName("ConfirmPasswordBox");
                    int userOffset = AuthenticatedUsers.IndexOf(MainViewModel.UserToolbarViewModel.User);

                    // Obtains the current string representations of both old and new passwords
                    OldPassword = IsOldPasswordVisible ? oldPasswordTextBox.Text.Trim() : oldPasswordBox.Password.Trim();
                    NewPassword = IsNewPasswordVisible ? newPasswordTextBox.Text.Trim() : newPasswordBox.Password.Trim();

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

                    ConfirmedPassword = confirmPasswordBox.Password.Trim();
                    if (!ConfirmedPassword.Equals(NewPassword))
                    {
                        MessageBoxHandler.Show("New password must be confirmed", "Error", 3, MessageBoxImage.Error);
                        return;
                    }

                    AuthenticatedUsers[userOffset].Password = oldPasswordBox.Password = oldPasswordTextBox.Text = NewPassword;

                    newPasswordBox.Clear();
                    newPasswordTextBox.Clear();
                    confirmPasswordBox.Clear();

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

                    // Updates and activates the filter
                    usernameFilter = (o) => { return string.IsNullOrEmpty(usernameInput) ? true : ((UserModel)o).Username.Contains(usernameInput); };

                    usersCollectionView.Filter = (o) => { return usernameFilter.Invoke(o); };
                    usersCollectionView.Refresh();

                    // Checks whether the searched user can be added
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
                        AuthenticatedUsers[SelectedUser].Username), "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (confirmResult == MessageBoxResult.Yes)
                        AuthenticatedUsers.RemoveAt(SelectedUser);
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="settingsView">The view to obtain controls from</param>
        /// <param name="mainViewModel">The MainView's view model</param>
        /// <param name="authenticatedUsers">List of all users who can authenticate to the system</param>
        public void Init(SettingsView settingsView, MainViewModel mainViewModel, ObservableCollection<UserModel> authenticatedUsers)
        {
            PasswordBox oldPasswordBox, newPasswordBox, confirmPasswordBox;

            MainViewModel = mainViewModel;
            SettingsView = settingsView;

            AuthenticatedUsers = authenticatedUsers;
            IsOldPasswordVisible = false;
            IsNewPasswordVisible = false;

            oldPasswordBox = (PasswordBox)SettingsView.FindName("OldPasswordBox");
            oldPasswordBox.Password = MainViewModel.UserToolbarViewModel.User.Password;

            newPasswordBox = (PasswordBox)SettingsView.FindName("NewPasswordBox");
            newPasswordBox.Clear();

            confirmPasswordBox = (PasswordBox)SettingsView.FindName("ConfirmPasswordBox");
            confirmPasswordBox.Clear();

            CanAdd = false;
            CanRemove = false;
        }
    }
}