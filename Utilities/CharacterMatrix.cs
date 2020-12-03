using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Utilities
{
    public class CharacterMatrix
    {
        public List<string> Lines { get; }
        public int Width { get; }
        public int Height { get; }

        public CharacterMatrix(List<string> lines)
        {
            Lines = lines;
            Width = lines[0].Length;
            Height = lines.Count;
        }

        public char GetChar(Point p)
        {
            return Lines[p.Y][p.X];
        }

        public bool IsInside(Point p)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < Width && p.Y < Height;
        }
    }
}
