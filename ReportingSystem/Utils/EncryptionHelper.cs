using System.Security.Cryptography;
using System.Text;

namespace ReportingSystem.Utils
{
    public class EncryptionHelper
    {
        private static readonly string encryptionKey = "1234567890123456"; // Replace with your key

        public static string Encrypt(string plainText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);

            // Generate a random IV
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new(csEncrypt);
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(aesAlg.IV) + Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);

            // Extract IV from the beginning of the cipher text
            byte[] ivBytes = Convert.FromBase64String(cipherText[..24]); // Assuming 128-bit IV

            aesAlg.IV = ivBytes;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText[24..]));
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
