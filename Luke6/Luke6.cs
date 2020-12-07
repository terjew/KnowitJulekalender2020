using System;
using System.Linq;
using Utilities;

namespace Luke6
{
    class Luke6
    {
        static void Main(string[] args)
        {
            int numElves = 127;
            var bagSizes = TextFile.ReadPositiveIntsList("godteri.txt");
            var sum = bagSizes.Sum();
            int i = 1;
            while ((sum % numElves) != 0)
            {
                sum -= bagSizes[bagSizes.Count - i++];
            }
            Console.WriteLine($"Hver alv får {sum / numElves} biter");
        }
    }
}
