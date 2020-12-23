using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;

namespace Luke7
{
    class Luke7
    {
        static void Main(string[] args)
        {
            int numSymmetric = 0;
            Performance.Benchmark("Read and solve", () =>
            {
                var lines = TextFile.ReadStringList("forest.txt");
                lines.Reverse();
                var matches = Regex.Matches(lines[0], @"#");
                var indices = matches.Select(m => m.Index).ToArray();
                numSymmetric = indices.Select(i => IsTreeSymmetrical(lines, i)).Count(s => s);
            });
            Console.WriteLine($"Bonden har {numSymmetric} trær som kan selges");
        }

        static bool IsTreeSymmetrical(List<string> lines, int pos)
        {
            foreach (var branch in lines)
            {
                int left = pos - 1, right = pos + 1;
                while (true)
                {
                    if (left < 0 || right >= branch.Length) break;
                    if (branch[left] != branch[right]) return false;
                    if (branch[left] == ' ' && branch[left + 1] == ' ') break;
                    left--; right++;
                }
            }
            return true;
        }
    }
}
