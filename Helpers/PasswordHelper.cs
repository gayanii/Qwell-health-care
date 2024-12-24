using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Helpers
{
    public class PasswordHelper
    {
        public static string GenerateSalt()
        {
            var saltBytes = new byte[16]; // 16 bytes = 128 bits, a common salt size
            RandomNumberGenerator.Fill(saltBytes); // Fill the array with secure random bytes
            return Convert.ToBase64String(saltBytes); // Return the salt as a Base64-encoded string
        }

        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
