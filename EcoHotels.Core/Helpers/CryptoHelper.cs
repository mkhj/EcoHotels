using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EcoHotels.Core.Helpers
{
    public class CryptoHelper
    {
        const int KEY_SIZE = 32;

        // Create our own Key and IV
        static readonly byte[] ENCRYPT_KEY = Key("94553513593651443545334545889342");
        static readonly byte[] ENCRYPT_IV = { 1, 5, 6, 7, 8, 1, 3, 3, 5, 7, 8, 9, 4, 4, 5, 6, 4, 5, 6, 2, 4, 5, 6, 4, 6, 4, 2, 3, 4, 8, 6, 2 };

        private static byte[] Key(string sText)
        {
            int keyLength = Encoding.Default.GetByteCount(sText);

            if (keyLength < 1 || keyLength > KEY_SIZE)
            {
                throw new ArgumentException("Key must be more than 0 and less than " + KEY_SIZE + " bytes long.");
            }

            if (keyLength < KEY_SIZE)
            {
                sText = sText.PadRight(KEY_SIZE);
            }

            return Encoding.Default.GetBytes(sText);
        }

        public static string Encrypt(string sText)
        {
            // Convert Input Text into Bytes Array
            var bText = Encoding.Default.GetBytes(sText.Trim());

            using (var alg = SymmetricAlgorithm.Create("Rijndael"))
            {
                alg.BlockSize = 256;
                alg.KeySize = KEY_SIZE * 8;

                using (var stream = new MemoryStream())
                {
                    using (var crypto = new CryptoStream(stream, alg.CreateEncryptor(ENCRYPT_KEY, ENCRYPT_IV), CryptoStreamMode.Write))
                    {
                        // Do the actual encryption
                        crypto.Write(bText, 0, bText.Length);
                        crypto.Close();

                        return Convert.ToBase64String(stream.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string sText)
        {
            // Convert Input Text into Bytes Array
            var bText = Convert.FromBase64String(sText);

            // For storing encrypted bytes
            var mStream = new MemoryStream(bText);

            var alg = SymmetricAlgorithm.Create("Rijndael");

            alg.BlockSize = 256;
            alg.KeySize = 256;

            var decryptor = alg.CreateDecryptor(ENCRYPT_KEY, ENCRYPT_IV);

            // Tell the crypto stream where and how to decrypt
            var crypto = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);

            // Do the actually decryption
            var bOutput = new byte[bText.Length - 1 + 1];
            crypto.Read(bOutput, 0, bText.Length);
            crypto.Close();

            return Encoding.Default.GetString(bOutput).Replace("\x000", String.Empty);
        }


    }
}
