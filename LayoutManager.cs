using System;

namespace Layout
{
    public static class DrawController {
        static internal char[,] Display = new char[256,256];
        static private short MaxX {get; set;}
        static private short MaxY {get; set;}

        static internal void Draw() {
            MaxX = (short) (Console.BufferWidth + 12); // for some reason the buffer is -12 than in reality
            MaxY = (short) Console.BufferHeight;

            for (short y = 0; y < MaxY; y++) {
                for (short x = 0; x < MaxX; x++) {
                    Console.Write(Display[y, x]);
                }
                Console.Write("\n");
            }
            Console.WriteLine(MaxX + "x" + MaxY);
        }
    }

    public class UIElement
    {
        public short X {get; set;}
        public short Y {get; set;}
        public short Width {get; set;}
        public short Height {get; set;}
        public byte Z {get; set;}
        public string[] Content {get; set;}

        public UIElement(string[] content, short x, short y, byte z = 128) {
            X = x;
            Y = y;
            Z = z;
            Height = (short) content.Length;
            Width = FindWidth(content);
            Content = content;
        }

        internal void CastOnDisplay() {
            for (short y = 0; y < Height; y++) {
                for (short x = 0; x < Width; x++) {
                    if (x >= Content[y].Length) break;
                    DrawController.Display[y + Y, x + X] = Content[y][x];
                }
            }
        }
        
        short FindWidth(string[] content) {
            short longestRow = 0;
            foreach (string row in content) {
                if (longestRow < row.Length) longestRow = (short) row.Length;
            }
            return longestRow;
        }
    }
}