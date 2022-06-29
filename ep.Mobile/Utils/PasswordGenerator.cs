using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Utils
{
    public class PasswordGenerator
    {
        private static List<char> chars = new List<char>();
        private static readonly int _len = 8;

        private static void AddChars(ref List<char> chars)
        {
            for (char c = 'a'; c <= 'z'; c++)
            {
                chars.Add(c);
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                chars.Add(c);
            }
            for (char c = '!'; c <= '?'; c++)
            {
                chars.Add(c);
            }
            for (char c = '0'; c <= '9'; c++)
            {
                chars.Add(c);
            }
        }

        public static string GeneratePassword()
        {
            AddChars(ref chars);
            var sb = new StringBuilder();
            var rnd = new Random();
            int i = 0;
            while (i < _len)
            {
                sb.Append(chars[rnd.Next(0, chars.Count)]);
                i++;
            }
            return sb.ToString();
        }

        public static string GenerateVerificationCode()
        {
            var numbers = new List<char>();
            for (char c = '0'; c <= '9'; c++)
            {
                numbers.Add(c);
            }

            var sb = new StringBuilder();
            var rnd = new Random();
            int i = 0;
            while (i < 6)
            {
                sb.Append(numbers[rnd.Next(0, numbers.Count)]);
                i++;
            }
            return sb.ToString();
        }
    }
}
