using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace dotGuava.Security
{
    /// <summary>
    /// Class that encrypts and decrypts strings
    /// </summary>
    public class Cypher
    {
        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="input">Represents the string to be encrypted.</param>        
        /// <returns>Encrypted string representation.</returns>
        public static string Encrypt(string input)
        {
            var byteArray = Encoding.UTF8.GetBytes(input);

            return Convert.ToBase64String(byteArray);
        }
        /// <summary>
        /// Decrypts an encrypted string.
        /// </summary>
        /// <param name="input">Represents encrypted string</param>        
        /// <returns>Real value for encrypted string.</returns>
        public static string Decrypt(string input)
        {
            var byteArray = Convert.FromBase64String(input);

            return Encoding.UTF8.GetString(byteArray);
        }
        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="input">Represents the string to be encrypted.</param>
        /// <param name="key">Represents the key to be used for encryption.</param>
        /// <returns>Encrypted string representation.</returns>
        public static string Encrypt(string input, string key)
        {
            // Get the bytes of the string
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes();

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + inputBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < inputBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = inputBytes[i];
            }

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }
        /// <summary>
        /// Decrypts an encrypted string.
        /// </summary>
        /// <param name="input">Represents encrypted string</param> 
        /// /// <param name="key">Represents the key to be used for decryption.</param>
        /// <returns>Real value for encrypted string.</returns>
        public static string Decrypt(string input, string key)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            // Getting the size of salt
            int _saltSize = 16;
                        
            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];

            for (int i = _saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - _saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }
        private static byte[] GetRandomBytes()
        {
            int _saltSize = 16;
            byte[] ba = new byte[_saltSize];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }
        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 16);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 16);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
