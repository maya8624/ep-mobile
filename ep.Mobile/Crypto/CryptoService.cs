using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
    }
}
