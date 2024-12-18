using StoreManagement.Core.Interfaces.Cryptography;
using System.Security.Cryptography;

namespace StoreManagement.Services.Cryptography
{
    public class PasswordHelper : IPasswordHelper
    {
        public (string hashedPassword, string salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
                {
                    var hash = pbkdf2.GetBytes(32);
                    var hashedPassword = Convert.ToBase64String(hash);
                    var saltBase64 = Convert.ToBase64String(salt);

                    return (hashedPassword, saltBase64);
                }
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var salt = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000))
            {
                var enteredHash = pbkdf2.GetBytes(32);
                var enteredHashBase64 = Convert.ToBase64String(enteredHash);
                return enteredHashBase64 == storedHash;
            }
        }
    }
}
