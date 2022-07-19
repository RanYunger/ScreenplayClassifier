using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class SignInViewModel
    {
        // Properties
        public List<UserModel> membersList { get; private set; }
        public string UsernameText { get; private set; }
        public string PasswordText { get; private set; }
        public int AttemptsCount { get; private set; }

        // Constructors
        public SignInViewModel()
        {
            membersList = new List<UserModel>();

            membersList.Add(new UserModel("RanY", UserModel.UserRole.ADMIN, "bla1"));
            membersList.Add(new UserModel("ShyOZ", UserModel.UserRole.ADMIN, "bla2"));
        }

        // Methods
        #region Commands
        public Command SignInCommand
        {
            get
            {
                SignInView signInView = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);

                return new Command(() =>
                {
                    UserModel identifiedUser = membersList.Find(u => u.Username.Trim().Equals(UsernameText)
                        && u.Password.Trim().Equals(PasswordText));

                    if (identifiedUser != null)
                    {
                        new MainView(identifiedUser).Show();
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
                SignInView signInView = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);

                return new Command(() =>
                {
                    new MainView(new UserModel()).Show();
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
                Button signInButton = null, continueAsGuestButton = null, openCommandShellButton = null;

                App.Current.Dispatcher.Invoke(() => signInView = (SignInView)App.Current.MainWindow);
                App.Current.Dispatcher.Invoke(() => welcomeImage = (Image)signInView.FindName("WelcomeImage"));
                App.Current.Dispatcher.Invoke(() => kickUserMediaElement = (MediaElement)signInView.FindName("KickUserMediaElement"));
                App.Current.Dispatcher.Invoke(() => usernameTextBox = (TextBox)signInView.FindName("UsernameTextBox"));
                App.Current.Dispatcher.Invoke(() => passwordBox = (PasswordBox)signInView.FindName("PasswordBox"));
                App.Current.Dispatcher.Invoke(() => signInButton = (Button)signInView.FindName("SignInButton"));
                App.Current.Dispatcher.Invoke(() => continueAsGuestButton = (Button)signInView.FindName("ContinueAsGuestButton"));
                App.Current.Dispatcher.Invoke(() => openCommandShellButton = (Button)signInView.FindName("OpenCommandShellButton"));

                return new Command(() =>
                {
                    welcomeImage.Visibility = Visibility.Collapsed;
                    usernameTextBox.IsEnabled = passwordBox.IsEnabled = false;
                    signInButton.IsEnabled = continueAsGuestButton.IsEnabled = openCommandShellButton.IsEnabled = false;

                    kickUserMediaElement.Visibility = Visibility.Visible;
                    kickUserMediaElement.Source = new Uri(Environment.CurrentDirectory + @"\Media\Videos\You Shall Not Pass.mp4");
                    kickUserMediaElement.Play();

                    // TODO: FIX (Close the view as soon as video ends playing)
                    /*new Thread(() =>
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            while (!kickUserMediaElement.NaturalDuration.HasTimeSpan) ;
                            while (kickUserMediaElement.Position != kickUserMediaElement.NaturalDuration.TimeSpan) ;
                            signInView.Close();
                        });
                    }).Start();*/
                });
            }
        }
        #endregion
    }
}