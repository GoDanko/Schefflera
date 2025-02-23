using System;
using System.ComponentModel;
using System.Dynamic;
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

        static internal void Draw(UIElement specificElement) { // THIS METHOD CRASHES (when running it with DiagnostiMsg)
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
        public string[]? lines = null;
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

        public string AdjustStringSize(string input, int length, char defaultChar = ' ') {
            string result = input;
            for (int i = input.Length; i < length; i++) {
                result += defaultChar;
            }
            return result;
        }
    }

    public class TextEditorWindow : UIElement {

        public int editorLines {get; set;}

        public TextEditorWindow(short x, short y, short width, short height) : base(x, y) {
            Width = width;
            Height = height;
            lines = UpdateContent("Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem eum fugiat, quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti, quos dolores et quas molestias excepturi sint, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat");
            // UpdateContent seems to return an empty array, despite the lorem ipsum, check why.
            Content = AffirmFixedContent();
        }
        
        string[] UpdateContent(string input) {
            return UpdateContent(TextEditor.StringToCharArray(input));
        }

        string[] UpdateContent(char[] input) {
            List<string> result = new List<string> ();
            int lineLength = Width - 2;
            if (input.Length < lineLength) {
                short addedLines = 0;
                string line = "";

                for (int i = 0; i < input.Length; i++) {
                    string word = "";

                    for (int j = i; input[j] == ' '; j++) {
                        word += input[j];
                        i = j + 1;
                    }

                    if (line.Length + word.Length > lineLength) {
                        result.Add(line);
                        line = word + ' ';
                        addedLines++;
                    } else {
                        line += word + ' ';
                    }
                }
            }
            return result.ToArray();
        }

        public string[] AffirmFixedContent() {
            List<string> result = new List<string> ();
            for (short y = 0; y < Height; y++) {
                string rowContent = "|";
                if (y == 0 || y == Height -1) {
                    rowContent = "-";
                    for (int i = 1; i < Width; i++) rowContent += '-';
                } else if (lines != null && lines.Length > y) {
                    rowContent += AdjustStringSize(lines[y - 1], Width - 2) + '|';
                } else {
                    for (int i = 0; i < Width - 2; i++) rowContent += ' ';
                    rowContent += '|';
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

        public DiagnosticMsg(string DiagnosticMessage) : base(1, (short) (DrawController.MaxY)) {
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