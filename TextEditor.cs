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
            LoadedText = new string[editorWindow.Height - 2];
        }

        internal bool RequestKey() {
           ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            if (pressedKey.Key == ConsoleKey.LeftArrow || pressedKey.Key == ConsoleKey.RightArrow || pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.DownArrow) {
                HandleNavigation(pressedKey);
                return false;
            } else if (pressedKey.Key == ConsoleKey.Escape) {
                return true;
            }
            new DiagnosticMsg("Invalid input");
            return false;
        }

        private void HandleNavigation(ConsoleKeyInfo input) {       // This method makes logical sense (at first glance)
                                                            // But it wasn't tested, because the app cannot display
                                                            // The content of the text editor yet.

            (int, int) TrackCursor = Console.GetCursorPosition();

            if (input.Key == ConsoleKey.LeftArrow) {
                if (TrackCursor.Item1 < MostLeftIndex + EditorWindow.X + 1 && TrackCursor.Item2 > 0) {
                    Console.SetCursorPosition(LoadedText[TrackCursor.Item2 - 1].Length, TrackCursor.Item2 - 1);
                    // If we would go out of bounds moving backwards
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2);
                }

            } else if (input.Key == ConsoleKey.RightArrow) {
                if (TrackCursor.Item1 > LoadedText[TrackCursor.Item2].Length + EditorWindow.X - 1 && TrackCursor.Item2 < LoadedText.Length) {
                    Console.SetCursorPosition(MostLeftIndex + EditorWindow.X, TrackCursor.Item2 + 1);
                    // If we would go out of bounds moving forward
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 + 1, TrackCursor.Item2);
                }
            }

            // if ((input.Modifiers & ConsoleModifiers.Control) != 0) {}    // Implement modifiers later, for now just check what sticks
        }
    }
}