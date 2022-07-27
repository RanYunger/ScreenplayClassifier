using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        // Constants
        public string usernamePattern = "([A-Z]{1}[a-z]+){2,3}";
        public string passwordPattern = "[A-Z]{2,3}[0-9]{5,6}";

        // Fields
        private ObservableCollection<UserModel> members;
        private string usernameInput;
        private int attemptsCount;
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public ObservableCollection<UserModel> Members
        {
            get { return members; }
            set
            {
                members = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Members"));
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

        // Constructors
        public SignInViewModel()
        {
            Members = new ObservableCollection<UserModel>();
            UsernameInput = string.Empty;

            Members.Add(new UserModel("RanYunger", UserModel.UserRole.ADMIN, "RY120696"));
            Members.Add(new UserModel("ShyOhevZion", UserModel.UserRole.MEMBER, "SHZ12345"));
        }

        // Methods
        #region Commands
        public Command SignInCommand
        {
            get
            {
                SignInView signInView = null;
                PasswordBox passwordBox = null;
                WrapPanel usernameErrorWrapPanel = null, passwordErrorWrapPanel = null;
                TextBlock usernameErrorTextBlock = null, passwordErrorTextBlock = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => passwordBox = (PasswordBox)signInView.FindName("PasswordBox"));

                App.Current.Dispatcher.Invoke(() => usernameErrorWrapPanel = (WrapPanel)signInView.FindName("UsernameErrorWrapPanel"));
                App.Current.Dispatcher.Invoke(() => passwordErrorWrapPanel = (WrapPanel)signInView.FindName("PasswordErrorWrapPanel"));
                App.Current.Dispatcher.Invoke(() => usernameErrorTextBlock = (TextBlock)usernameErrorWrapPanel.FindName("UsernameErrorTextBlock"));
                App.Current.Dispatcher.Invoke(() => passwordErrorTextBlock = (TextBlock)passwordErrorWrapPanel.FindName("PasswordErrorTextBlock"));

                return new Command(() =>
                {
                    Regex usernameRegex = new Regex(usernamePattern), passwordRegex = new Regex(passwordPattern);
                    UserModel identifiedUser;

                    usernameErrorWrapPanel.Visibility = Visibility.Hidden;
                    if ((UsernameInput == null) || (UsernameInput.Trim().Equals(string.Empty)))
                    {
                        usernameErrorTextBlock.Text = "Enter username";
                        usernameErrorWrapPanel.Visibility = Visibility.Visible;
                    }
                    else if (!usernameRegex.IsMatch(UsernameInput))
                    {
                        usernameErrorTextBlock.Text = "Invalid username";
                        usernameErrorWrapPanel.Visibility = Visibility.Visible;
                    }

                    passwordErrorWrapPanel.Visibility = Visibility.Hidden;
                    if ((passwordBox.Password == null) || (passwordBox.Password.Trim().Equals(string.Empty)))
                    {
                        passwordErrorTextBlock.Text = "Enter password";
                        passwordErrorWrapPanel.Visibility = Visibility.Visible;
                    }
                    else if (!passwordRegex.IsMatch(passwordBox.Password))
                    {
                        passwordErrorTextBlock.Text = "Invalid password";
                        passwordErrorWrapPanel.Visibility = Visibility.Visible;
                    }

                    identifiedUser = FindUser(UsernameInput.Trim(), passwordBox.Password.Trim());
                    if (identifiedUser != null)
                    {
                        App.Current.MainWindow = new MainView();
                        App.Current.Dispatcher.Invoke(() => ((MainViewModel)App.Current.MainWindow.DataContext).Init(identifiedUser));
                        App.Current.MainWindow.Show();

                        signInView.Close();
                    }
                    else if (++AttemptsCount == 3)
                        KickUserCommand.Execute(null);
                });
            }
        }
        public Command ContinueAsGuestCommand
        {
            get
            {
                MainView mainView = new MainView();
                SignInView signInView = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);

                return new Command(() =>
                {
                    App.Current.MainWindow = mainView;
                    App.Current.Dispatcher.Invoke(() => ((MainViewModel)mainView.DataContext).Init(new UserModel()));
                    App.Current.MainWindow.Show();

                    signInView.Close();
                });
            }
        }
        public Command OpenCommandShellCommand
        {

            get
            {
                SignInView signInView = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);

                return new Command(() =>
                {
                    // TODO: COMPLETE (Open the command shell version of ScreenplayClassifier)
                    signInView.Close();
                });
            }
        }
        public Command KickUserCommand
        {
            get
            {
                SignInView signInView = null;
                Image welcomeImage = null;
                MediaElement kickUserMediaElement = null;
                TextBox usernameTextBox = null;
                PasswordBox passwordBox = null;
                WrapPanel usernameErrorWrapPanel = null, passwordErrorWrapPanel = null;
                Button signInButton = null, continueAsGuestButton = null, openCommandShellButton = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => welcomeImage = (Image)signInView.FindName("WelcomeImage"));
                App.Current.Dispatcher.Invoke(() => kickUserMediaElement = (MediaElement)signInView.FindName("KickUserMediaElement"));

                App.Current.Dispatcher.Invoke(() => usernameTextBox = (TextBox)signInView.FindName("UsernameTextBox"));
                App.Current.Dispatcher.Invoke(() => passwordBox = (PasswordBox)signInView.FindName("PasswordBox"));
                App.Current.Dispatcher.Invoke(() => usernameErrorWrapPanel = (WrapPanel)signInView.FindName("UsernameErrorWrapPanel"));
                App.Current.Dispatcher.Invoke(() => passwordErrorWrapPanel = (WrapPanel)signInView.FindName("PasswordErrorWrapPanel"));
                App.Current.Dispatcher.Invoke(() => signInButton = (Button)signInView.FindName("SignInButton"));
                App.Current.Dispatcher.Invoke(() => continueAsGuestButton = (Button)signInView.FindName("ContinueAsGuestButton"));
                App.Current.Dispatcher.Invoke(() => openCommandShellButton = (Button)signInView.FindName("OpenCommandShellButton"));

                return new Command(() =>
                {
                    System.Timers.Timer videoTimer = new System.Timers.Timer(6000);

                    welcomeImage.Visibility = Visibility.Collapsed;
                    usernameTextBox.IsEnabled = passwordBox.IsEnabled = false;
                    usernameErrorWrapPanel.Visibility = passwordErrorWrapPanel.Visibility = Visibility.Hidden;
                    signInButton.IsEnabled = continueAsGuestButton.IsEnabled = openCommandShellButton.IsEnabled = false;

                    kickUserMediaElement.Visibility = Visibility.Visible;
                    kickUserMediaElement.Source = new Uri(Environment.CurrentDirectory + @"\Media\Videos\You Shall Not Pass.mp4");
                    kickUserMediaElement.Play();

                    videoTimer.Elapsed += VideoTimer_Elapsed;
                    videoTimer.Start();
                });
            }
        }
        private void VideoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => App.Current.MainWindow.Close());
            App.Current.Dispatcher.Invoke(() => Environment.Exit(0));
        }
        #endregion

        public UserModel FindUser(string username, string password)
        {
            foreach (UserModel user in members)
                if (user.Username.Equals(username) && user.Password.Equals(password))
                    return user;

            return null;
        }
    }
}