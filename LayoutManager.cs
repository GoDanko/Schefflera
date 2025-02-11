using System;

namespace Layout
{
    public static class DrawController {
        static internal char[,] Display = new char[256,256];
        static private short MaxX {get; set;}
        static private short MaxY {get; set;}

        static internal void CastOnDisplayBuffer(UIElement element) {
            for (short y = 0; y < element.Height; y++) {
                // for (short i = 0; i < element.X; i++) {
                //     if (DrawController.Display[y + element.Y, i] != '\0') continue;
                //     else DrawController.Display[y + element.Y, i] = '\0';
                // }
                for (short x = 0; x < element.Width; x++) {
                    if (y + element.Y < MaxY || x + element.X < MaxX) {
                        DrawController.Display[y + element.Y, x + element.X] = element.Content[y][x];
                    }
                }
            }
        }

        static public void ClearDisplayBuffer(char defaultChar = ' ') {
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

        static internal void Draw(UIElement element) {
            CastOnDisplayBuffer(element);
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
        private readonly string[] FixedContent;

        public TextEditorWindow(short x, short y, short width, short height) : base(x, y) {
            Width = width;
            Height = height;
        }

        public void AffirmFixedContent() {
            List<string> result = new List<string> ();
            for (short x = 0; x < Width; x++) {
                string rowContent = "";
                for (short y = 0; y < Height; y++) {
                    if (x == 1 || x == Height) rowContent += '-';
                    else if (y == 1 || y == Width) rowContent += '|';
                    else {
                        // if (Content[y][x] != ' ') rowContent += Content[y][x];
                        // else 
                        rowContent += ' ';
                    };
                }
            }
        }
    }
}