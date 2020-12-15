using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke15
{
    class Luke15
    {
        static void Main(string[] args)
        {
            var combinations = TextFile.ReadStringList("riddles.txt");
            List<(string, string)> pairs = new List<(string, string)>();

            foreach (var line in combinations)
            {
                var pair = line.Split(", ");
                pairs.Add((pair[0], pair[1]));
            }

            var dictionary = TextFile.ReadStringList("wordlist.txt").ToHashSet();

            HashSet<string> infixes = new HashSet<string>();
            foreach (var kvp in pairs)
            {
                var prefix = kvp.Item1;
                var suffix = kvp.Item2;
                var infixCandidates = dictionary.AsParallel().Where(word => word.Length > prefix.Length && word.StartsWith(prefix)).Select(word => word.Substring(prefix.Length)).ToArray();
                foreach (var candidate in infixCandidates)
                {
                    if (dictionary.Contains(candidate) && dictionary.Contains(candidate + suffix))
                    {
                        infixes.Add(candidate);
                    }
                }
            }

            Console.WriteLine(infixes.Sum(w => w.Length));
            foreach (var infix in infixes.OrderBy(s => s)) Console.WriteLine(infix);
        }
    }
}
