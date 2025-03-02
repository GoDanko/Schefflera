using System;
using System.Runtime.CompilerServices;
using LayoutMod;

namespace TextMod
{
    internal class TextEditor
    {
        public char[] Text {get; set;}    // You can find a way to cut the text into pieces based on the Width of the editor
        private short Lines {get; set;}
        public byte CurrentTextIndex;
        private short FirstLineIndex {get; set;}
        private char PressedKeyChar;
        private TextEditorWindow EditorWindow {get; set;}

        public TextEditor(short x, short y, short width, short height) {
            Text = new char[3] {'s', 'c', 'w'};
            EditorWindow = new TextEditorWindow(x, y, width, height);
            StartEditing();
        }
        internal void StartEditing() {
            Console.SetCursorPosition(EditorWindow.X + 1, EditorWindow.Y + 1);
            EditorWindow.Lines = UpdateContent("Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem eum fugiat, quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.");

            bool leaveLoop = false;
            do {
                EditorWindow.Content = EditorWindow.AffirmFixedContent();
                DrawController.Draw(EditorWindow);

                leaveLoop = this.RequestKey();
            } while (!leaveLoop);
            DrawController.CastOnDisplayBuffer(EditorWindow);
        }

        internal bool RequestKey() {
            
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            PressedKeyChar = pressedKey.KeyChar;

            if (PressedKeyChar == '\0') {
                if (pressedKey.Key == ConsoleKey.Backspace) {
                    // Yet to figure out the right implementation of backspace, to delete content
                    return false;
                }
                if (pressedKey.Key == ConsoleKey.LeftArrow || pressedKey.Key == ConsoleKey.RightArrow || pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.DownArrow) {
                    HandleNavigation(pressedKey);
                    return false;
                }
            } else if (char.IsLetter(PressedKeyChar)) {
                HandlePrintingChars();
                return false;
            } else if (char.IsDigit(PressedKeyChar)) {
                HandlePrintingChars();
                return false;
            } else if (char.IsWhiteSpace(PressedKeyChar)) {
                HandlePrintingChars();
                return false;
            } else if (char.IsSymbol(PressedKeyChar) || char.IsPunctuation(PressedKeyChar)) {
                HandlePrintingChars();
                return false;
            }
            return true;
        }

        void HandlePrintingChars() {
            // Most likely You should delegate the drawing entirely to the UIElement and DrawController class
            (int, int) TrackCursor = Console.GetCursorPosition();

            if (TrackCursor.Item1 >= EditorWindow.X + EditorWindow.Width - 1) {
                Console.SetCursorPosition(EditorWindow.X + 1, TrackCursor.Item2 + 1);
            }
            Text[0] += PressedKeyChar;
            Console.Write(PressedKeyChar); // So this is teporary

            // YOUR NEXT GOAL is to assign the characters to the char[],
            // while making sure that you move (if needed) all the
            // following characters forward. And if needed you have to
            // push the previous word to the next line, and if it
            // doesn't fit, then move it even further; and so on!
        }
        
        private void HandleNavigation(ConsoleKeyInfo input) { // kinda works
            (int, int) TrackCursor = Console.GetCursorPosition();

            if (input.Key == ConsoleKey.LeftArrow) {
                if (TrackCursor.Item1 < EditorWindow.X + 2) {
                    if (TrackCursor.Item2 > 0) {
                        if (EditorWindow.Lines != null) {
                            Console.SetCursorPosition(EditorWindow.Lines[TrackCursor.Item2 - EditorWindow.Y].Length - 1, TrackCursor.Item2 - 1);
                        }
                    }
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2);
                }            
            } else if (input.Key == ConsoleKey.RightArrow) {
                if (EditorWindow.Lines != null && TrackCursor.Item2 >= EditorWindow.Lines[TrackCursor.Item2 - EditorWindow.Y].Length) {
                    if (TrackCursor.Item2 <= EditorWindow.Lines.Length) {
                        Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2 + 1); // Overflows the editor
                    }
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 + 1, TrackCursor.Item2);
                }
            } else if (input.Key == ConsoleKey.UpArrow) {
                Console.SetCursorPosition(TrackCursor.Item1, TrackCursor.Item2 - 1);
            } else if (input.Key == ConsoleKey.DownArrow) {
                Console.SetCursorPosition(TrackCursor.Item1, TrackCursor.Item2 + 1);
            }

            // if ((input.Modifiers & ConsoleModifiers.Control) != 0) {}    // Implement modifiers later, for now just check what sticks
        }

        static public char[] StringToCharArray(string input) {
            List<char> result = new List<char> ();
            for (int i = 0; i < input.Length; i++) {
                result.Add(input[i]);
            }
            return result.ToArray();
        }
        
        public string[] UpdateContent(string input) {
            return UpdateContent(StringToCharArray(input));
        }

        public string[] UpdateContent(char[] input) {
            List<string> result = new List<string> ();
            int lineLength = EditorWindow.Width - 2;    
            if (input.Length > lineLength) {
                string line = "";
                int characterIndex = 0;

                while (true) {
                    bool exitLoop = false;
                    string word = "";

                    do {
                        word += input[characterIndex];

                        if (characterIndex >= input.Length - 1) {
                            exitLoop = true;
                            break;
                        }
                        characterIndex++;
                    } while (input[characterIndex] != ' ');

                    if (line.Length + word.Length > lineLength) {
                        result.Add(line);
                        if (word[0] == ' ') line = word.Substring(1);
                        else line = word;
                    } else {
                        line += word;
                    }

                    if (exitLoop) {
                        result.Add(line);
                        break;
                    }
                }
            }
            return result.ToArray();
        }
    }

    public class Pointer {
        short CoordinateX {get; set;}
        short CoordinateY {get; set;}
        short CurrentLine {get; set;}

        public Pointer() {
            (int, int) trackCursor = Console.GetCursorPosition();

            CoordinateX = (short)trackCursor.Item1;
            CoordinateY = (short)trackCursor.Item2;
        }

        public Pointer(short x, short y) {
            CoordinateX = x;
            CoordinateY = y;
        }

        public void MovePointer(short targetX, short targetY) {
            if (targetX < Console.BufferWidth && targetY < Console.BufferHeight) {
                Console.SetCursorPosition(targetX, targetY);
            }
        }

        void ShiftPointer(short xShiftBy, short yShiftBy = 0) {

        }
    }
}