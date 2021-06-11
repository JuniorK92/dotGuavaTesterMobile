using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

namespace dotGuava.Essential
{
    /// <summary>
    /// Class that generate unique codes that can be used and compared through different devices .
    /// </summary>
    public static class Token
    {
        /// <summary>
        /// Generate a 40 items token list each one of them with a designated lenght based on a designated seed.
        /// </summary>
        /// <param name="lenght">Lenght in digits of each token.</param>
        /// <param name="seed">Seed to be used for token generation, this will determine token compatibility.</param>
        /// <returns>A ready to be used token list.</returns>
        public static IEnumerable<string> GetStringTokenList(uint lenght, string key)
        {
            if (lenght < 4 || lenght > 12)
            {
                return new List<string>();
            }

            var tokenList = new List<string>();

            try
            {
                var utcDateTime = DateTime.UtcNow;
                var consideredUntilHours = utcDateTime.AddMilliseconds(utcDateTime.Millisecond * -1).AddSeconds(utcDateTime.Second * -1).AddMinutes(utcDateTime.Minute * -1);
                var ticks = consideredUntilHours.Ticks;

                var keyBytes = Encoding.UTF8.GetBytes(key);

                long summedKeyBytes = 0;
                foreach (var x in keyBytes)
                {
                    summedKeyBytes += (int.Parse(x.ToString()) * lenght);
                }

                var raw = ticks.ToString().Substring(0, 8);
                var rawInt = long.Parse(raw);

                for (int i = 10; i < 50; i++)
                {
                    var rawProcessed = (rawInt * (summedKeyBytes * (i * rawInt)));
                    if (rawProcessed < 0) { rawProcessed = (rawProcessed * -1); }

                    var rawProcessedString = ((ulong)(rawInt * (summedKeyBytes * (i * rawInt)))).ToString();
                    
                    string token = rawProcessedString;

                    if (token.Length < lenght)
                    {
                        token += (raw.ToString() + raw.ToString());
                    }

                    var toAdd = token.Substring(0, (int)lenght);
                    int counter = 0;
                    while (tokenList.Contains(toAdd))
                    {
                        counter++;

                        toAdd = token.Substring(counter, (int)lenght);
                    }

                    tokenList.Add(toAdd.ToString());
                }

                return tokenList;
            }
            catch (Exception) { return new List<string>(); }
        }
    }
}
