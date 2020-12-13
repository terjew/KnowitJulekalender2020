using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Luke13
{
    class Luke13
    {
        static void Main(string[] args)
        {
            var sample = "csfgunqvmiotgixxqeexdnwrtrgftpafkqepkvwwscotfahzneobiipslnbmgyxxumdwxeymprtjrhapxqvguqazkwiorstwcjii";

            char[] remaining = null;

            Performance.TimeRun("read and solve (string)", () =>
            {
                remaining = GetRemainingCharacters(File.ReadAllText("text.txt")).ToArray();
            }, 100, 1000);

            Console.WriteLine(new string(remaining));
        }

        public static IEnumerable<char> GetRemainingCharacters(string s)
        {
            int[] count = new int[26];
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (c < 'a') continue;
                var index = c - 'a';
                count[index]++;
                if (count[index] == index + 1) yield return c;
            }
        }
    }
}
