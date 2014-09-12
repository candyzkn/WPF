using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyWPF.Utils
{
    public class Encryption
    {
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static byte[] EncryptBytes(byte[] bytes, string password)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, Keys);
                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    aes.IV = aes.Key;
                    using (MemoryStream encryptionStream = new MemoryStream())
                    {
                        using (CryptoStream encrypt = new CryptoStream(encryptionStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            encrypt.Write(bytes, 0, bytes.Length);
                            encrypt.FlushFinalBlock();
                        }
                        return encryptionStream.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Encrypt a string using AES
        /// </summary>
        /// <param name="str">String to encrypt</param>
        /// <param name="password">Encryption password</param>
        /// <returns>Encrypted string in case of success; otherwise - empty string</returns>
        public static string EncryptString(string str, string password)
        {
            var result = EncryptBytes(Encoding.UTF8.GetBytes(str), password);
            if (result == null) return null;
            return Convert.ToBase64String(result);
        }

        public static Byte[] DecryptBytes(Byte[] bytes, string password)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, Keys);
                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    aes.IV = aes.Key;

                    using (MemoryStream decryptionStream = new MemoryStream())
                    {
                        using (CryptoStream decrypt = new CryptoStream(decryptionStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            decrypt.Write(bytes, 0, bytes.Length);
                            decrypt.Flush();
                        }
                        return decryptionStream.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypt encrypted string
        /// </summary>
        /// <param name="str">Encrypted string</param>
        /// <param name="password">Password used for encryption</param>
        /// <returns>Decrypted string if success; otherwise - empty string</returns>
        public static string DecryptString(string str, string password)
        {
            var result = DecryptBytes(Convert.FromBase64String(str), password);
            if (result == null) return null;
            return Encoding.UTF8.GetString(result,0,result.Length);
        }
 
    }
}
