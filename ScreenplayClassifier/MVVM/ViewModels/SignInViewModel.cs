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
        // Constants
        public string usernamePattern = "([A-Z]{1}[a-z]+){2,3}"; // E.G. RanYunger, ShyOhevZion
        public string passwordPattern = "[A-Z]{2,3}[0-9]{5,6}"; // E.G. RY120696, SHZ101098

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
                    Regex usernameRegex = new Regex(usernamePattern), passwordRegex = new Regex(passwordPattern);
                    UserModel identifiedUser = null;

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

                    identifiedUser = FindUser(UsernameInput.Trim(), passwordBox.Password.Trim());
                    if (identifiedUser != null)
                    {
                        App.Current.MainWindow = new MainView();
                        ((MainViewModel)App.Current.MainWindow.DataContext).Init(identifiedUser, AuthenticatedUsers);
                        App.Current.MainWindow.Show();

                        SignInView.Close();
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
            get
            {
                return new Command(() =>
                {
                    App.Current.MainWindow = new MainView();

                    ((MainViewModel)App.Current.MainWindow.DataContext).Init(new UserModel(), AuthenticatedUsers);
                    App.Current.MainWindow.Show();

                    SignInView.Close();
                });
            }
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
                    Timer videoTimer = new Timer(5800);

                    UsernameInput = string.Empty;
                    passwordBox.Clear();

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

        public void Init()
        {
            foreach (Window view in App.Current.Windows)
                if (view is SignInView)
                {
                    SignInView = (SignInView)view;
                    break;
                }

            AuthenticatedUsers = Storage.LoadUsers();
            UsernameInput = UsernameError = PasswordError = string.Empty;

            CanSignin = true;
        }

        public UserModel FindUser(string username, string password)
        {
            foreach (UserModel user in AuthenticatedUsers)
                if (user.Username.Equals(username) && user.Password.Equals(password))
                    return user;

            return null;
        }
    }
}