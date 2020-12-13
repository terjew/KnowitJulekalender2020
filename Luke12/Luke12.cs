using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Luke12
{
    class Luke12
    {
        static void Main(string[] args)
        {
            int size = 0;
            Performance.TimeRun("Enumerate and count", () =>
            {
                var input = TextFile.EnumerateAsciiCharacters("family.txt");
                size = GetLargestGeneration(input);
            });
            Performance.TimeRun("Read all and count (IEnumerable)", () =>
            {
                var str = File.ReadAllText("family.txt");
                size = GetLargestGeneration(str);
            });
            Performance.TimeRun("Read all and count (string)", () =>
            {
                var str = File.ReadAllText("family.txt");
                size = GetLargestGenerationString(str);
            });
            Performance.TimeRun("Read (string)", () =>
            {
                var str = File.ReadAllText("family.txt");
            });
            Console.WriteLine($"Largest generation has {size} elves");
        }

        private static int GetLargestGeneration(IEnumerable<char> input)
        {
            int generation = 0;
            List<int> generations = new List<int>();
            generations.Add(0);
            var enumerator = input.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Current)
                {
                    case '(':
                        generation++;
                        if (generations.Count <= generation) generations.Add(0);
                        break;
                    case ')':
                        generation--;
                        break;
                    default:
                        if (enumerator.Current >= 'A' && enumerator.Current <= 'Z') generations[generation]++;
                        break;
                }
            }
            return generations.Max();
        }


        private static int GetLargestGenerationString(string familyTree)
        {
            int pos = 0;
            int generation = 0;
            int[] generations = new int[2000];
            while (pos < familyTree.Length)
            {
                var c = familyTree[pos++];
                switch (c)
                {
                    case '(':
                        generation++;
                        break;
                    case ')':
                        generation--;
                        break;
                    default:
                        if (c >= 'A' && c <= 'Z') generations[generation]++;
                        break;
                }
            }
            return generations.Max();
        }

    }
}
