using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Luke8
{
    class Location
    {
        public string Name;
        public int X;
        public int Y;
        public int TimeSaved;
        public Location(string line)
        {
            var segments = line.Split(new[] { ": (", ", ", ")" }, StringSplitOptions.RemoveEmptyEntries);
            Name = segments[0];
            X = int.Parse(segments[1]);
            Y = int.Parse(segments[2]);
            TimeSaved = 0;
        }

        public void StepTime(int santaX, int santaY)
        {
            var dist = Math.Abs(santaX - X) + Math.Abs(santaY - Y);
            if (dist >= 50) return;
            else if (dist >= 20) TimeSaved += 1;
            else if (dist >= 5) TimeSaved += 2;
            else if (dist > 0) TimeSaved += 3;
            else TimeSaved += 4;
        }
    }

    class Quadrant
    {
        public int MinX;
        public int MinY;
        public int MaxX;
        public int MaxY;
        public Location[] Locations;
    }

    class Solver
    {
        private int santaX;
        private int santaY;
        private int width;
        private int height;
        private Quadrant[] quadrants;
        private Dictionary<string, Location> locationDict;
        private List<string> route;

        private void StepAll()
        {
            var qx = santaX / 50;
            var qy = santaY / 50;
            int startx = Math.Max(0, qx - 1);
            int endx = Math.Min(width - 1, qx + 1);
            int starty = Math.Max(0, qy - 1);
            int endy = Math.Min(height - 1, qy + 1);
            for (int y = starty; y <= endy; y++)
            {
                for (int x = startx; x <= endx; x++)
                {
                    var quadrant = quadrants[y * width + x];
                    foreach (var location in quadrant.Locations) location.StepTime(santaX, santaY);
                }
            }
        }

        public Solver(string filename)
        {
            var input = TextFile.ReadStringList("input.txt");
            locationDict = new Dictionary<string, Location>();
            route = new List<string>();
            foreach (var line in input)
            {
                if (line.Length < 1) continue;
                if (line.Contains(':'))
                {
                    var location = new Location(line);
                    locationDict.Add(location.Name, location);
                }
                else
                {
                    route.Add(line);
                }
            }

            var locations = locationDict.Values.ToArray();
            width = 1 + locations.Max(l => l.X) / 50;
            height = 1 + locations.Max(l => l.Y) / 50;

            quadrants = new Quadrant[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var minX = x * 50;
                    var maxX = x * 50 + 49;
                    var minY = y * 50;
                    var maxY = y * 50 + 49;
                    var quadrantLocations = locations.Where(l =>
                       l.X >= minX &&
                       l.X <= maxX &&
                       l.Y >= minY &&
                       l.Y <= maxY
                    );
                    quadrants[y * width + x] = new Quadrant()
                    {
                        MinX = minX,
                        MinY = minY,
                        MaxX = maxX,
                        MaxY = maxY,
                        Locations = quadrantLocations.ToArray()
                    };

                }
            }
        }
        public float Solve()
        {
            santaX = 0;
            santaY = 0;
            foreach (var destination in route)
            {
                //FIXME: Calculate total effect of entire segment at once, for each affected location.
                var targetX = locationDict[destination].X;
                int dx = targetX - santaX > 0 ? 1 : -1;
                while (targetX != santaX)
                {
                    santaX += dx;
                    StepAll();
                }


                var targetY = locationDict[destination].Y;
                int dy = targetY - santaY > 0 ? 1 : -1;
                while (targetY != santaY)
                {
                    santaY += dy;
                    StepAll();
                }
            }

            float min = float.MaxValue;
            float max = 0;
            foreach (var location in quadrants.SelectMany(q => q.Locations))
            {
                var saved = location.TimeSaved;
                if (saved < min) min = saved;
                if (saved > max) max = saved;
            }
            return max - min;
        }
    }

    class Luke8
    {
        static void Main(string[] args)
        {
            float maxDiff = 0;
            Performance.Benchmark("read and simulate", () =>
            {
                var solver = new Solver("input.txt");
                maxDiff = solver.Solve();
            },10,10);
            Console.WriteLine($"Maksimal tidsforskjell er {maxDiff * 0.25f} sekunder");
        }

        
    }
}
