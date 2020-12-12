using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Luke11
{
    class Luke11
    {
        static void Main(string[] args)
        {
            var hasPassword = ObfuscatedContains("juletre", "jeljxmw");
            var password = "eamqia";
            var path = "hint.txt";
            Performance.TimeRun("naive", () => SolveNaive(path, password), 10, 20);
            Performance.TimeRun("linq", () => SolveLinq(path, password), 10, 20);
            Performance.TimeRun("plinq", () => SolvePLinq(path, password), 10, 20);
        }

        private static string SolveLinq(string path, string password)
        {
            return TextFile.EnumerateLines(path).First(l => ObfuscatedContains(l, password));
        }

        private static string SolvePLinq(string path, string password)
        {
            return TextFile.EnumerateLines(path).AsParallel().First(l => ObfuscatedContains(l, password));
        }

        private static string SolveNaive(string path, string password)
        {
            foreach (var line in TextFile.EnumerateLines(path))
            {
                if (ObfuscatedContains(line, password))
                {
                    return line;
                }
            }
            return null;
        }

        static bool ObfuscatedContains(string hint, string password)
        {
            if (hint.Length < password.Length) return false;
            List<string> lines = new List<string>();
            lines.Add(hint);
            var prev = hint.Select(c => c - 'a').ToArray();
            while(prev.Length > 1)
            {
                prev = prev.Skip(1).Select((c, i) => (c + 1 + prev[i]) % 26).ToArray();
                lines.Add(new string(prev.Select(i => (char)(i + 'a')).ToArray()));
            }

            //can't be in a column shorter than password
            var last = hint.Length - password.Length;
            for (int i = 0; i <= last; i++)
            {
                var sb = new StringBuilder();
                foreach (var line in lines) if (line.Length > i) sb.Append(line[i]);
                if (sb.ToString().Contains(password)) return true;
            }
            return false;
        }
    }
}
