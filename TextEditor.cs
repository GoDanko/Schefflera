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
        public List<string> Text {get; set;}    // You can find a way to cut the text into pieces based on the Width of the editor
        private short Lines {get; set;}
        public byte CurrentTextIndex;
        private short FirstLineIndex {get; set;}
        private char LastChar;
        private TextEditorWindow EditorWindow {get; set;}

        public TextEditor(TextEditorWindow editorWindow) {
            EditorWindow = editorWindow;
            Text = new List<string> () {""};
            Console.SetCursorPosition(EditorWindow.X + 1, EditorWindow.Y + 1);
        }

        void PopulateWindowContent() {
            
        }

        internal bool RequestKey() {
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            LastChar = pressedKey.KeyChar;

            if (LastChar == '\0') {
                if (pressedKey.Key == ConsoleKey.Backspace) {
                    // Yet to figure out the right implementation of backspace, to delete content
                }
                if (pressedKey.Key == ConsoleKey.LeftArrow || pressedKey.Key == ConsoleKey.RightArrow || pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.DownArrow) {
                    HandleNavigation(pressedKey);
                    return false;
                }
            } else if (char.IsLetter(LastChar)) {
                HandlePrintingChars();
                return false;
            } else if (char.IsDigit(LastChar)) {
                HandlePrintingChars();
                return false;
            } else if (char.IsWhiteSpace(LastChar)) {
                HandlePrintingChars();
                return false;
            } else if (char.IsSymbol(LastChar) || char.IsPunctuation(LastChar)) {
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
            Text[0] += LastChar;
            Console.Write(LastChar); // So this is teporary
        }
        
        private void HandleNavigation(ConsoleKeyInfo input) {   
            // Needs more integration with String Text, to understand if there's content along which it can move

            (int, int) TrackCursor = Console.GetCursorPosition();

            if (input.Key == ConsoleKey.LeftArrow) {
                if (TrackCursor.Item1 < EditorWindow.X + 2) {
                    if (TrackCursor.Item2 > 0) {
                        Console.SetCursorPosition(Text[TrackCursor.Item2 - 1].Length, TrackCursor.Item2 - 1);
                    }
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2);
                }

            } else if (input.Key == ConsoleKey.RightArrow) {
                if (TrackCursor.Item1 > EditorWindow.X + EditorWindow.Width - 2) {

                }
            }

            // if ((input.Modifiers & ConsoleModifiers.Control) != 0) {}    // Implement modifiers later, for now just check what sticks
        }
    }
}