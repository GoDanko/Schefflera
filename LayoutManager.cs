using System;

namespace Layout
{
    public class DrawController {
        readonly public short MaxX;
        readonly public short MaxY;
        private char[,] Display {get; set;}
        public byte[,] DisplayMap {get; set;}
        public List<UIElement> UiElements {get; set;}
        public DrawController() {
            MaxX = (short)Console.BufferWidth;
            MaxY = (short)Console.BufferHeight;

            UiElements = new List<UIElement> ();
            Display = new char[,] {};
            DisplayMap = new byte[,] {};
        }

        internal char[,] InitializeDisplay(char [,] display, byte[,] displayMap) {
            for (short y = 0; y < MaxY; y++) {
                for (short x = 0; x < MaxX; x++) {
                    if (displayMap[x, y] != 0) {
                        if (UiElements[displayMap[x, y]].OnScreen && !UiElements[displayMap[x, y]].Initialised) {
                            display = PushToDisplay(UiElements[displayMap[x, y]], displayMap, display);
                        } else continue;
                    }
                }
            }
            Draw();
            return display;
        }

        private char[,] PushToDisplay(UIElement element, byte[,] displayMap, char[,] display) {
            for (short y = element.Y; y < element.Y + element.Height; y++) {
                for (short x = element.X; x < element.X + element.Width; x++) {
                    if (displayMap[x, y] == element.MappedID) {
                        display[x, y] = element.Content[x - element.X, y - element.Y];
                    }
                }
                element.Initialised = true;
            }
            return display;
        }

        void Draw() {
            for (int y = 0; y < MaxY; y++) {
                for (int x = 0; x < MaxX; x++) {
                    Console.Write(Display[y, x] == '\0' ? ' ' : Display[y, x]);
                }
                Console.SetCursorPosition(0, y +1);
            }
        }
    }

    public class UIElement
    {
        public byte MappedID {get; set;}
        internal bool CantDrawX {get; set;} = false;
        internal bool CantDrawY {get; set;} = false;
        public bool Initialised {get; set;}
        public bool OnScreen {get; set;}
        public short X {get; set;}
        public short Y {get; set;}
        public short Width {get; set;}
        public short Height {get; set;}
        public byte Z {get; set;}
        public char[,] Content {get; set;}
        private List<byte> _overlayingIDs = new List<byte> ();
        public List<byte> OverlayingIDs {
            get { return _overlayingIDs; }
            set {
                foreach (byte AddedValue in value) {
                    bool alreadyAdded = false;
                    foreach (byte ID in _overlayingIDs) {
                        if (AddedValue < 255 && AddedValue == ID) {
                            alreadyAdded = true;
                            break;
                        }
                    }
                    if (!alreadyAdded) _overlayingIDs.Add(AddedValue);
                }
            }
        }

        public UIElement(short x, short y, short width, short height, byte z = 128) {
            X = x;
            Y = y;
            Z = Z;
            Width = width;
            Height = height;
            Content = new char[x, y];
        }

        void MapElementToFrame(DrawController frame) {
            if (frame.DisplayMap != null) {
                if (frame.UiElements.Count < 256) {
                    this.MappedID = (byte)(frame.UiElements.Count + 1);
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
                            frame.DisplayMap[x, y] = this.MappedID;
                        } else {
                            if (frame.UiElements[frame.DisplayMap[x, y]].Z <= this.Z) frame.DisplayMap[x, y] = this.MappedID;
                            this.OverlayingIDs = new List<byte> {frame.DisplayMap[x, y]};
                            frame.UiElements[frame.DisplayMap[x, y]].OverlayingIDs = new List<byte>{this.MappedID};
                            continue;
                        }
                    }
                }
                frame.UiElements.Add(this);
            } else {
                Console.Write("Initialise instance of DrawController's with a DisplayMap first");
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