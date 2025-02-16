using System;
using System.ComponentModel;
using TextMod;

namespace LayoutMod
{
    public static class DrawController {
        static private char[,] Display = new char[256,256];
        static private short maxX {get; set;}
        static private short maxY {get; set;}
        static internal short MaxX {get {return maxX;} set {maxX = value;}}
        static internal short MaxY {get{return maxY;} set {maxY = value;}}

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

        static internal void Draw(UIElement specificElement) {
            AdjustConsoleBufferSize(default);
            Console.SetCursorPosition(specificElement.X, specificElement.Y);

            for (short y = 0; y < specificElement.Height; y++) {
                string rowContent = "";
                if (y >= MaxY) {
                    new DiagnosticMsg("Avoided Overflow on Y axis");
                    break;
                }
                for (short x = 0; x < specificElement.Width; x++) {
                    rowContent += specificElement.Content[y][x];
                    if (x >= MaxX) {
                        new DiagnosticMsg("Avoided Overflow on X axis");
                        break;
                    }
                }
                Console.Write(rowContent);
                Console.SetCursorPosition(specificElement.X, specificElement.Y + y + 1);
            }
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
                    // Thread.Sleep(300);
                    if (y == 0 || y == Height -1) rowContent += '-';
                    else if (x == 0 || x == Width -1) rowContent += '|';
                    else rowContent += ' ';
                }
                result.Add(rowContent);
            }
            return result.ToArray();
        }

        internal void AccessEditor() {
            TextEditor editingLogic = new TextEditor(this);
            
            bool leaveLoop = false;
            do {
                leaveLoop = editingLogic.RequestKey();
            } while (!leaveLoop);
        }
    }

    public class DiagnosticMsg : UIElement {
        private (int, int) PreviousCursorPosition {get; set;}

        public DiagnosticMsg(string DiagnosticMessage) : base(1, (short)(DrawController.MaxY - 3)) {
            PreviousCursorPosition = Console.GetCursorPosition();

            string aestheticLine = "";
            foreach (char countChar in DiagnosticMessage) aestheticLine += '~';
            Content = new string[] {aestheticLine, DiagnosticMessage, aestheticLine};

            DrawController.Draw(this); // This if repeated kills the program

            Console.SetCursorPosition(PreviousCursorPosition.Item1, PreviousCursorPosition.Item2);
            Console.ReadKey(false);
        }
    }
}