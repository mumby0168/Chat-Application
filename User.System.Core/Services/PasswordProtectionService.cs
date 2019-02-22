using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using User.System.Core.Services.Interfaces;

namespace User.System.Core.Services
{
    public class PasswordProtectionService : IPasswordProtectionService
    {
        /// <summary>
        /// Returns an encrypted password and salt in the tuple order (password, salt).
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public Tuple<byte[], byte[]> Encrypt(string password)
        {
            var salt = GenerateSalt();

            var rfc2 = new Rfc2898DeriveBytes(password, salt, 10);

            return new Tuple<byte[], byte[]>(rfc2.GetBytes(32), salt);
        }

        public bool Check(string passwordEntered, byte[] passwordToCheck, byte[] salt)
        {
            var provider = new Rfc2898DeriveBytes(passwordEntered, salt, 10);

            var one = GetString(provider.GetBytes(32));

            var two = GetString(passwordToCheck);

            return one == two;
        }

        private string GetString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        private byte[] GenerateSalt()
        {
            var salt = new byte[32];

            var provider = new RNGCryptoServiceProvider();

            provider.GetBytes(salt);

            return salt;
        }
    }
}
