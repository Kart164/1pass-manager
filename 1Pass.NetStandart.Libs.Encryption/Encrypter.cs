using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _1Pass.NetStandart.Libs.Encryption
{
    public static class Encrypter
    {

        public static async Task<string> Encrypt(string key, string passToCrypt, string salt)
        {
            byte[] cryptedBytes;
            var passBytes = Encoding.UTF8.GetBytes(passToCrypt);
            var keyForAes= new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(salt+"1PassManager"), 32768);

            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = keyForAes.GetBytes(aes.KeySize / 8);
                aes.IV = keyForAes.GetBytes(aes.BlockSize / 8);

                using (var mems = new MemoryStream())
                {
                    using (var crypts = new CryptoStream(mems, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        await crypts.WriteAsync(passBytes, 0, passBytes.Length);

                    cryptedBytes = mems.ToArray();
                }
            }
            return Convert.ToBase64String(cryptedBytes);
        }

        public static async Task<string> Decrypt(string key, string cryptedPass,string salt)
        {
            byte[] decrypted;
            var cryptedPassBytes = Convert.FromBase64String(cryptedPass);
            var keyForAes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(salt + "1PassManager"), 32768);

            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = keyForAes.GetBytes(aes.KeySize / 8);
                aes.IV = keyForAes.GetBytes(aes.BlockSize / 8);

                using (var mems = new MemoryStream())
                {
                    using (var crypts = new CryptoStream(mems, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        await crypts.WriteAsync(cryptedPassBytes, 0, cryptedPassBytes.Length);


                    decrypted = mems.ToArray();
                }
                  
            }

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}

