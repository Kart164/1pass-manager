using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _1Pass.Encryption
{
    public static class CharArrayShuffler
    {
        public static char[] Shuffle(params char[] chars)
        {
            int n = chars.Length;
            while (n>1)
            {
                int k = RandomNumberGenerator.GetInt32(0, n--);
                (chars[k], chars[n]) = (chars[n], chars[k]);
            }
            return chars;
        }  
    }
}
