using System;
using System.Linq;
using Utilities;

namespace Luke9
{
    class Luke9
    {
        private int width;
        private int height;
        private bool[] infected;

        public Luke9(string filename)
        {
            var lines = TextFile.ReadStringList(filename);
            width = lines[0].Trim().Length;
            height = lines.Count;
            infected = new bool[height * width];
            for (int y = 0; y < height; y++)
            {
                string s = lines[y].Trim();
                for (int x = 0; x < width; x++)
                {
                    infected[y * width + x] = s[x] == 'S';
                }
            }
        }

        public int Solve()
        {
            int newInfected = infected.Count(b => b);
            int steps = 0;
            while (newInfected > 0)
            {
                newInfected = 0;
                //step simulation
                bool[] next = (bool[])infected.Clone();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        if (infected[index]) continue; //already infected
                        int n = 0;
                        if (y > 0 && infected[index - width]) n++;
                        if (y < height - 1 && infected[index + width]) n++;
                        if (n < 2 && x > 0 && infected[index - 1]) n++;
                        if (n < 2 && x < width - 1 && infected[index + 1]) n++;
                        if (n > 1)
                        {
                            newInfected++;
                            next[index] = true;
                        }
                    }
                }
                infected = next;
                steps++;
            }
            return steps;
        }

        static void Main(string[] args)
        {
            int days = 0;
            Performance.Benchmark("read and solve", () =>
            {
                var luke = new Luke9("elves.txt");
                days = luke.Solve();
            });
            Console.WriteLine(days);
        }
    }
}
