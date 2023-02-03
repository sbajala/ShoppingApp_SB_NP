using Isi.ShoppingApp.Core.Entities;
using Isi.Utility.Authentication;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

//SHARMAINE
namespace Isi.ShoppingApp.Data.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString;
        public UserRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["UserDatabase"].ConnectionString;
        }

        public List<User> GetUsers()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

           using SqlCommand command = connection.CreateCommand();
           command.CommandText = "SELECT FirstName, LastName, Username, PasswordSalt, PasswordHash, Admin, Balance " +
                                 "FROM dbo.Users";

            List<User> users = new List<User>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(ReadNextUser(reader));
            }
            return users;
        }

        private User ReadNextUser(SqlDataReader reader)
        {
            string firstName = reader.GetString(0);
            string lastName = reader.GetString(1);
            string username = reader.GetString(2);
            byte[] passwordSalt = (byte[])reader.GetValue(3);
            byte[] passwordHash = (byte[])reader.GetValue(4);
            bool isAdmin = reader.GetBoolean(5);
            decimal balance = reader.GetDecimal(6);

            return new User(firstName, lastName, username, new HashedPassword(passwordSalt, passwordHash), isAdmin, balance);
        }

        public bool UserExist(string username)
        {
            return (GetUser(username) != null);
        }

        public User GetUser(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

           using SqlCommand command = connection.CreateCommand();
           command.CommandText = "SELECT FirstName, LastName, Username, PasswordSalt, PasswordHash, Admin, Balance " +
                                 "FROM dbo.Users " +
                                 "WHERE Username = @Username";

            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                return ReadNextUser(reader);

            return null;
        }

        public HashedPassword GetUserPassword(string username)
        {
            return new HashedPassword(GetPasswordSalt(username), GetPasswordHash(username));
        }

        private byte[] GetPasswordSalt(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT PasswordSalt FROM dbo.Users WHERE Username = @Username";

            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                return (byte[])reader.GetValue("PasswordSalt");

            return null;
        }

        private byte[] GetPasswordHash(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT PasswordHash FROM dbo.Users WHERE Username = @Username";

            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                return (byte[])reader.GetValue("PasswordHash"); ;

            return null;
        }

        public User CreateUser(User user)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO dbo.Users(FirstName, LastName, Username, PasswordSalt, PasswordHash, Admin, Balance) " +
                "VALUES(@FirstName, @LastName, @Username, @PasswordSalt, @PasswordHash, @Admin, @Balance)";

            command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = user.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = user.LastName;
            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = user.Username;
            command.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary).Value = user.HashedPassword.Salt;
            command.Parameters.Add("@PasswordHash", SqlDbType.VarBinary).Value = user.HashedPassword.Hash;
            command.Parameters.Add("@Admin", SqlDbType.Bit).Value = user.IsAdmin;
            command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = user.Balance;

            command.ExecuteNonQuery();
            return new User(user.FirstName, user.LastName, user.Username, user.HashedPassword, user.IsAdmin, user.Balance);
        }

        public bool DeleteUser(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM dbo.Users WHERE Username = @Username";

            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

            int rowsChanged = command.ExecuteNonQuery();
            return rowsChanged > 0;
        }

        public bool UpdateUser(User user)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE dbo.Users SET FirstName = @FirstName, LastName = @LastName, Username = @Username, " +
                                  "PasswordSalt = @PasswordSalt, PasswordHash = @PasswordHash, Admin = @Admin, Balance = @Balance";

            command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = user.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = user.LastName;
            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = user.Username;
            command.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary).Value = user.HashedPassword.Salt;
            command.Parameters.Add("@PasswordHash", SqlDbType.VarBinary).Value = user.HashedPassword.Hash;
            command.Parameters.Add("@Admin", SqlDbType.Bit).Value = user.IsAdmin;
            command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = user.Balance;

            int rowsChanges = command.ExecuteNonQuery();
            return rowsChanges > 0;
        }

    }
}
