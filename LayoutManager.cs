using System;

namespace LayoutMod
{
    public static class DrawController {
        static private char[,] Display = new char[256,256];
        static private short MaxX {get; set;}
        static private short MaxY {get; set;}

        static internal void CastOnDisplayBuffer(UIElement element) {
            for (short y = 0; y < element.Height; y++) {
                for (short x = 0; x < element.Width; x++) {
                    if (y + element.Y < MaxY || x + element.X < MaxX) {
                        DrawController.Display[y + element.Y, x + element.X] = element.Content[y][x];
                    }
                }
            }
        }

        static public void ReinitialiseDisplayBuffer(char defaultChar = ' ') {
            AdjustConsoleBufferSize(default, -4);
            for (int y = 0; y < MaxY; y++) {
                for (int x = 0; x < MaxX; x++){
                    Display[y, x] = defaultChar;
                }
            }
        }

        static internal void Draw() {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            AdjustConsoleBufferSize(default, -4);

            for (short y = 0; y < MaxY; y++) {
                string rowContent = "";
                for (short x = 0; x < MaxX; x++) {
                    rowContent += Display[y, x];
                }
                Console.Write(rowContent + "\n");
            }
            Console.WriteLine(MaxX + "x" + MaxY);
        }

        static void AdjustConsoleBufferSize(short correctionX = 0, short correctionY = 0) {
            MaxX = (short) (Console.BufferWidth + correctionX);
            MaxY = (short) (Console.BufferHeight + correctionY);
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
        public TextEditorWindow(short x, short y, short width, short height) : base(x, y) {
            Width = width;
            Height = height;
            Content = AffirmFixedContent();
        }

        public string[] AffirmFixedContent() {
            List<string> result = new List<string> ();
            for (short y = 0; y < Height; y++) {
                string rowContent = "";
                for (short x = 0; x < Width; x++) {
                    if (y == 0 || y == Height -1) rowContent += '-';
                    else if (x == 0 || x == Width -1) rowContent += '|';
                    else rowContent += ' ';
                }
                result.Add(rowContent);
            }
            return result.ToArray();
        }
    }
}