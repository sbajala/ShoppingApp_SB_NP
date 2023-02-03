using Isi.ShoppingApp.Core.Entities;
using Isi.ShoppingApp.Domain.Services;
using Isi.Utility.Authentication;
using Isi.Utility.ViewModels;
using System.Linq;
using System.Windows;

//SHARMAINE
namespace Isi.ShoppingApp.Presentation.ViewModels
{
    public delegate void SignUpSucceededHandler(string message);
    public delegate void SignUpFailedHandler(string message);

    class SignUpViewModel : ViewModel
    {
        UserService userService;
        public DelegateCommand SignUpCommand { get; }

        public event SignUpSucceededHandler SignUpSucceeded;
        public event SignUpFailedHandler SignUpFailed;

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                if (IsInputValid(value))
                {
                    firstName = value;
                    NotifyPropertyChanged(nameof(FirstName));
                    SignUpCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                if (IsInputValid(value))
                {
                    lastName = value;
                    NotifyPropertyChanged(nameof(LastName));
                    SignUpCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string FullName
        {
            get => firstName + " " + lastName;
        }

        private string username;
        public string Username
        {
            get => username;
            set
            {
                if (IsInputValid(value))
                {
                    username = value;
                    NotifyPropertyChanged(nameof(Username));
                    SignUpCommand.NotifyCanExecuteChanged();
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


        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
            }
        }

        private decimal balance;
        public decimal Balance
        {
            get => balance;
            set
            {
                if (value > 0)
                    balance = value;
            }
        }

        public SignUpViewModel()
        {
            userService = new UserService();
            SignUpCommand = new DelegateCommand(SignUp, CanSignUp);
        }

        private bool CanSignUp(object _)
        {
            return IsNameValid(firstName)
                && IsNameValid(lastName)
                && IsUsernameValid(username)
                && hashedPassword != null;
        }

        private void SignUp(object _)
        {

            if (CanSignUp(_))
            {
               userService.AddUser(new User(FirstName, LastName, Username, HashedPassword, IsAdmin, Balance));
               SignUpSucceeded?.Invoke("Hurray! Your account has been successfully created.");
                ClearFieldProperties();
            }
            SignUpFailed?.Invoke("Oops! Could not successfully sign up");
        }
        
        private void ClearFieldProperties()
        {
            FirstName = "";
            LastName = "";
            Username = "";
            HashedPassword = null;
        }

        private bool IsInputValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        private bool IsNameValid(string name)
        {
            return IsInputValid(name)
                && !ContainsPunctuation(name)
                && !ContainsNumbers(name);
        }

        private bool IsUsernameValid(string username)
        {
            return IsInputValid(username)
                && !ContainsPunctuation(username)
                && !ContainsUppercase(username)
                && !username.Contains(" ");
        }

        private bool ContainsPunctuation(string input)
        {
            bool containsPunctuation = false;

            for(int i = 0; i < input.Length; i++)
            {
                if (input.Any(char.IsPunctuation))
                {
                    containsPunctuation = true;
                    break;
                }
          
            }
            return containsPunctuation;
        }

        private bool ContainsUppercase(string input)
        {
            bool containsUppercase = false;

            for (int i = 0; i < input.Length; i++)
            {
                if (input.Any(char.IsUpper))
                {
                    containsUppercase = true;
                    break;
                }

            }
            return containsUppercase;
        }

        private bool ContainsNumbers(string input)
        {
            bool containsNumbers = false;

            for (int i = 0; i < input.Length; i++)
            {
                if (input.Any(char.IsDigit))
                {
                    containsNumbers = true;
                    break;
                }
            }
            return containsNumbers;
        }
    }
}
