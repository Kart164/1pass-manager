using System;
using System.Security.Cryptography;

namespace _1Pass.NetStandart.Libs.Encryption
{
    public static class CharArrayShuffler
    {
        public static char[] Shuffle(char[] chars)
        {
            int n = chars.Length;
            var seed = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(seed);
            }                 
            var rnd = new Random(BitConverter.ToInt32(seed,0));
            while (n>1)
            {
                var k = rnd.Next(0, n--);
                (chars[k], chars[n]) = (chars[n], chars[k]);
            }
            return chars;
        }  
    }
}
