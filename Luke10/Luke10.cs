using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke10
{
    class Luke10
    {
        static void Main(string[] args)
        {
            string elf = "";
            int score = 0;
            Performance.TimeRun("Solution", () =>
            {
                (elf, score) = Solve("leker.txt");
            });
            Performance.TimeRun("LINQ solution", () =>
            {
                (elf, score) = SolveLinq("leker.txt");
            });
            Console.WriteLine($"{elf}-{score}");
        }

        static (string elf,int score) SolveLinq(string filename)
        {
            var winner = TextFile.ReadStringList(filename)
                .SelectMany(l => l.Split(',').Reverse().Select((e, i) => new { elf = e, score = i }))
                .GroupBy(scoring => scoring.elf)
                .Select(g => new { elf = g.Key, score = g.Sum(scoring => scoring.score) })
                .OrderByDescending(scoring => scoring.score)
                .First();
            return (winner.elf, winner.score);
        }

        static (string elf, int score) Solve(string filename)
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            foreach (var line in TextFile.ReadStringList("leker.txt"))
            {
                foreach (var scoring in line.Split(',').Reverse().Select((e, i) => new { elf = e, score = i }))
                {
                    if (!scores.ContainsKey(scoring.elf)) scores.Add(scoring.elf, 0);
                    scores[scoring.elf] += scoring.score;
                }
            }
            var winner = scores.OrderByDescending(kvp => kvp.Value).First();
            return (winner.Key, winner.Value);
        }
    }
}
