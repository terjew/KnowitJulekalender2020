using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace Luke17
{
    class Luke17
    {
        static readonly string vacuumText =
            "  sss  \n" +
            " sssss \n" +
            "sssssss\n" +
            "sssXsss\n" +
            "sssssss\n" +
            " sssss \n" +
            "  sss  ";

        static readonly string brushText =
            "kkk   kkk\n" +
            "kkkkkkkkk\n" +
            "kkkkkkkkk\n" +
            " kkkkkkk \n" +
            " kkkXkkk \n" +
            " kkkkkkk \n" +
            "kkkkkkkkk\n" +
            "kkkkkkkkk\n" + 
            "kkk   kkk";

        static int Solve()
        {
            var lines = TextFile.ReadStringList("kart.txt");
            var map = MatrixFactory.BuildCharMatrix(lines);
            var vacuum = MatrixFactory.BuildCharMatrix(vacuumText.Split("\n").ToList());
            var brush = MatrixFactory.BuildCharMatrix(brushText.Split("\n").ToList());

            for (int y = 0; y <= map.Height - vacuum.Height; y++)
            {
                for (int x = 0; x <= map.Width - vacuum.Width; x++)
                {
                    if (!AnyOverlappingValue(map, vacuum, x, y, 'x'))
                    {
                        StampValue(map, brush, x - 1, y - 1, '.');
                    }
                }
            }
            return map.Array.Count(c => c == ' ');
        }

        static int SolveMT()
        {
            var lines = TextFile.ReadStringList("kart.txt");
            var map = MatrixFactory.BuildCharMatrix(lines);
            var vacuum = MatrixFactory.BuildCharMatrix(vacuumText.Split("\n").ToList());
            var brush = MatrixFactory.BuildCharMatrix(brushText.Split("\n").ToList());

            int nx = 5;
            int ny = 5;
            Task[] tasks = new Task[nx * ny];

            int chunkwidth = map.Width / nx;
            int chunkheight = map.Height / nx;

            for(int chunky = 0; chunky < ny; chunky++)
            {
                for (int chunkx = 0; chunkx < nx; chunkx++)
                {
                    int cx = chunkx * chunkwidth;
                    int cy = chunky * chunkheight;
                    tasks[chunky * nx + chunkx] = Task.Run(() =>
                    {
                        for (int y = cy; y < cy + chunkheight; y++)
                        {
                            if (y > map.Height - vacuum.Height) continue;
                            for (int x = cx; x < cx + chunkwidth; x++)
                            {
                                if (x > map.Width - vacuum.Width) continue;
                                if (!AnyOverlappingValue(map, vacuum, x, y, 'x'))
                                {
                                    StampValue(map, brush, x - 1, y - 1, '.');
                                }
                            }
                        }
                    }
                    );
                }
            }

            Task.WaitAll(tasks);
            return map.Array.Count(c => c == ' ');
        }

        static void Main(string[] args)
        {
            int dirtyLeft = 0;
            Performance.Benchmark("Solve (naive)", () =>
            {
                dirtyLeft = Solve();
            }, 5, 1, 2);
            Console.WriteLine(dirtyLeft);
            Performance.Benchmark("Solve (mt)", () =>
            {
                dirtyLeft = SolveMT();
            }, 5, 1, 2);
            Console.WriteLine(dirtyLeft);
        }

        public static bool AnyOverlappingValue(Matrix<char> map, Matrix<char> sprite, int spriteX, int spriteY, char value)
        {
            for (int y = 0; y < sprite.Height; y++)
            {
                for (int x = 0; x < sprite.Width; x++)
                {
                    if (map[spriteX + x, spriteY + y] == value && sprite[x, y] != ' ') return true;
                }
            }
            return false;
        }

        public static bool StampValue(Matrix<char> map, Matrix<char> sprite, int spriteX, int spriteY, char value)
        {
            for (int y = 0; y < sprite.Height; y++)
            {
                var py = spriteY + y;
                if (py < 0 || py >= map.Height) continue;
                for (int x = 0; x < sprite.Width; x++)
                {
                    var px = spriteX + x;
                    if (px < 0 || px >= map.Width) continue;
                    var pixel = map[px, py];
                    if ((pixel == ' ') && sprite[x, y] != ' ') map[px, py] = value;
                }
            }
            return false;
        }
    }
}
