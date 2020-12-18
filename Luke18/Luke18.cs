using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Luke18
{
    class Luke18
    {
        static void Main(string[] args)
        {
            int count = 0;
            Performance.TimeRun("Read and solve", () =>
            {
                count = Solve();
            },10,10);
            Console.WriteLine(count);
            Performance.TimeRun("Read and solve MT", () =>
            {
                count = SolveLinqMT();
            },10,10);
            Console.WriteLine(count);

            Performance.TimeRun("Read file", () =>
            {
                TextFile.ReadStringList("wordlist.txt");
            }, 10, 10);

            var cacheWords = TextFile.ReadStringList("wordlist.txt");
            Performance.TimeRun("solve", () =>
            {
                count = SolveNoIO(cacheWords);
            }, 100, 20);
            Console.WriteLine(count);
            Performance.TimeRun("solve MT", () =>
            {
                count = SolveLinqMTNoIO(cacheWords);
            }, 100, 100);
            Console.WriteLine(count);
        }

        public static int SolveNoIO(List<string> wordlist)
        {
            int count = 0;
            foreach(var word in wordlist)
            {
                if (IsPalmostDrome(word))
                {
                    count++;
                }

            }
            return count;
        }

        public static int SolveLinqMTNoIO(List<string> wordlist)
        {
            return wordlist.AsParallel().Count(l => IsPalmostDrome(l));
        }


        public static int Solve()
        {
            int count = 0;
            using (StreamReader sr = File.OpenText("wordlist.txt"))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    if (IsPalmostDrome(s))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static int SolveLinqMT()
        {
            return TextFile.EnumerateLines("wordlist.txt").AsParallel().Count(l => IsPalmostDrome(l));
        }


        static bool IsPalmostDrome(string word, int start = 0)
        {
            if (word.Length < 3) return start != 0;
            for (int head = start; head < word.Length / 2; head++)
            {
                int tail = head + 1;
                if (word[head] != word[^tail])
                {
                    if (word[head + 1] == word[^tail] && word[head] == word[^(tail + 1)])
                    {
                        return IsPalmostDrome(word, head + 2);
                    }
                    return false;
                }
            }
            return start != 0;
        }

    }
}
