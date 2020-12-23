using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Luke16
{
    class Luke16
    {
        static void Main(string[] args)
        {
            int count = 0;

            Performance.Benchmark(nameof(IsPerfectSquare), () =>
            {
                count = 0;
                for (int i = 0; i < 1_000_000; i++) if (IsPerfectSquare(i)) count++;
            });

            Performance.Benchmark(nameof(IsPerfectSquareOptimized), () =>
            {
                count = 0; 
                for (int i = 0; i < 1_000_000; i++) if (IsPerfectSquareOptimized(i)) count++;
            },1000,10);
            Console.WriteLine("Count: " + count);

            Performance.Benchmark(nameof(CountSquareAbundantMT), () =>
            {
                count = CountSquareAbundantMT(1000000);
            });
            Console.WriteLine("Count: " + count);

            Performance.Benchmark(nameof(CountSquareAbundantST), () =>
            {
                count = CountSquareAbundantST(1000000);
            });
            Console.WriteLine("Count: " + count);
        }

        static int CountSquareAbundantST(int max)
        {
            int[] sumDivisors = new int[max];
            Array.Fill(sumDivisors, 1);
            sumDivisors[0] = 0;
            for (int divisor = 2; divisor < max; divisor++)
            {
                for (int number = divisor * 2; number < max; number += divisor)
                {
                    sumDivisors[number] += divisor;
                }
            }

            int count = 0;
            for (int i = 2; i < max; i++)
            {
                var diff = sumDivisors[i] - i;
                if (diff > 0 && IsPerfectSquareOptimized(diff)) count++;
            }

            return count;
        }


        static int CountSquareAbundantMT(int max)
        {
            int[] sumDivisors = new int[max];
            Array.Fill(sumDivisors, 1);
            sumDivisors[0] = 0;
            for (int divisor = 2; divisor < max; divisor++)
            {
                for (int number = divisor * 2; number < max; number += divisor)
                {
                    sumDivisors[number] += divisor;
                }
            }

            return sumDivisors.AsParallel().Where((v, i) =>
            {
                var diff = v - i;
                return (diff > 0 && IsPerfectSquareOptimized(diff));
            }).Count();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsPerfectSquareOptimized(int n)
        {
            if ((0x202021202030213 & (1L << (int)(n & 63))) > 0)
            {
                long t = (long)Math.Round(Math.Sqrt((double)n));
                return t * t == n;
            }
            return false;
        }

        static bool IsPerfectSquare(int n)
        {
            long t = (long)Math.Round(Math.Sqrt((double)n));
            return t * t == n;
        }

    }
}
