using System;
using System.Security.Cryptography;
using System.Text;

namespace Isi.Utility.Authentication
{
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes the given password with a randomly generated salt.
        /// </summary>
        /// <param name="password"> Must not be null. </param>
        /// <returns>
        /// An object containing the random salt byte array and the computed hash byte array.
        /// </returns>
        /// <exception cref="ArgumentNullException"> If <paramref name="password"/> is null. </exception>
        public static HashedPassword HashPassword(string password)
        {
            ValidatePasswordArgument(password);
            return HashPassword(ConvertToByteArray(password), GenerateRandomSalt(length: 32));
        }

        /// <summary>
        /// Checks whether the given password correctly matches the stored password. <br/>
        /// This is done by hashing the given password with the stored salt, and comparing the resulting hash with the stored hash. <br/>
        /// If the resulting hash is equal to the stored hash, then the password is correct. Otherwise, it is incorrect.
        /// </summary>
        /// <param name="password"> Must not be null. </param>
        /// <param name="storedPassword"> Must not be null, and its salt and hash byte arrays must not be null. </param>
        /// <returns> A password result value specifying whether the given password is correct or incorrect. </returns>
        /// <exception cref="ArgumentNullException"> If <paramref name="password"/> is null. </exception>
        /// <exception cref="ArgumentNullException"> If <paramref name="storedPassword"/> is null. </exception>
        /// <exception cref="ArgumentException"> If <paramref name="storedPassword"/> has a null salt byte array. </exception>
        /// <exception cref="ArgumentException"> If <paramref name="storedPassword"/> has a null hash byte array. </exception>
        public static PasswordResult CheckPassword(string password, HashedPassword storedPassword)
        {
            ValidatePasswordArgument(password);
            ValidateStoredPasswordArgument(storedPassword);

            byte[] passwordByteArray = ConvertToByteArray(password);
            HashedPassword hashedPassword = HashPassword(passwordByteArray, storedPassword.Salt);

            if (ByteArraysAreEqual(hashedPassword.Hash, storedPassword.Hash))
                return PasswordResult.Correct;
            return PasswordResult.Incorrect;
        }

        private static void ValidatePasswordArgument(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password), "Password must not be null.");
        }

        private static void ValidateStoredPasswordArgument(HashedPassword storedPassword)
        {
            if (storedPassword == null)
                throw new ArgumentNullException(nameof(storedPassword), "Hashed password must not be null.");
            if (storedPassword.Salt == null)
                throw new ArgumentException("Salt byte array in hashed password must not be null.", nameof(storedPassword));
            if (storedPassword.Hash == null)
                throw new ArgumentException("Hash byte array in hashed password must not be null.", nameof(storedPassword));
        }

        private static byte[] ConvertToByteArray(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        private static byte[] GenerateRandomSalt(int length)
        {
            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must not be less than 1.");

            byte[] salt = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(salt);

            return salt;
        }

        private static HashedPassword HashPassword(byte[] password, byte[] salt)
        {
            byte[] saltedPassword = new byte[password.Length + salt.Length];

            for (int i = 0; i < password.Length; i++)
                saltedPassword[i] = password[i];
            for (int i = 0; i < salt.Length; i++)
                saltedPassword[password.Length + i] = salt[i];
            
            return new HashedPassword(salt, new SHA256Managed().ComputeHash(saltedPassword));
        }

        private static bool ByteArraysAreEqual(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                return false;

            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    return false;
            }

            return true;
        }
    }
}
