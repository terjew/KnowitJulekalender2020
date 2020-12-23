using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke14
{
    class Luke14
    {
        private static bool[] GeneratePrimeTruthTable(int maxPrime, bool inverted = true)
        {
            var maxSquareRoot = (int)Math.Sqrt(maxPrime);
            var eliminated = new bool[maxPrime + 1];
            eliminated[0] = true;
            eliminated[1] = true;
            for (int j = 4; j <= maxPrime; j += 2)
            {
                eliminated[j] = true;
            }

            for (int i = 3; i <= maxPrime; i += 2)
            {
                if (!eliminated[i])
                {
                    if (i <= maxSquareRoot)
                    {
                        for (int j = i * i; j <= maxPrime; j += i)
                        {
                            eliminated[j] = true;
                        }
                    }
                }
            }
            if (inverted)
            {
                return eliminated;
            }
            else
            {
                return eliminated.Select(b => !b).ToArray();
            }
        }

        static void Main(string[] args)
        {
            int count = 0;
            Performance.Benchmark("Solve with sieve", () =>
            {
                List<int> sequence = new List<int>();
                sequence.Add(0);
                sequence.Add(1);
                HashSet<int> allInSequence = new HashSet<int>();
                allInSequence.Add(0);
                allInSequence.Add(1);
                int i_2 = 0;
                int i_1 = 1;
                for (int i = 2; i < 1800813; i++)
                {
                    var curr = i_2 - i;
                    if (curr < 0 || allInSequence.Contains(curr))
                    {
                        curr = i_2 + i;
                    }
                    sequence.Add(curr);
                    allInSequence.Add(curr);
                    i_2 = i_1;
                    i_1 = curr;
                }

                var primeTable = GeneratePrimeTruthTable(sequence.Max(), false);
                count = sequence.Count(num => primeTable[num]);
            },1,1);
            Console.WriteLine($"Sekvensen inneholder {count} primtall");
        }
    }
}
