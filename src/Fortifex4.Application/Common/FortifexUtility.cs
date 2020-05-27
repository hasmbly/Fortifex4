using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Fortifex4.Application.Common
{
    public static class FortifexUtility
    {
        public static string CreatePasswordHash(string password, string passwordSalt)
        {
            var hash = KeyDerivation.Pbkdf2(password, Encoding.UTF8.GetBytes(passwordSalt), KeyDerivationPrf.HMACSHA512, 10000, 256 / 8);

            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPasswordHash(string password, string passwordSalt, string passwordHash)
            => CreatePasswordHash(password, passwordSalt) == passwordHash;

        public static string CreatePasswordSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public static void SplitFullName(string fullName, out string firstName, out string lastName)
        {
            int indexOfFirstSpace = fullName.IndexOf(" ");

            if (indexOfFirstSpace > 0)
            {
                firstName = fullName.Substring(0, indexOfFirstSpace);
                int startIndex = indexOfFirstSpace + 1;
                int length = fullName.Length - (indexOfFirstSpace + 1);
                lastName = fullName.Substring(startIndex, length);
            }
            else
            {
                firstName = fullName;
                lastName = string.Empty;
            }
        }
    }
}