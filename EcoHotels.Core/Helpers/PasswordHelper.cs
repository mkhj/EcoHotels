using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EcoHotels.Core.Helpers
{
    public class PasswordHelper
    {
        public static string Generate(int length = 8)
        {
            var iteration = 0;
            var randomNumber = 0;
            var password = string.Empty;
            var rand = new Random();

            while (iteration < length) {
                randomNumber = Convert.ToInt32(Math.Floor((rand.Next() * 100m)) % 94) + 33;

                if ((randomNumber >= 33) && (randomNumber <= 47)) { continue; }
                if ((randomNumber >= 58) && (randomNumber <= 64)) { continue; }
                if ((randomNumber >= 91) && (randomNumber <= 96)) { continue; }
                if ((randomNumber >= 123) && (randomNumber <= 126)) { continue; }

                iteration++;
                password += Convert.ToChar(randomNumber);

                
            }

            return password;
        }

        /// <summary>
        /// Hashes a password
        /// </summary>
        /// <param name="password">The password to hash</param>

        /// <returns>The hashed password as a 128 character hex string</returns>
        public static string HashPassword(string password)
        {

            var salt = GetRandomSalt();
            var hash = Sha256(salt + password);
            return salt + hash;
        }

        /// <summary>
        /// Validates a password
        /// </summary>
        /// <param name="password">The password to test</param>
        /// <param name="correctHash">The hash of the correct password</param>
        /// <returns>True if password is the correct password, false otherwise</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            if (correctHash.Length < 128)
            {
                return false;
                //throw new ArgumentException("correctHash must be 128 hex characters!");
            }

            var salt = correctHash.Substring(0, 64);
            var validHash = correctHash.Substring(64, 64);
            var passHash = Sha256(salt + password);
            return string.Compare(validHash, passHash) == 0;
        }

        /// <summary>
        /// returns the SHA256 hash of a string, formatted in hex
        /// </summary>
        /// <remarks>It returns Hex because PHP seems to do the same</remarks>
        /// <param name="toHash"></param>
        /// <returns></returns>
        private static string Sha256(string toHash)
        {
            var hash = new SHA256Managed();
            var utf8 = Encoding.UTF8.GetBytes(toHash);
            return BytesToHex(hash.ComputeHash(utf8));
        }

        /// <summary>
        /// Returns a random 64 character hex string (256 bits)
        /// </summary>
        /// <returns></returns>
        public static string GetRandomSalt()
        {
            var salt = new byte[32];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(salt);
            }
            return BytesToHex(salt);

            //var random = new RNGCryptoServiceProvider();
            //var salt = new byte[32]; //256 bits
            //random.GetBytes(salt);
            //return BytesToHex(salt);
        }

        /// <summary>
        /// Converts a byte array to a hex string
        /// </summary>
        /// <param name="toConvert"></param>
        /// <returns></returns>
        private static string BytesToHex(byte[] toConvert)
        {
            var s = new StringBuilder(toConvert.Length * 2);
            foreach (var b in toConvert)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }
    }
}
