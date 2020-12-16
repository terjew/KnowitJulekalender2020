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

            Performance.TimeRun(nameof(IsPerfectSquare), () =>
            {
                for (int i = 0; i < 1_000_000; i++) IsPerfectSquare(i);
            });

            Performance.TimeRun(nameof(IsPerfectSquareOptimized), () =>
            {
                for (int i = 0; i < 1_000_000; i++) IsPerfectSquareOptimized(i);
            },1000,10);

            Performance.TimeRun(nameof(CountSquareAbundantMT), () =>
            {
                count = CountSquareAbundantMT(1000000);
            }, 10, 10, 5);
            Console.WriteLine("Count: " + count);

            Performance.TimeRun(nameof(CountSquareAbundantST), () =>
            {
                count = CountSquareAbundantST(1000000);
            }, 10, 10, 5);
            Console.WriteLine("Count: " + count);

            Performance.TimeRun(nameof(CountKnowitAnders), () =>
            {
                count = CountKnowitAnders();
            }, 10, 10, 5);
            Console.WriteLine("Count: " + count);
        }

        static int CountKnowitAnders()
        {
            int NUMBER_LIMIT = 1_000_000;
            int SQUARE_LIMIT = 2_400_000;
            // Build table of squares.
            HashSet<int> squares = new HashSet<int>();
            int factor = 1, square = 1;
            while (square < SQUARE_LIMIT)
            {
                squares.Add(square);
                factor++;
                square = factor * factor;
            }

            // Get the abundant numbers.
            // https://math.stackexchange.com/questions/561328/finding-abundant-numbers-from-1-to-10-million-using-a-sum
            int[] sigma = new int[NUMBER_LIMIT];
            Array.Fill(sigma, 1);
            for (int i = 2; i < NUMBER_LIMIT; i++)
            {
                for (int j = i * 2; j < NUMBER_LIMIT; j += i)
                {
                    sigma[j] += i;
                }
            }
            int squareCount = 0;
            for (int i = 2; i < NUMBER_LIMIT; i++)
            {
                if (sigma[i] > i)
                {
                    // This is an abundant number.
                    if (squares.Contains(sigma[i] - i))
                    {
                        squareCount++;
                    }
                }
            }
            return squareCount;
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
