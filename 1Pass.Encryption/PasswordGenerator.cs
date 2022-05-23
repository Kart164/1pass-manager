using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _1Pass.Encryption
{
    public static class PasswordGenerator
    {
        private static string lowers = "abcdefghijklmnopqrstuvwxyz";
        private static string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string number = "0123456789";
        private static string special = "*$-+?_&=!%{}/";

        public static string Generate(int length = 16, bool withSpecialSymbols = true)
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
