using Isi.ShoppingApp.Core.Entities;
using Isi.ShoppingApp.Data.Repositories;
using Isi.Utility.Authentication;
using Isi.Utility.ViewModels;
using System.Collections.Generic;
using System.Windows;

//SHARMAINE
namespace Isi.ShoppingApp.Domain.Services
{
    public class UserService : ViewModel
    {
        UserRepository repository;
        public UserService()
        {
            repository = new UserRepository();
        }

        public bool UserExist(string username)
        {
            if (IsStringValid(username))
                return repository.UserExist(username);

            return false;
        }

        public List<User> GetUsers()
        {
            return repository.GetUsers();
        }

        public User GetUser(string username)
        {
            if(IsStringValid(username))
                return repository.GetUser(username);

            return null;
        }

        public HashedPassword GetUserPassword(string username)
        {
            if(IsStringValid(username))
                return repository.GetUserPassword(username);

            return null;
        }

        public User AddUser(User user)
        {
            if(user != null)
                return repository.CreateUser(user);

            return null;
        }

        public bool DeleteUser(User user)
        {
            if(repository.UserExist(user.Username))
                return repository.DeleteUser(user.Username);

            return false;
        }

        public bool AddToBalance(User user, decimal amount)
        {
            if(repository.UserExist(user.Username) && amount > 0)
            {
                user.AddAmountToBalance(amount);
                return repository.UpdateUser(user);
            }
            return false;
        }

        public bool SubtractFromBalance(User user, decimal amount)
        {
            if (repository.UserExist(user.Username) && amount <= user.Balance)
            {
                user.SubtractAmountFromBalance(amount);
                return repository.UpdateUser(user);
            }
            return false;
        }

        private bool IsStringValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}