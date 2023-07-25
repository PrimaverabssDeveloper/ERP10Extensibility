using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebHookSample.Helpers
{
    /// <summary>Provides a way to encode and decode text.</summary>
    public static class CryptoHelper
    {
        /// <summary>The used encoding.</summary>
        private static readonly Encoding encoding = Encoding.UTF8;

        /// <summary>Encodes the plain text using the Advanced Encryption Standard (AES) algorithm.</summary>
        /// <param name="Id">The unique identifier.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="secret">The secret. Must be a 24-character unsigned alphanumeric string.</param>
        /// <param name="plainText">The text to encode.</param>
        /// <returns>The encoded text.</returns>
        internal static string Encrypt(Guid Id, DateTime timestamp, string secret, string plainText)
        {
            byte[] requestBytes = Id.ToByteArray();
            byte[] timestampBytes = BitConverter.GetBytes(timestamp.Ticks);
            byte[] sharedSecretBytes = encoding.GetBytes(secret);
            byte[] buffer = encoding.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = timestampBytes.Concat(sharedSecretBytes).ToArray();
                aes.IV = requestBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
        }

        /// <summary>Decodes the encrypted text using the Advanced Encryption Standard (AES) algorithm.</summary>
        /// <param name="Id">The unique identifier.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="secret">The secret. Must be a 24-character unsigned alphanumeric string.</param>
        /// <param name="encryptedText">The text to decode.</param>
        /// <returns>The decoded text.</returns>
        internal static string Decrypt(Guid Id, DateTime timestamp, string secret, string encryptedText)
        {
            byte[] requestBytes = Id.ToByteArray();
            byte[] timestampBytes = BitConverter.GetBytes(timestamp.Ticks);
            byte[] sharedSecretBytes = encoding.GetBytes(secret);
            byte[] buffer = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = timestampBytes.Concat(sharedSecretBytes).ToArray();
                aes.IV = requestBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                return encoding.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
        }
    }
}
