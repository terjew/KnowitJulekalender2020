using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Utilities;

namespace Luke5
{
    class Program
    {
        static Size GetDirection(char c)
        {
            switch (c)
            {
                case 'O': return new Size(0, 1);
                case 'N': return new Size(0, -1);
                case 'V': return new Size(-1, 0);
                case 'H': return new Size(1, 0);
            }
            return new Size(0, 0);
        }

        static void Main(string[] args)
        {
            float area = 0;
            Performance.TimeRun("Read and calculate", () =>
            {
                var corners = new List<Point>();
                var pos = new Point(0, 0);
                corners.Add(pos);
                var route = File.ReadAllText("rute.txt");
                int prev = route[0];
                foreach (var c in route)
                {
                    if (c != prev)
                    {
                        corners.Add(pos);
                        prev = c;
                    }
                    pos += GetDirection(c);
                }
                area = GetArea(corners);
            });
            Console.WriteLine($"Arealet er {area}");
        }
        
        static float GetArea(List<Point> corners)
        {
            float sum = 0;
            Point prev = corners[corners.Count - 1];
            foreach(var corner in corners)
            {
                sum += prev.X * corner.Y - prev.Y * corner.X;
                prev = corner;
            }
            return sum / 2;
        }


    }
}
