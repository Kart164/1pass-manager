using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace _1Pass.Encryption
{
    public class Encrypter
    {
        private byte[] _key;

        public Encrypter(string key)
        {
            _key = Encoding.UTF8.GetBytes(key);
        }

        public async Task<string> Encrypt(string pass, string salt)
        {
            byte[] cryptedBytes;
            var passBytes = Encoding.UTF8.GetBytes(pass);
            var keyForAes= new Rfc2898DeriveBytes( _key, Encoding.UTF8.GetBytes(salt), 32768);

            using var aes = new AesManaged();
            aes.KeySize = 256;
            aes.Key = keyForAes.GetBytes(aes.KeySize/8);
            aes.IV = keyForAes.GetBytes(aes.BlockSize/8);

            using var mems = new MemoryStream();
            using var crypts= new CryptoStream(mems, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await crypts.WriteAsync(passBytes, 0, passBytes.Length);
            crypts.Close();

            cryptedBytes=mems.ToArray();

            var sb = new StringBuilder();
            foreach (var cryptbyte in cryptedBytes)
            {
                sb.Append(cryptbyte.ToString("x2"));
            }
            return sb.ToString();
        }

        public async Task<string> Decrypt(string cryptedPass,string salt)
        {
            byte[] decrypted;
            var cryptedPassBytes = Encoding.UTF8.GetBytes(cryptedPass);
            var keyForAes = new Rfc2898DeriveBytes(_key, Encoding.UTF8.GetBytes(salt), 32768);

            using var aes = new AesManaged();
            aes.KeySize = 256;
            aes.Key = keyForAes.GetBytes(aes.KeySize / 8);
            aes.IV = keyForAes.GetBytes(aes.BlockSize / 8);

            using var mems = new MemoryStream();
            using var crypts = new CryptoStream(mems, aes.CreateDecryptor(), CryptoStreamMode.Write);
            await crypts.WriteAsync(cryptedPassBytes, 0, cryptedPassBytes.Length);
            crypts.Close();

            decrypted = mems.ToArray();

            var sb = new StringBuilder();
            foreach (var decrbyte in decrypted)
            {
                sb.Append(decrbyte.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
