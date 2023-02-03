using Isi.Utility.Authentication;


//SHARMAINE
namespace Isi.ShoppingApp.Core.Entities
{
    public class User
    {
        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                if (IsStringValid(value))
                    firstName = value;
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                if (IsStringValid(value))
                    lastName = value;
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
            private set
            {
                if (IsStringValid(value))
                    username = value;
            }

        }

        private HashedPassword hashedPassword;
        public HashedPassword HashedPassword
        {
            get => hashedPassword;
            private set
            {
                if (value != null)
                    hashedPassword = value;
            }
        }

        private decimal balance;
        public decimal Balance
        {
            get => balance;
            private set
            {
                if (value > 0)
                    balance = value;
            }
        }


        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            private set
            {
                isAdmin = value;
            }
        }


       public User(string firstName, string lastName, string username, HashedPassword hashedPassword, bool isAdmin, decimal balance)
       {
           FirstName = firstName;
           LastName = lastName;
           Username = username;
           HashedPassword = hashedPassword;
           IsAdmin = isAdmin;
           Balance = balance;
       
       }

        private bool IsStringValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public void AddAmountToBalance(decimal amount)
        {
            if (amount > 0)
                Balance += amount;
        }

        public void SubtractAmountFromBalance(decimal amount)
        {
            if (amount <= Balance)
                Balance -= amount;
        }

        public override string ToString()
        {
            return $"User: {FullName}, Username: {Username}, IsAdmin: {IsAdmin}, Balance: {Balance}";
        }

    }
}
