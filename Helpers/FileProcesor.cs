using Domain.Interfaces.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public class FileProcesor : IFileProcesor
    {
        private readonly ICryptoTransform encryptor;
        private readonly ICryptoTransform decryptor;

        public FileProcesor(FileProcesorConfiguration configuration)
        {
            Configuration = configuration;

            var initVectorBytes = Encoding.UTF8.GetBytes(configuration.InitVector);
            var password = new PasswordDeriveBytes(configuration.PassPhrase, null);
            var keyBytes = password.GetBytes(configuration.Keysize / 8);
            var symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };

            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        }

        public FileProcesorConfiguration Configuration { get; }

        public string GetCsv<T>(IEnumerable<T> items)
        {
            string ret = null;
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                writer.WriteLine(string.Join(Configuration.SeparadorCsv, properties.Select(p => p.Name)));
                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(Configuration.SeparadorCsv, properties.Select(p => p.GetValue(item, null))));
                }
                writer.Flush();
                ms.Position = 0;
                ret = Encoding.ASCII.GetString(ms.ToArray());
            }
            return ret;
        }

        public string EncryptString(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                var cipherTextBytes = memoryStream.ToArray();
                return Convert.ToBase64String(cipherTextBytes);
            }
        }

        public string DecryptString(string cipherText)
        {
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                var plainTextBytes = new byte[cipherTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }
    }
}