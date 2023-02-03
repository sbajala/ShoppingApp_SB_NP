using System;

namespace Isi.Utility.Authentication
{
    [Serializable]
    public class HashedPassword
    {
        public byte[] Salt { get; }
        public byte[] Hash { get; }

        public HashedPassword(byte[] salt, byte[] hash)
        {
            Salt = salt;
            Hash = hash;
        }
    }
}
