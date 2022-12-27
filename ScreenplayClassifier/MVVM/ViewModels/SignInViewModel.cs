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

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<UserModel> authenticatedUsers;
        private string usernameInput, usernameError, passwordError;
        private int attemptsCount;
        private bool canSignin;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public SignInView SignInView { get; private set; }

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

        public string UsernameInput
        {
            get { return usernameInput; }
            set
            {
                usernameInput = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UsernameInput"));
            }
        }

        public string UsernameError
        {
            get { return usernameError; }
            set
            {
                usernameError = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UsernameError"));
            }
        }

        public string PasswordError
        {
            get { return passwordError; }
            set
            {
                passwordError = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PasswordError"));
            }
        }

        public int AttemptsCount
        {
            get { return attemptsCount; }
            set
            {
                attemptsCount = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AttemptsCount"));
            }
        }

        public bool CanSignin
        {
            get { return canSignin; }
            set
            {
                canSignin = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanSignin"));
            }
        }

        // Constructors
        public SignInViewModel()
        {
            Init();
        }

        // Methods
        #region Commands
        public Command SignInCommand
        {
            get
            {
                PasswordBox passwordBox = (PasswordBox)SignInView.FindName("PasswordBox");
                WrapPanel usernameErrorWrapPanel = (WrapPanel)SignInView.FindName("UsernameErrorWrapPanel"),
                    passwordErrorWrapPanel = (WrapPanel)SignInView.FindName("PasswordErrorWrapPanel");

                return new Command(() =>
                {
                    Regex usernameRegex = new Regex(JSON.USERNAMEPATTERN), passwordRegex = new Regex(JSON.PASSWORDPATTERN);
                    UserModel identifiedUser = null;

                    // Validates the input username
                    usernameErrorWrapPanel.Visibility = Visibility.Hidden;
                    if (string.IsNullOrEmpty(UsernameInput))
                    {
                        UsernameError = "Enter username";
                        usernameErrorWrapPanel.Visibility = Visibility.Visible;
                    }
                    else if (!usernameRegex.IsMatch(UsernameInput))
                    {
                        UsernameError = "Invalid username";
                        usernameErrorWrapPanel.Visibility = Visibility.Visible;
                    }

                    // Validates the input password
                    passwordErrorWrapPanel.Visibility = Visibility.Hidden;
                    if (string.IsNullOrEmpty(passwordBox.Password))
                    {
                        PasswordError = "Enter password";
                        passwordErrorWrapPanel.Visibility = Visibility.Visible;
                    }
                    else if (!passwordRegex.IsMatch(passwordBox.Password))
                    {
                        PasswordError = "Invalid password";
                        passwordErrorWrapPanel.Visibility = Visibility.Visible;
                    }

                    identifiedUser = FindUser(UsernameInput.Trim());
                    if (identifiedUser != null)
                    {
                        if (!identifiedUser.Password.Equals(passwordBox.Password.Trim()))
                        {
                            PasswordError = "Wrong password";
                            passwordErrorWrapPanel.Visibility = Visibility.Visible;
                        }
                        else
                            OpenMainView(identifiedUser);
                    }
                    else if (++AttemptsCount == 3)
                    {
                        CanSignin = false;
                        KickUserCommand.Execute(null);
                    }
                });
            }
        }

        public Command ContinueAsGuestCommand
        {
            get { return new Command(() => OpenMainView(new UserModel())); }
        }

        public Command OpenCommandShellCommand
        {

            get
            {
                return new Command(() =>
                {
                    // TODO: COMPLETE (Open the command shell version of ScreenplayClassifier)
                    SignInView.Close();
                });
            }
        }

        public Command KickUserCommand
        {
            get
            {
                MediaElement kickUserMediaElement = (MediaElement)SignInView.FindName("KickUserMediaElement");
                PasswordBox passwordBox = passwordBox = (PasswordBox)SignInView.FindName("PasswordBox");
                WrapPanel usernameErrorWrapPanel = (WrapPanel)SignInView.FindName("UsernameErrorWrapPanel"),
                    passwordErrorWrapPanel = (WrapPanel)SignInView.FindName("PasswordErrorWrapPanel");

                return new Command(() =>
                {
                    Timer videoTimer = new Timer(6000);

                    UsernameInput = string.Empty;
                    passwordBox.Clear();

                    // Shows and activates the video 
                    usernameErrorWrapPanel.Visibility = passwordErrorWrapPanel.Visibility = Visibility.Hidden;
                    kickUserMediaElement.Source = new Uri(FolderPaths.VIDEOS + "You Shall Not Pass.mp4");
                    kickUserMediaElement.Play();

                    videoTimer.Elapsed += VideoTimer_Elapsed;
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
        /// Initiates the view model.
        /// </summary>
        public void Init()
        {
            foreach (Window view in App.Current.Windows)
                if (view is SignInView)
                {
                    SignInView = (SignInView)view;
                    break;
                }

            if (JSON.LoadedUsers == null)
                JSON.LoadUsers();
            AuthenticatedUsers = new ObservableCollection<UserModel>(JSON.LoadedUsers);

            UsernameInput = string.Empty;
            UsernameError = string.Empty;
            PasswordError = string.Empty;

            CanSignin = true;
        }

        /// <summary>
        /// Opens the MainView.
        /// </summary>
        /// <param name="user">The user authenticated to the system</param>
        private void OpenMainView(UserModel user)
        {
            App.Current.MainWindow = new MainView();

            ((MainViewModel)App.Current.MainWindow.DataContext).Init(user, AuthenticatedUsers);
            SignInView.Close();

            App.Current.MainWindow.Show();
        }
        
        /// <summary>
        /// Searches a user by his username.
        /// </summary>
        /// <param name="username">The username to search by</param>
        /// <returns>The user (if exists), null otherwise</returns>
        private UserModel FindUser(string username)
        {
            foreach (UserModel user in AuthenticatedUsers)
                if (user.Username.Equals(username))
                    return user;

            return null;
        }
    }
}