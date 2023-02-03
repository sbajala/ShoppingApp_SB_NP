using Isi.Utility.ViewModels;
using Isi.ShoppingApp.Core.Entities;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Isi.Utility.Authentication;
using System.Collections.ObjectModel;
using Isi.ShoppingApp.Presentation.Views;
using Isi.ShoppingApp.Domain.Services;

//SHARMAINE
namespace Isi.ShoppingApp.Presentation.ViewModels
{
    public delegate void LoginSucceededHandler();
    public delegate void LoginFailedHandler(string message);
    class LoginViewModel : ViewModel
    {
        public UserService userService;
        public DelegateCommand LoginCommand { get; }
        public ObservableCollection<User> Users { get; }

        public event LoginSucceededHandler LoginSucceeded;
        public event LoginFailedHandler FailedLogin;

        private string username;
        public string Username
        {
            get => username;
            set
            {
                if (IsInputValid(value))
                {
                    username = value;
                    LoginCommand.NotifyCanExecuteChanged();
                    NotifyPropertyChanged(nameof(Username));
                }
            }
        }


        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (IsInputValid(value))
                {
                    password = value;
                    NotifyPropertyChanged(nameof(Password));
                }
            }

        }

        private HashedPassword hashedPassword;
        public HashedPassword HashedPassword
        {
            get => hashedPassword;
            set
            {
                if (value != null)
                {
                    hashedPassword = value;
                    NotifyPropertyChanged(nameof(HashedPassword));
                }
            }

        }

        public bool IsAdmin { get; }

        private bool loggedIn;
        public bool IsLoggedIn
        {
            get => loggedIn;
            set
            {
                loggedIn = value;
            }
        }

        public bool IsLoggedOut
        {
            get => !loggedIn;
        }

        public LoginViewModel()
        {
            userService = new UserService();
            LoginCommand = new DelegateCommand(LogIn, CanLogIn);
        }

        private bool CanLogIn(object _)
        {
            return IsInputValid(username)
                && IsInputValid(password);
        }

        private void LogIn(object _)
        {
            if (CanLogIn(_))
            { 
                if (IsUsernameExistant(Username))
                {
                    HashedPassword = GetHashedPassword(Username);
                    if (ValidateLogin(Password, HashedPassword))
                    {
                        IsLoggedIn = true;
                        LoginSucceeded?.Invoke();
                    }
                    else
                    {
                        FailedLogin?.Invoke("Password is incorrect.");
                    }
                }
                else
                {
                    FailedLogin?.Invoke("Username does not exist.");
                }
            }
        }

        private bool IsInputValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        private bool IsUsernameExistant(string username)
        {
            return userService.UserExist(username);
        }

        private HashedPassword GetHashedPassword(string username)
        {
            return HashedPassword = userService.GetUserPassword(username);
        }

       private bool ValidateLogin(string password, HashedPassword hashedPassword)
       {
            PasswordResult result = PasswordHasher.CheckPassword(password, hashedPassword);
            return result == PasswordResult.Correct;
       }
    }
}
