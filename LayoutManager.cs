using System;

namespace Layout
{
    public class FrameBuilder {
        public short MaxX {get; set;}
        public short MaxY {get; set;}
        private char[,] Display {get; set;}
        public byte[,] DisplayMap {get; set;}
        public List<UIElement> UiElements {get; set;}
        public FrameBuilder() {
            // Make sure that the UIElements can be mapped after the FrameBuilder object is created
            // and that You don't need UiElements to create the instance (Add a Draw method)

            MaxX = (short)Console.BufferWidth;
            MaxY = (short)Console.BufferHeight;

            UiElements = new List<UIElement> ();
            Display = new char[,] {};
            DisplayMap = new byte[,] {};

            // FIGURE OUT which methods will be needed to create an instance of FrameBuilder

            InitializeDisplay(Display, DisplayMap);
            Draw();
        }

        char [,] InitializeDisplay(char [,] display, byte[,] displayMap) {
            for (short y = 0; y < MaxY; y++) {
                for (short x = 0; x < MaxX; x++) {
                    if (displayMap[x, y] != 0) {
                        if (UiElements[displayMap[x, y]].OnScreen && !UiElements[displayMap[x, y]].Initialised) {
                            display = QueueNewElement(UiElements[displayMap[x, y]], display);
                        } else continue;
                    }
                }
            }
            return display;
        }

        char[,] QueueNewElement(UIElement element, char[,] display) {
            for (short y = element.Y; y < element.Y + element.Height; y++) {
                if ((element.Y + element.Height) >= MaxY) {
                    element.CantDrawY = true;
                    break;
                }
                for (short x = element.X; x < element.X + element.Width; x++) {
                    if ((element.X + element.Width) >= MaxX) {
                        element.CantDrawX = true;
                        break;
                    }
                    display[x, y] = element.Content[x - element.X][y - element.Y];
                }
                element.Initialised = true;
            }
            return display;
        }

        void Draw() {
            int yLength = Display.GetLength(0);
            int xLength = Display.GetLength(1);
            
            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    Console.Write(Display[y, x]);
                }
                Console.Write("\n");
            }
        }
    }

    public class UIElement
    {
        public byte MapID {get; set;}
        internal bool CantDrawX {get; set;} = false;
        internal bool CantDrawY {get; set;} = false;
        public bool Initialised {get; set;}
        public bool OnScreen {get; set;}
        public short X {get; set;}
        public short Y {get; set;}
        public short Width {get; set;}
        public short Height {get; set;}
        public byte Z {get; set;}
        public string[] Content {get; set;} = new string[] {""};

        public UIElement(short x, short y, short width, short height, byte z = 128) {
            X = x;
            Y = y;
            Z = Z;
            Width = width;
            Height = height;
        }

        void MapElementToFrame(FrameBuilder frame) {
            if (frame.DisplayMap != null) {
                if (frame.UiElements.Count > 256) {
                    this.MapID = (byte)(frame.UiElements.Count + 1);
                } else {
                    CantDrawX = true;
                    CantDrawY = true;
                }

                for (int y = this.Y; y < this.Y + this.Height; y++) {
                    if (y >= frame.MaxY) {
                        this.CantDrawY = true;
                        break;
                    }
                    for (int x = this.X; x < this.X + this.Width; x++) {
                        if (x >= frame.MaxX) {
                            this.CantDrawX = true;
                            break;
                        }
                        if (frame.DisplayMap[x, y] == 0 || !frame.UiElements[frame.DisplayMap[x, y]].OnScreen) {
                            frame.DisplayMap[x, y] = this.MapID;
                        } else {
                            if (frame.UiElements[frame.DisplayMap[x, y]].Z <= this.Z) frame.DisplayMap[x, y] = this.MapID;
                            else continue;
                        }
                    }
                }
                frame.UiElements.Add(this);
            } else {
                Console.Write("Initialise instance of FrameBuilder's with a DisplayMap first");
            }
        }
    }

    // CREATE an inherited instance of UIElement that manifests error messages

    public class TextEditor : UIElement {
        public TextEditor(short x, short y, short width, short height) : base(x, y, width, height)
        {

        }
    }
}