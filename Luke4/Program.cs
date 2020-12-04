using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke4
{
    class Program
    {
        static void Main(string[] args)
        {
            long antallkaker = 0;
            Performance.TimeRun("Read and parse", () =>
            {
                var ingredienser = Sum(TextFile.ReadStringList("leveringsliste.txt"));
                var muligekaker = new[]
                {
                    ingredienser["sukker"] / 2,
                    ingredienser["mel"] / 3,
                    ingredienser["melk"] / 3,
                    ingredienser["egg"]
                };
                antallkaker = muligekaker.Min();
            });

            Console.WriteLine($"Det ble bakt {antallkaker} kaker");
        }

        public class IngredientAmount
        {
            public string Ingredient { get; set; }
            public long Amount { get; set; }
        }

        static Dictionary<string, long> Sum(List<string> lines)
        {
            return
                lines.Select(l => l
                    .Split(", ")
                    .Select(token => token.Split(": "))
                    .Select(pair => new IngredientAmount { Ingredient = pair[0], Amount = long.Parse(pair[1]) })
                )
                .SelectMany(l => l)
                .GroupBy(entry => entry.Ingredient)
                .Select(g => new IngredientAmount { Ingredient = g.Key, Amount = g.Sum(entry => entry.Amount) })
                .ToDictionary(entry => entry.Ingredient, entry => entry.Amount);

        }
    }
}
