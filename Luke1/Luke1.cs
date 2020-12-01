using System;
using System.IO;
using System.Linq;
using Utilities;

namespace Luke1
{
    class Luke1
    {
        static void Main(string[] args)
        {
            long num = 0;

            Performance.TimeRun("SolveSum", () =>
            {
                num = SolveSum();
            });
            Console.WriteLine(num);

            Performance.TimeRun("SolveSumInlineRead", () =>
            {
                num = SolveSumInlineRead();
            });
            Console.WriteLine(num);

            Performance.TimeRun("SolveArray", () =>
            {
                num = SolveArray();
            });
            Console.WriteLine(num);
        }

        static int SolveArray()
        {
            byte[] arr = new byte[100000];
            var numbers = TextFile.ReadPositiveIntsList("numbers.txt");
            foreach (int i in numbers)
            {
                arr[i - 1] = 1;
            }
            for (int i = 0; i < 100000; i++)
            {
                if (arr[i] == 0)
                {
                    return i + 1;
                }
            }
            return -1;
        }

        static int SolveSum()
        {
            int n = 100000;
            int expectedSum = (n * (n + 1)) / 2;
            var numbers = TextFile.ReadPositiveIntsList("numbers.txt");
            int sum = 0;
            foreach (int i in numbers)
            {
                sum += i;
            }
            return (int)(expectedSum - sum);
        }

        static int SolveSumInlineRead()
        {
            int n = 100000;
            int expectedSum = (n * (n + 1)) / 2;
            int sum = 0;

            using (FileStream fs = File.Open("numbers.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    int y = 0;
                    const int bufsize = 65536;
                    byte[] bytes = new byte[bufsize];
                    int available = 0;
                    while ((available = bs.Read(bytes, 0, bufsize)) != 0)
                    {
                        for (int i = 0; i < available; i++)
                        {
                            byte b = bytes[i];
                            if (b == ',')
                            {
                                sum += y;
                                y = 0;
                            }
                            else
                            {
                                y = y * 10 + (b - '0');
                            }
                        }
                    }
                    sum += y;
                }
            }
            return expectedSum - sum;
        }

    }
}
