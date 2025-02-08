using System;

namespace Layout
{
    public static class DrawController {
        static internal char[,] Display = new char[256,256];
        static private short MaxX {get; set;}
        static private short MaxY {get; set;}

        static internal void CastOnDisplay(UIElement element) {
            for (short y = 0; y < element.Height; y++) {
                for (short x = 0; x < element.Width; x++) {
                    if (x >= element.Content[y].Length) break;
                    DrawController.Display[y + element.Y, x + element.X] = element.Content[y][x];
                }
            }
        }

        static internal void Draw() {
            Console.Clear();
            MaxX = (short) (Console.BufferWidth + 12); // for some reason the buffer is -12
            MaxY = (short) Console.BufferHeight;

            for (short y = 0; y < MaxY; y++) {
                string rowContent = "";
                for (short x = 0; x < MaxX; x++) {
                    rowContent += Display[y, x];
                }
                Console.Write(rowContent + "\n");
            }
            Console.WriteLine(MaxX + "x" + MaxY);
        }

        static internal void Draw(UIElement element) {
            CastOnDisplay(element);
            MaxX = (short) (Console.BufferWidth + 12); // for some reason the buffer is -12
            MaxY = (short) Console.BufferHeight;

            for (short y = 0; y < MaxY; y++) {
                string rowContent = "";
                for (short x = 0; x < MaxX; x++) {
                    rowContent += Display[y, x];
                }
                Console.Write(rowContent + "\n");
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
        private string[] content = new string[0];
        public string[] Content {
            get {return content;}
            set {
                if (value != null) {
                    Height = (short) value.Length;
                    Width = FindWidth(value);
                    content = value;
                }
            }
        }

        public UIElement(short x, short y) {
            Content = new string[0];
            X = x;
            Y = y;
        }
        
        short FindWidth(string[] content) {
            short longestRow = 0;
            foreach (string row in content) {
                if (longestRow < row.Length) longestRow = (short) row.Length;
            }
            return longestRow;
        }
    }

    public class TextEditorWindow : UIElement {
        private readonly string[] FixedContent;

        public TextEditorWindow(short x, short y, short Width, short height) : base(x, y) {

        }

        void AffirmFixedContent() {
            List<string> fixedContent = new List<string> ();
            for (int i = 0; i < Width; i++) {
                string rowContent = "";
                for (int j = 0; j < Height; j++) {
                    if (i == 0 || i == Height) rowContent += '-';
                    else if (j == 0 || j == Width) rowContent += '|';
                    else rowContent += ' '; // Modify it, so you can skip the overwriting of the boxes between,
                                            // because you probably will be using this method also to correct
                                            // every attempt at overwriting the "FixedContent", without over-
                                            // writing the inside.
                }
            }
        }
    }
}