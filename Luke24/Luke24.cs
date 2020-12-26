using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace Luke24
{
    class Luke24
    {
        const byte MAT = (byte)'1';

        static void Main(string[] args)
        {
            int count = 0;
            Performance.Benchmark("File.ReadAllBytes", () => File.ReadAllBytes("rute.txt"));
            Performance.Benchmark("CountHousesTasks", () => count = CountHousesTasks("rute.txt"));
            Performance.Benchmark("CountHousesParallelFor", () => count = CountHousesParallelFor("rute.txt"), 10, 10, 2);
            Performance.Benchmark("ReadAllBytesPLinq", () => count = CountHousesReadAllBytesPLinq("rute.txt"));
            Performance.Benchmark("ReadAllBytesLinq", () => count = CountHousesReadAllBytesLinq("rute.txt"));
            Performance.Benchmark("ReadAllBytes", () => count = CountHousesReadAllBytes("rute.txt"));
            Performance.Benchmark("BufferedStream", () => count = CountHousesBufferedStream("rute.txt"));
            Console.WriteLine(count);
        }

        static int CountHousesBufferedStream(string path)
        {
            using FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            int mat = 10;
            using (BufferedStream bs = new BufferedStream(fs))
            {
                const int n = 65536;
                byte[] bytes = new byte[n];
                int available = 0;
                while ((available = bs.Read(bytes, 0, n)) != 0)
                {
                    for (int i = 0; i < available; i++)
                    {
                        if (bytes[i] == MAT)
                        {
                            mat += 2;
                        }
                    }
                }
            }
            return mat;
        }

        static int CountHousesReadAllBytes(string path)
        {
            int mat = 10;
            var bytes = File.ReadAllBytes(path);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == MAT)
                {
                    mat += 2;
                }
            }
            return mat;
        }

        static int CountHousesReadAllBytesLinq(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return bytes.Count(b => b == MAT) * 2 + 10;
        }

        static int CountHousesReadAllBytesPLinq(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return bytes.AsParallel().Count(b => b == MAT) * 2 + 10;
        }

        static int CountHousesParallelFor(string path)
        {
            int mat = 0;
            var bytes = File.ReadAllBytes(path);
            int chunk = 256;
            Parallel.For(0, bytes.Length / chunk, (i) =>
            {
                int start = i * chunk;
                int end = Math.Min(start + chunk, bytes.Length);
                for (int j = start; j < start + chunk; j++)
                {
                    if (bytes[j] == MAT) Interlocked.Increment(ref mat);
                }
            });
            return mat * 2 + 10;
        }

        static int CountHousesTasks(string path)
        {
            int num = 16;
            int[] counts = new int[num];
            Task[] tasks = new Task[num];
            var bytes = File.ReadAllBytes(path);
            int len = bytes.Length;
            int chunk = len / num + 1;
            for (int i = 0; i < num; i++)
            {
                int threadnum = i;
                int start = chunk * i;
                int end = Math.Min(chunk * (i + 1), len);
                tasks[i] = Task.Run(() =>
                {
                    int count = 0;
                    for (int j = start; j < end; j++)
                    {
                        if (bytes[j] == MAT) count++;
                    }
                    counts[threadnum] = count;
                });
            }
            Task.WaitAll(tasks);
            int total = 0; 
            for (int i = 0; i < num; i++)
            {
                total += counts[i];
            }
            return total * 2 + 10;
        }

    }
}
