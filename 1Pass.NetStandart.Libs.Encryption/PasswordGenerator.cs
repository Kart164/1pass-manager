using System;
using System.Security.Cryptography;
using System.Text;

namespace _1Pass.NetStandart.Libs.Encryption
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

            var seed = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetNonZeroBytes(seed);
            var rnd = new Random(BitConverter.ToInt32(seed, 0));
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[rnd.Next(0, chars.Length - 1)]);
            }
            return sb.ToString();
        }
    }
}
