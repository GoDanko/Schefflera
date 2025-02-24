using System;
using System.Runtime.CompilerServices;
using LayoutMod;

namespace TextMod
{

    // I made some very important decisions about how this app will be developed
    // And I wanted to share those decisions: I intend to focus more on testing
    // And I'd like (at least to try), to use char[] to store the editor content
    // I can always decide to give up on those if I find them too challenging.

    internal class TextEditor
    {
        public char[] Text {get; set;}    // You can find a way to cut the text into pieces based on the Width of the editor
        private short Lines {get; set;}
        public byte CurrentTextIndex;
        private short FirstLineIndex {get; set;}
        private char PressedKeyChar;
        private TextEditorWindow EditorWindow {get; set;}

        public TextEditor(TextEditorWindow editorWindow) {
            EditorWindow = editorWindow;
            Text = new char[3] {'s', 'c', 'w'};
            Console.SetCursorPosition(EditorWindow.X + 1, EditorWindow.Y + 1);
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
        }
        
        private void HandleNavigation(ConsoleKeyInfo input) { // kinda works
            (int, int) TrackCursor = Console.GetCursorPosition();

            if (input.Key == ConsoleKey.LeftArrow) {
                if (TrackCursor.Item1 < EditorWindow.X + 2) {
                    if (TrackCursor.Item2 > 0) {
                        Console.SetCursorPosition(EditorWindow.lines[TrackCursor.Item2 - EditorWindow.Y].Length - 1, TrackCursor.Item2 - 1);
                    }
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2);
                }            
            } else if (input.Key == ConsoleKey.RightArrow) {
                if (TrackCursor.Item2 >= EditorWindow.lines[TrackCursor.Item2 - EditorWindow.Y].Length) {
                    if (TrackCursor.Item2 <= EditorWindow.lines.Length) {
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
    }
}