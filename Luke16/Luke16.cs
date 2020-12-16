using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke16
{
    class Luke16
    {
        static void Main(string[] args)
        {
            int count = 0;
            Performance.TimeRun("naive", () =>
            {
                count = Enumerable.Range(1, 1000000 - 1).AsParallel().Count(n => IsRikeligSquared(n));
            },1,1,0);
            Console.WriteLine("Count: " + count);
        }

        static bool IsRikeligSquared(int n)
        {
            var n2 = n * 2;
            var sumDivisors = SumDivisors(n);
            var diff = sumDivisors - n2;
            return diff > 0 && IsPerfectSquare(diff);
        }

        static bool IsPerfectSquare(int n)
        {
            int t = (int)Math.Floor(Math.Sqrt((double)n) + 0.5);
            return t * t == n;
        }

        static int SumDivisors(int n)
        {
            int sum = n;
            for (int i = 1; i <= n / 2; i++)
            {
                if (n % i == 0) sum += i;
            }
            return sum;
        }

    }
}
