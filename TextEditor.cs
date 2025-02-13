using System;

using LayoutMod;

namespace TextMod
{
    internal class TextEditor
    {
        public string[] LoadedText {get; set;}
        private short FirstLineIndex {get; set;}
        private short MostLeftIndex {
            get {
                    if (DrawIndexes) return 4;
                    else return 1;
            }
        }
        private bool DrawIndexes {
            get {
                if (EditorWindow.Width > 16) return true;
                else return false;
            }
        }
        private TextEditorWindow EditorWindow {get; set;}

        public TextEditor(TextEditorWindow editorWindow) {
            Console.SetCursorPosition(editorWindow.X + 1, editorWindow.Y + 1);
            EditorWindow = editorWindow;
        }

        void RequestKey() {
           ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            if (pressedKey.Key == (ConsoleKey.LeftArrow | ConsoleKey.RightArrow | ConsoleKey.UpArrow | ConsoleKey.DownArrow)) {
                HandleNavigation(pressedKey);
            }
        }

        void HandleNavigation(ConsoleKeyInfo input) {       // This method makes logical sense (at first glance)
                                                            // But it wasn't tested, because the app cannot display
                                                            // The content of the text editor yet.

            (int, int) ConsoleCursor = Console.GetCursorPosition();

            if (input.Key == ConsoleKey.LeftArrow) {
                if (ConsoleCursor.Item1 < MostLeftIndex + EditorWindow.X + 1 && ConsoleCursor.Item2 > 0) {
                    Console.SetCursorPosition(LoadedText[ConsoleCursor.Item2 - 1].Length, ConsoleCursor.Item2 - 1);
                    // If we would go out of bounds moving backwards
                } else {
                    Console.SetCursorPosition(ConsoleCursor.Item1 - 1, ConsoleCursor.Item2);
                }

            } else if (input.Key == ConsoleKey.RightArrow) {
                if (ConsoleCursor.Item1 > LoadedText[ConsoleCursor.Item2].Length + EditorWindow.X - 1 && ConsoleCursor.Item2 < LoadedText.Length) {
                    Console.SetCursorPosition(MostLeftIndex + EditorWindow.X, ConsoleCursor.Item2 + 1);
                    // If we would go out of bounds moving forward
                } else {
                    Console.SetCursorPosition(ConsoleCursor.Item1 + 1, ConsoleCursor.Item2);
                }
            }

            // if ((input.Modifiers & ConsoleModifiers.Control) != 0) {}    // Implement modifiers later, for now just check what sticks
        }
    }
}