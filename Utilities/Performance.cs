using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Performance
    {
        public static void TimeRun(string what, Action a, int runs = 20, int loops = 100)
        {
            double total = 0;
            for (int run = 0; run < runs; run++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int loop = 0; loop < loops; loop++)
                {
                    a();
                }
                sw.Stop();
                var ms = sw.ElapsedMilliseconds;
                var us = sw.ElapsedTicks / ((double)Stopwatch.Frequency / 1000000);
                total += us;
                Console.WriteLine($"{what}: {ms} ms ({us} us) in {loops} iterations. {us/loops} us/run");
            }
            Console.WriteLine($"{what}: {total} total in {runs * loops} iterations. {total / (runs * loops)} us/run");
        }
    }
}
