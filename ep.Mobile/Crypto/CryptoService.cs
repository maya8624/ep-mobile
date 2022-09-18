using ep.Mobile.Reference;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ep.Mobile.Crypto
{
    public class CryptoService
    {
        public static (string eText, string iv) Encrypt(string key, string text)
        {
            Aes cipher = CreateCipher(key);

            //show the IV on page (will use for decrypt, normally send in clear along with ciphertext)
            var iv = Convert.ToBase64String(cipher.IV);
            //Create the encryptor, convert to bytes, and encrypt
            ICryptoTransform cryptoTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptoTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            //Convert to base64 for display
            var eText = Convert.ToBase64String(cipherText);
            return (eText, iv);
        }

        private static Aes CreateCipher(string key)
        {
            Aes cipher = Aes.Create();
            cipher.Padding = PaddingMode.ISO10126;         
            cipher.Key = Convert.FromBase64String(key);// conversions.HexToByteArray("892C*E496"); //symetric key
            return cipher;
        }

        public static string GetHash(string text, string salt)
        {
            byte[] secret = Encoding.UTF8.GetBytes(salt);            
            string hashedText = Convert.ToBase64String(KeyDerivation.Pbkdf2
            (
                   password: text,
                   salt: secret,
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 10000,
                   numBytesRequested: 256 / 8)
            );
            return hashedText;
        }

        public static string GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            var hex = Convert.ToBase64String(salt);
            return hex;
        }

        public static async Task<string> GetHashedText(string text)
        {
            var salt = await SecureStorage.GetAsync(Constant.StorageSaltKey);
            var hashedText = GetHash(text, salt);
            return hashedText;
        }
    }
}
