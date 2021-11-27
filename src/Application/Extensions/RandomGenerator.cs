using System;
using System.Linq;

namespace Application.Extentions
{
    public static class RandomGenerator
    {
        public static string GenerateNumber(int length = 4)
        {
            Random random = new Random();
            const string chars = "0123345667899";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateString(int length = 5, bool withLowerCase = true, bool withNumber = false, bool withUpperCase = false)
        {
            Random random = new Random();

            string chars = "369";
            if (withLowerCase || (!withLowerCase && !withNumber && !withUpperCase))
                chars += "qwertyuiopasdfghjklzxcvbnm";
            if (withNumber)
                chars += "0123345667899";
            if (withUpperCase)
                chars += "QWERTYUIOPASDFGHJKLZXCVBNM";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}