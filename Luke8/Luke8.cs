using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities;

namespace Luke8
{
    class Location
    {
        public string Name;
        public Point Pos;
        public int TimeSaved;
        public Location(string line)
        {
            var segments = line.Split(new[] { ": (", ", ", ")" }, StringSplitOptions.RemoveEmptyEntries);
            Name = segments[0];
            Pos = new Point(int.Parse(segments[1]), int.Parse(segments[2]));
            TimeSaved = 0;
        }

        public void StepTime(Point santaPosition)
        {
            var dist = Math.Abs(santaPosition.X - Pos.X) + Math.Abs(santaPosition.Y - Pos.Y);
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

    class Luke8
    {
        static void Main(string[] args)
        {
            float maxDiff = 0;
            Performance.TimeRun("read and simulate", () =>
            {
                var input = TextFile.ReadStringList("input.txt");
                Dictionary<string, Location> locationDict = new Dictionary<string, Location>();
                List<string> route = new List<string>();
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
                int width = 1 + locations.Max(l => l.Pos.X) / 50;
                int height = 1 + locations.Max(l => l.Pos.Y) / 50;

                Quadrant[] quadrants = new Quadrant[width * height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var minX = x * 50;
                        var maxX = x * 50 + 49;
                        var minY = y * 50;
                        var maxY = y * 50 + 49;
                        var quadrantLocations = locations.Where( l => 
                            l.Pos.X >= minX &&
                            l.Pos.X <= maxX &&
                            l.Pos.Y >= minY &&
                            l.Pos.Y <= maxY
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

                Point pos = new Point(0, 0);
                foreach (var destination in route)
                {
                    var target = locationDict[destination].Pos;
                    Size vector = new Size(target.X - pos.X > 0 ? 1 : -1, 0);
                    while (target.X != pos.X)
                    {
                        pos += vector;
                        StepAll(pos, width, height, quadrants);
                    }
                    vector = new Size(0, target.Y - pos.Y > 0 ? 1 : -1);
                    while (target.Y != pos.Y)
                    {
                        pos += vector;
                        StepAll(pos, width, height, quadrants);
                    }
                }

                var ordered = locations.OrderBy(l => l.TimeSaved).ToArray();
                maxDiff = Math.Abs(ordered.First().TimeSaved - ordered.Last().TimeSaved);
            },10,10);
            Console.WriteLine($"Maksimal tidsforskjell er {maxDiff * 0.25f} sekunder");
        }

        private static void StepAll(Point pos, int width, int height, Quadrant[] quadrants)
        {
            var qx = pos.X / 50;
            var qy = pos.Y / 50;
            int startx = Math.Max(0, qx - 1);
            int endx = Math.Min(width - 1, qx + 1);
            int starty = Math.Max(0, qy - 1);
            int endy = Math.Min(height - 1, qy + 1);
            for (int y = starty; y <= endy; y++)
            {
                for (int x = startx; x <= endx; x++)
                {
                    var quadrant = quadrants[y * width + x];
                    foreach (var location in quadrant.Locations) location.StepTime(pos);
                }
            }

        }
    }
}
