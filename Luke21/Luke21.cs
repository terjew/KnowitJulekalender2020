using System;
using System.IO;
using Utilities;

namespace Luke21
{
    class Luke21
    {
        static void Main(string[] args)
        {
            int treated = 0;
            Performance.TimeRun("Read+solve", () =>
            {
                treated = Solve();
            }, 10, 1000);
            Console.WriteLine($"Treated: {treated}");
        }

        static int Solve()
        {
            int[] prioritizedCases = new int[6];
            string santa = "Claus";
            int santaPriority = -1;
            int queuedBeforeSanta = -1;
            int treated = 0;
            using (StreamReader sr = File.OpenText("input.txt"))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    var last = s[^1];
                    int priority = -1;
                    if (last == '-')
                    {
                        for (int i = 1; i < 6; i++)
                        {
                            if (prioritizedCases[i] > 0)
                            {
                                prioritizedCases[i]--;
                                break;
                            }
                        }
                        treated++;
                    }
                    else
                    {
                        priority = last - '0';
                        if (s.Length == 7 && s.StartsWith(santa))
                        {
                            santaPriority = priority;
                            queuedBeforeSanta = prioritizedCases[1];
                            for (int i = 2; i <= santaPriority; i++) queuedBeforeSanta += prioritizedCases[i];
                            if (santaPriority == 1) return queuedBeforeSanta;
                            break;
                        }
                        prioritizedCases[priority]++;
                    }

                }
                while ((s = sr.ReadLine()) != null)
                {
                    var last = s[^1];
                    if (last == '-')
                    {
                        queuedBeforeSanta--;
                        treated++;
                    }
                    else
                    {
                        var priority = last - '0';
                        if (priority < santaPriority) queuedBeforeSanta++;
                    }
                }
                return treated + queuedBeforeSanta;
            }
        }
    }
}
