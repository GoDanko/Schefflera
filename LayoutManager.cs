using System;

namespace Layout
{
    public class FrameBuilder {
        public short MaxX {get; set;}
        public short MaxY {get; set;}
        public char[,] Display {get; set;}
        public FrameBuilder() {
            Display = new char[,] {};
        }

        // char[,] InitializeDisplay() {    // FIGURE A WAY TO INITIALIZE Display
        //     List<List<char>> result = new List<List<char>> ();
        //     return result.ToArray();
        // }

        void Draw() {
            int yLength = Display.GetLength(0);
            int xLength = Display.GetLength(1);
            
            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    // Console.Write(Display[y][x]);    // FIGURE A WAY TO ADDRESS EACH CHAR CELL
                }
                Console.Write("\n");
            }
        }

        void Draw(UIElement uIElement) {
            int yLength = uIElement.Height + uIElement.Y;
            int xLength = uIElement.Width + uIElement.X;
            
            for (int y = uIElement.Y; y < yLength; y++) {
                for (int x = uIElement.X; x < xLength; x++) {
                    // Console.Write(Display[y][x]);
                }
                Console.Write("\n");
            }
        }
    }

    public class UIElement
    {
        private bool CantDrawX {get; set;} = false;
        private bool CantDrawY {get; set;} = false;
        private bool AbortDrawing {get; set;} = false;
        public short X {get; set;}
        public short Y {get; set;}
        public short Width {get; set;}
        public short Height {get; set;}
        public string[] Content {get; set;} = new string[] {""};

        public UIElement(short x, short y, short width, short height, bool dynamicSize = false) {
            if (x + width > Console.BufferWidth) {  // TEST RELIABILITY OF THIS APPROACH
                CantDrawX = true;
                if (dynamicSize) {
                    x = (short)(x + width - Console.BufferWidth);
                    if (x < 0) width += X;
                    if (width < 1) {
                        AbortDrawing = true;
                        x = 0;
                        width = 0;
                    } else {
                        CantDrawX = false;
                    }
                }
            }
            if (y + height > Console.BufferHeight) {
                CantDrawY = true;
                if (dynamicSize) {
                    y = (short)(y + height - Console.BufferHeight);
                    if (y < 0) height += y;
                    if (height < 1) {
                        AbortDrawing = true;
                        y = 0;
                        height = 0;
                    } else {
                        CantDrawY = false;
                    }
                }
            }

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public class TextEditor : UIElement {
        public TextEditor(short x, short y, short width, short height) : base(x, y, width, height)
        {

        }
    }
}