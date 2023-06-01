using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ScreenplayClassifier.Utilities;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class EntryViewModel : PropertyChangeNotifier
    {
        // Fields
        private string usernameError, passwordError;
        private int attemptsCount;
        private bool canSignIn;

        // Properties
        public EntryView EntryView { get; private set; }

        public string UsernameError
        {
            get { return usernameError; }
            set
            {
                usernameError = value;

                NotifyPropertyChange();
            }
        }

        public string PasswordError
        {
            get { return passwordError; }
            set
            {
                passwordError = value;

                NotifyPropertyChange();
            }
        }

        public int AttemptsCount
        {
            get { return attemptsCount; }
            set
            {
                attemptsCount = value;

                NotifyPropertyChange();
            }
        }

        public bool CanSignIn
        {
            get { return canSignIn; }
            set
            {
                canSignIn = value;

                NotifyPropertyChange();
            }
        }

        // Constructors
        public EntryViewModel()
        {
            InitCongifurations();
            InitMongoDB();

            Init();
        }

        // Methods
        #region Commands
        public Command EnterUsernameTextboxCommand
        {
            get
            {
                TextBox usernameTextBox = (TextBox)EntryView.FindName("UsernameTextBox");

                return new Command(() =>
                {
                    string usernameInput = usernameTextBox.Text;

                    if (string.Equals(usernameInput, "Username"))
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
                TextBox usernameTextBox = (TextBox)EntryView.FindName("UsernameTextBox");

                return new Command(() =>
                {
                    string usernameInput = usernameTextBox.Text;

                    if (string.IsNullOrEmpty(usernameInput))
                    {
                        usernameTextBox.Foreground = Brushes.Gray;
                        usernameTextBox.Text = "Username";
                    }
                });
            }
        }

        public Command EnterPasswordTextboxCommand
        {
            get
            {
                TextBox passwordTextBox = (TextBox)EntryView.FindName("PasswordTextBox");
                PasswordBox passwordBox = (PasswordBox)EntryView.FindName("PasswordBox");

                return new Command(() =>
                {
                    passwordTextBox.Visibility = Visibility.Collapsed;
                    passwordBox.Focus();
                });
            }
        }

        public Command LeavePasswordTextboxCommand
        {
            get
            {
                TextBox passwordTextBox = (TextBox)EntryView.FindName("PasswordTextBox");
                PasswordBox passwordBox = (PasswordBox)EntryView.FindName("PasswordBox");

                return new Command(() =>
                {
                    if (string.IsNullOrEmpty(passwordBox.Password))
                        passwordTextBox.Visibility = Visibility.Visible;
                });
            }
        }

        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Keyboard.IsKeyDown(Key.Tab))
                        EnterPasswordTextboxCommand.Execute(null);
                });
            }
        }

        public Command SignInCommand
        {
            get
            {
                TextBox usernameTextBox = (TextBox)EntryView.FindName("UsernameTextBox");
                PasswordBox passwordBox = (PasswordBox)EntryView.FindName("PasswordBox");
                WrapPanel usernameErrorWrapPanel = (WrapPanel)EntryView.FindName("UsernameErrorWrapPanel"),
                    passwordErrorWrapPanel = (WrapPanel)EntryView.FindName("PasswordErrorWrapPanel");

                return new Command(() =>
                {
                    UserModel identifiedUser = null;
                    bool emptyUsername = (string.IsNullOrEmpty(usernameTextBox.Text)) || (usernameTextBox.Text == "Username");
                    bool emptyPassword = string.IsNullOrEmpty(passwordBox.Password);

                    UsernameError = string.Empty;
                    usernameErrorWrapPanel.Visibility = Visibility.Hidden;

                    PasswordError = string.Empty;
                    passwordErrorWrapPanel.Visibility = Visibility.Hidden;

                    // Validates input credentials were entered

                    if (emptyUsername || emptyPassword)
                    {
                        if (emptyUsername)
                        {
                            UsernameError = "Enter username";
                            usernameErrorWrapPanel.Visibility = Visibility.Visible;
                        }

                        if (emptyPassword)
                        {
                            PasswordError = "Enter password";
                            passwordErrorWrapPanel.Visibility = Visibility.Visible;
                        }

                        return;

                    }
                    // Validates the user's credentials
                    identifiedUser = FindUser(usernameTextBox.Text.Trim());
                    if (identifiedUser == null)
                        CheckAttempts();
                    else
                    {
                        if (string.IsNullOrEmpty(passwordBox.Password))
                        {
                            PasswordError = "Enter password";
                            passwordErrorWrapPanel.Visibility = Visibility.Visible;
                        }
                        else if (identifiedUser.Password.Equals(passwordBox.Password.Trim()))
                            OpenMainView(identifiedUser);
                        else
                            CheckAttempts();
                    }
                });
            }
        }

        public Command ContinueAsGuestCommand
        {
            get { return new Command(() => OpenMainView(new UserModel())); }
        }

        public Command KickUserCommand
        {
            get
            {
                TextBox usernameTextBox = (TextBox)EntryView.FindName("UsernameTextBox"),
                    passwordTextBox = (TextBox)EntryView.FindName("PasswordTextBox");
                PasswordBox passwordBox = (PasswordBox)EntryView.FindName("PasswordBox");
                MediaElement kickUserMediaElement = (MediaElement)EntryView.FindName("KickUserMediaElement");
                WrapPanel usernameErrorWrapPanel = (WrapPanel)EntryView.FindName("UsernameErrorWrapPanel"),
                    passwordErrorWrapPanel = (WrapPanel)EntryView.FindName("PasswordErrorWrapPanel");

                return new Command(() =>
                {
                    Timer videoTimer = new Timer(6000);

                    // Disables all controls
                    usernameTextBox.Clear();

                    passwordTextBox.Clear();
                    passwordBox.Clear();

                    usernameErrorWrapPanel.Visibility = passwordErrorWrapPanel.Visibility = Visibility.Hidden;

                    EnterPasswordTextboxCommand.Execute(null); // collapses passwordTextBox's visibility 

                    // Shows and activates the video 
                    videoTimer.Elapsed += VideoTimer_Elapsed;

                    kickUserMediaElement.Play();
                    videoTimer.Start();
                });
            }
        }
        private void VideoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        /// <summary>
        /// Initiates the configurations handler (if it hasn't been initiated).
        /// </summary>
        private void InitCongifurations()
        {
            if (CONFIGURATIONS.CONSTANTS == null)
                CONFIGURATIONS.Init();
        }

        /// <summary>
        /// Initiatess the MongoDB handler (if it hasn't been initiated).
        /// </summary>
        public void InitMongoDB()
        {
            // If the Mongo database is initiated for the first time
            if (MONGODB.DATABASE == null)
                MONGODB.Init();

            // If the users collection is loaded for the first time
            if (MONGODB.Reports == null)
                MONGODB.LoadReports();

            if (MONGODB.Screenplays == null)
                MONGODB.LoadScreenplays();

            if (MONGODB.Users == null)
                MONGODB.LoadUsers();
        }

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        public void Init()
        {
            TextBox usernameTextBox = null;
            MediaElement kickUserMediaElement = null;

            foreach (Window view in App.Current.Windows)
                if (view is EntryView)
                {
                    EntryView = (EntryView)view;
                    break;
                }

            UsernameError = string.Empty;
            PasswordError = string.Empty;
            attemptsCount = 3;
            CanSignIn = true;

            usernameTextBox = (TextBox)EntryView.FindName("UsernameTextBox");
            usernameTextBox.Foreground = Brushes.Gray;
            usernameTextBox.Text = "Username";

            kickUserMediaElement = (MediaElement)EntryView.FindName("KickUserMediaElement");
            kickUserMediaElement.Source = new Uri(FolderPaths.VIDEOS + "You Shall Not Pass.mp4");
        }

        /// <summary>
        /// Opens the MainView.
        /// </summary>
        /// <param name="user">The user authenticated to the system</param>
        private void OpenMainView(UserModel user)
        {
            App.Current.MainWindow = new MainView();

            ((MainViewModel)App.Current.MainWindow.DataContext).Init(user);
            EntryView.Close();

            App.Current.MainWindow.Show();
        }

        /// <summary>
        /// Searches a user by his username.
        /// </summary>
        /// <param name="username">The username to search by</param>
        /// <returns>The user (if exists), null otherwise</returns>
        private UserModel FindUser(string username)
        {
            foreach (UserModel user in MONGODB.Users)
                if (user.Username.Equals(username))
                    return user;

            return null;
        }

        /// <summary>
        /// Checks number of sign-in attempts and raises an error if the credentials do not match.
        /// </summary>
        private void CheckAttempts()
        {
            bool pluralCondition;

            if (--AttemptsCount == 0)
            {
                CanSignIn = false;
                KickUserCommand.Execute(null);
            }
            else
            {
                pluralCondition = AttemptsCount == 1;
                Utilities.MessageBox.ShowError(string.Format("Wrong username or password.\n{0} attempt{1} remain{2}.",
                AttemptsCount, pluralCondition ? string.Empty : "s", pluralCondition ? "s" : string.Empty));
            }
        }
    }
}