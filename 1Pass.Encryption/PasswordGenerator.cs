using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _1Pass.Encryption
{
    public class PasswordGenerator
    {
        private string lowers = "abcdefghijklmnopqrstuvwxyz";
        private string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string number = "0123456789";
        private string special = "*$-+?_&=!%{}/";

        public string Generate(int length, bool withSpecialSymbols)
        {
            var alphabet = lowers+uppers+number;
            if (withSpecialSymbols)
            {
                alphabet += special;
            }
            var chars = CharArrayShuffler.Shuffle(alphabet.ToCharArray());
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[RandomNumberGenerator.GetInt32(0, chars.Length - 1)]);
            }
            return sb.ToString();
        }
    }
}
