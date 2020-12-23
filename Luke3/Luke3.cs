using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Utilities;

namespace Luke3
{
    class Luke3
    {
        static void Main(string[] args)
        {
            List<string> candidates = null;
            Performance.Benchmark("Read and search", () =>
            {
                var matrix = new CharacterMatrix(TextFile.ReadStringList("matrix.txt"));
                candidates = TextFile.ReadStringList("wordlist.txt");
                EliminateWords(matrix, candidates);
                candidates.Sort();
            }, 10, 10);
            Console.WriteLine(string.Join(',', candidates));
        }

        private static void EliminateWords(CharacterMatrix matrix, List<string> candidates)
        {
            List<string> strings = null;

            strings = FindAllStrings(matrix);

            Dictionary<string, string> reverseMap = new Dictionary<string, string>();
            foreach (var str in candidates) reverseMap.Add(str, Reverse(str));

            foreach (var kvp in reverseMap)
            {
                if (FindString(strings, kvp.Key, kvp.Value)) candidates.Remove(kvp.Key);
            }
        }

        static bool FindString(List<string> strings, string candidate, string candidateReversed)
        {
            foreach(var str in strings)
            {
                if (str.Contains(candidate) || str.Contains(candidateReversed)) return true;
            }
            return false;
        }

        private static List<string> FindAllStrings(CharacterMatrix matrix)
        {
            List<string> allStrings = new List<string>();

            //horisontal
            allStrings.AddRange(matrix.Lines);

            //vertical
            Size down = new Size(0, 1);
            for (int x = 0; x < matrix.Width; x++)
            {
                allStrings.Add(Traverse(matrix, new Point(x, 0), down));
            }

            //diag down
            Size diagDown = new Size(1, 1);
            for (int y = matrix.Height - 1; y > 0; y--)
            {
                allStrings.Add(Traverse(matrix, new Point(0, y), diagDown));
            }
            for (int x = 0; x < matrix.Width; x++)
            {
                allStrings.Add(Traverse(matrix, new Point(x, 0), diagDown));
            }

            //diag up
            Size diagUp = new Size(1, -1);
            for (int y = matrix.Height - 2; y >= 0; y--)
            {
                allStrings.Add(Traverse(matrix, new Point(0, y), diagUp));
            }
            for (int x = 0; x < matrix.Width; x++)
            {
                allStrings.Add(Traverse(matrix, new Point(x, matrix.Height - 1), diagUp));
            }

            return allStrings;
        }

        public static string Reverse(string text)
        {
            if (text == null) return null;
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        static string Traverse(CharacterMatrix matrix, Point pos, Size direction)
        {
            StringBuilder sb = new StringBuilder();
            while (matrix.IsInside(pos))
            {
                sb.Append(matrix.GetChar(pos));
                pos += direction;
            }
            return sb.ToString();
        }
    }
}
