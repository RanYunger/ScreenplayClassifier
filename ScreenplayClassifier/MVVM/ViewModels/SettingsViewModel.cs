using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static ScreenplayClassifier.MVVM.Models.UserModel;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SettingsViewModel : PropertyChangeNotifier
    {
        // Fields
        private Predicate<object> usernameFilter;

        private ObservableCollection<UserModel> authenticatedUsers;
        private int selectedUser;
        private bool isNewPasswordVisible, canAdd, canRemove, canChangeRole;
        private string oldPassword, newPassword, confirmedPassword;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public SettingsView SettingsView { get; private set; }

        public ObservableCollection<UserModel> AuthenticatedUsers
        {
            get { return authenticatedUsers; }
            set
            {
                authenticatedUsers = value;

                NotifyPropertyChange();
            }
        }

        public int SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                CanRemove = selectedUser != -1;
                CanChangeRole = selectedUser != -1;

                NotifyPropertyChange();
            }
        }

        public bool IsNewPasswordVisible
        {
            get { return isNewPasswordVisible; }
            set
            {
                isNewPasswordVisible = value;

                NotifyPropertyChange();
            }
        }

        public bool CanAdd
        {
            get { return canAdd; }
            set
            {
                canAdd = value;

                NotifyPropertyChange();
            }
        }

        public bool CanRemove
        {
            get { return canRemove; }
            set
            {
                canRemove = value;

                NotifyPropertyChange();
            }
        }

        public bool CanChangeRole
        {
            get { return canChangeRole; }
            set
            {
                canChangeRole = value;

                NotifyPropertyChange();
            }
        }

        public string OldPassword
        {
            get { return oldPassword; }
            set
            {
                oldPassword = value;

                NotifyPropertyChange();
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;

                NotifyPropertyChange();
            }
        }

        public string ConfirmedPassword
        {
            get { return confirmedPassword; }
            set
            {
                confirmedPassword = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public SettingsViewModel() { }

        // Methods
        #region Commands
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
                        newPasswordTextBox.Visibility = System.Windows.Visibility.Visible;
                        newPasswordBox.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        newPasswordBox.Password = newPasswordTextBox.Text;
                        newPasswordTextBox.Visibility = System.Windows.Visibility.Collapsed;
                        newPasswordBox.Visibility = System.Windows.Visibility.Visible;
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
                    TextBox newPasswordTextBox = (TextBox)SettingsView.FindName("NewPasswordTextBox");
                    PasswordBox newPasswordBox = (PasswordBox)SettingsView.FindName("NewPasswordBox"),
                        confirmPasswordBox = (PasswordBox)SettingsView.FindName("ConfirmPasswordBox");
                    int userOffset = AuthenticatedUsers.IndexOf(MainViewModel.UserToolbarViewModel.User);

                    // Obtains the current string representations of both old and new passwords
                    OldPassword = MainViewModel.UserToolbarViewModel.User.Password;
                    NewPassword = IsNewPasswordVisible ? newPasswordTextBox.Text.Trim() : newPasswordBox.Password.Trim();

                    // Validations
                    if (string.IsNullOrEmpty(NewPassword))
                    {
                        MessageBox.ShowError("Enter new password.");
                        return;
                    }

                    if (!passwordRegex.IsMatch(NewPassword))
                    {
                        MessageBox.ShowError("Password must contain 2-3 capital letters followed by 5-6 digits.");
                        return;
                    }

                    if (OldPassword.Equals(NewPassword))
                    {
                        MessageBox.ShowError("This is your current password.");
                        return;
                    }

                    ConfirmedPassword = confirmPasswordBox.Password.Trim();
                    if (!ConfirmedPassword.Equals(NewPassword))
                    {
                        MessageBox.ShowError("New password must be confirmed.");
                        return;
                    }

                    AuthenticatedUsers[userOffset].Password = NewPassword;

                    newPasswordBox.Clear();
                    newPasswordTextBox.Clear();
                    confirmPasswordBox.Clear();

                    MessageBox.ShowInformation("Password changed successfuly.");
                });
            }
        }

        public Command EnterUsernameTextboxCommand
        {
            get
            {
                TextBox usernameTextBox = null;

                return new Command(() =>
                {
                    string usernameInput = string.Empty;

                    // Validation
                    if (SettingsView == null)
                        return;

                    usernameTextBox = (TextBox)SettingsView.FindName("UsernameTextBox");
                    usernameInput = usernameTextBox.Text;

                    if (string.Equals(usernameInput, "Search by username"))
                    {
                        usernameTextBox.Foreground = Brushes.Black;
                        usernameTextBox.Text = string.Empty;
                    }
                });
            }
        }

        public Command LeaveUsernameTextboxCommand
        {
            get
            {
                TextBox usernameTextBox = null;

                return new Command(() =>
                {
                    string usernameInput = string.Empty;

                    if (SettingsView == null)
                        return;

                    usernameTextBox = (TextBox)SettingsView.FindName("UsernameTextBox");
                    usernameInput = usernameTextBox.Text;

                    if (string.IsNullOrEmpty(usernameInput))
                    {
                        usernameTextBox.Foreground = Brushes.Gray;
                        usernameTextBox.Text = "Search by username";
                    }
                });
            }
        }

        public Command SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    TextBox usernameTextBox = (TextBox)SettingsView.FindName("UsernameTextBox");
                    ICollectionView usersCollectionView = CollectionViewSource.GetDefaultView(AuthenticatedUsers);
                    string usernameInput = usernameTextBox.Text;

                    // Updates and activates the filter
                    usernameFilter = (o) =>
                    {
                        return (string.IsNullOrEmpty(usernameInput.Trim())) || (string.Equals(usernameInput, "Search by username"))
                            ? true : ((UserModel)o).Username.Contains(usernameInput);
                    };
                    usersCollectionView.Filter = (o) => { return usernameFilter.Invoke(o); };
                    usersCollectionView.Refresh();

                    // Checks whether the searched user can be added
                    CanAdd = !string.IsNullOrEmpty(usernameInput.Trim());
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
                    Regex usernameRegex = new Regex(JSON.USERNAMEPATTERN);
                    TextBox usernameTextBox = (TextBox)SettingsView.FindName("UsernameTextBox");
                    string usernameInput = usernameTextBox.Text;

                    if (!usernameRegex.IsMatch(usernameInput))
                    {
                        MessageBox.ShowError("Username must contain 2-3 capital words seperated by dots (E.G. Ran.Yunger, Shy.Ohev.Zion)");
                        return;
                    }

                    AuthenticatedUsers.Add(new UserModel(usernameInput, UserModel.UserRole.MEMBER, "ABC123"));

                    MessageBox.ShowInformation(usernameInput + " added successfuly");
                });
            }
        }

        public Command RemoveMemberCommand
        {
            get
            {
                return new Command(() =>
                {
                bool removeConfirmation = MessageBox.ShowQuestion(string.Format("Are you sure you want to remove {0}?" +
                    "\nThis action cannot be undone.", AuthenticatedUsers[SelectedUser].Username), true);

                    if (removeConfirmation)
                        AuthenticatedUsers.RemoveAt(SelectedUser);
                });
            }
        }

        public Command ChangeRoleCommand
        {
            get
            {
                return new Command(() =>
                {
                    UserModel affectedUser = AuthenticatedUsers[selectedUser];
                    UserRole newRole = affectedUser.Role == UserRole.ADMIN ? UserRole.MEMBER : UserRole.ADMIN;
                    bool changeConfirmation = MessageBox.ShowQuestion(string.Format("Are you sure you want to change {0}'s role to {1}?", 
                        affectedUser.Username, newRole), true);

                    // Changes the user's role (if confirmation is given)
                    if (changeConfirmation)
                    {
                        affectedUser.Role = newRole;
                        MessageBox.ShowInformation(string.Format("{0} has been changed to {1}.", affectedUser.Username, newRole));
                    }
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
            PasswordBox newPasswordBox = null, confirmPasswordBox = null;
            TextBox usernameTextBox = null;

            SettingsView = settingsView;
            MainViewModel = mainViewModel;

            AuthenticatedUsers = authenticatedUsers;
            IsNewPasswordVisible = false;

            newPasswordBox = (PasswordBox)SettingsView.FindName("NewPasswordBox");
            newPasswordBox.Clear();

            confirmPasswordBox = (PasswordBox)SettingsView.FindName("ConfirmPasswordBox");
            confirmPasswordBox.Clear();

            usernameTextBox = (TextBox)SettingsView.FindName("UsernameTextBox");
            usernameTextBox.Foreground = Brushes.Gray;
            usernameTextBox.Text = "Search by username";

            CanAdd = false;
            CanRemove = false;
            CanChangeRole = false;
        }
    }
}