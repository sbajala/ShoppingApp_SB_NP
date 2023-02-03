using System;

namespace Isi.Utility.Authentication
{
    public interface IPasswordProvider
    {
        event Action PasswordChanged;

        string Password { get; }
        bool HasPassword { get; }

        void Clear();
    }
}
