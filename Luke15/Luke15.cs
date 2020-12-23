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
            int sum = 0;
            Performance.Benchmark("LINQ solution", () =>
            {
                sum = Solve();
            }, 10, 1, 3);

            Console.WriteLine(sum);
        }

        static int Solve()
        {
            var combinations = TextFile.ReadStringList("riddles.txt");
            var dictionary = TextFile.ReadStringSet("wordlist.txt");
            var pairs = combinations.Select(c => c.Split(", ")).Select(arr => (arr[0], arr[1])).ToArray();
            return dictionary
                .AsParallel()
                .Where(word => pairs.Any(p => dictionary.Contains($"{p.Item1}{word}") && dictionary.Contains($"{word}{p.Item2}")))
                .Sum(w => w.Length);
        }
    }
}
