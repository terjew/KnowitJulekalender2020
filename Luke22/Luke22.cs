using System;
using System.IO;
using System.Linq;
using Utilities;

namespace Luke22
{
    class Luke22
    {
        static void Main(string[] args)
        {
            int lineno = 0;
            Performance.Benchmark("Read and solve", () => lineno = Solve("input.txt"), 10, 10, 2);
            Console.WriteLine(lineno);
        }

        static int Solve(string filename)
        {
            int max = 0;
            int best = 0;
            int current = 0;
            using (StreamReader sr = File.OpenText(filename))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    var count = CountSpellings(s);
                    if (count > max)
                    {
                        max = count;
                        best = current;
                    }
                    current++;
                }
            }
            return best;
        }

        static bool FindName(string name, char[] letters, int lettersLength)
        {
            int[] letterPositions = new int[name.Length];
            int needleIndex = 0;
            int i = 0;
            char needle = name[needleIndex];
            while (i < lettersLength)
            {
                if (letters[i] == needle)
                {
                    letterPositions[needleIndex] = i;
                    if (needleIndex == name.Length - 1)
                    {
                        foreach(var pos in letterPositions)
                        {
                            letters[pos] = '^';
                        }
                        return true;
                    }
                    needle = name[++needleIndex];
                }
                i++;
            }
            return false;
        }

        static int CountSpellings(string line)
        {
            var length = 0;
            while (line[length] != ' ') length++;
            char[] arr = line.ToCharArray(0, length);
            var namePart = line.Substring(length + 2, line.Length - 3 - length);
            var names = namePart.Split(", ").Select(n => n.ToLower());
            int count = 0;
            foreach (var name in names)
            {
                if (FindName(name, arr, length))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
