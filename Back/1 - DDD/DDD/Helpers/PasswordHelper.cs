using System;
using System.Security.Cryptography;
using System.Text;

namespace DDD.Helpers
{
    public static class PasswordHelper
    {
        public static string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(1, false));
            builder.Append(RandomString(4, true));
            builder.Append(RandomSpecialCharacters(1));
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }

        private static int RandomNumber(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * RandomDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
                return builder.ToString().ToLower();

            return builder.ToString();
        }

        private static string RandomSpecialCharacters(int size)
        {
            string validChars = "._$*#@";
            char[] chars = new char[size];

            for (int i = 0; i < size; i++)
            {
                chars[i] = validChars[RandomNumberGenerator.GetInt32(0, validChars.Length)];
            }

            return new string(chars);
        }

        private static double RandomDouble()
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[8];
            rng.GetBytes(bytes);
            var ul = BitConverter.ToUInt64(bytes, 0) / (1 << 11);
            return ul / (double)(1UL << 53);
        }
    }
}
