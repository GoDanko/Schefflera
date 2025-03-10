using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using LayoutMod;

namespace TextMod
{
    internal class TextEditor
    {
        public char[] Text {get; set;}
        private short Lines {get; set;}
        public byte CurrentTextIndex;
        private short FirstLineIndex {get; set;}
        private char PressedKeyChar;
        private TextEditorWindow TextElement {get; set;}
        private TextPointer CurrentCursor {get; set;}

        public TextEditor(short x, short y, short width, short height, string content = "") {
            TextElement = new TextEditorWindow(x, y, width, height);

            if (content == null || content == default) {
                TextElement.Lines = new string[1] {""};
                Text = new char[0];
            } else {
                TextElement.Lines = ParseToEditor(content);

                Text = ArrayTooling.StringToCharArray(content);
            }
            TextElement.Content = TextElement.AffirmFixedContent();
            DrawController.Draw(TextElement);

            CurrentCursor = new TextPointer(TextElement, Text);
        }

        internal void StartEditing() {
            CurrentCursor = new TextPointer(TextElement, Text);
            
            bool leaveLoop = false;
            do {
                TextElement.Content = TextElement.AffirmFixedContent();
                DrawController.Draw(TextElement);

                leaveLoop = this.RequestKey();
                TextElement.Lines = ParseToEditor("Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem eum fugiat, quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.");

            } while (!leaveLoop);
            DrawController.CastOnDisplayBuffer(TextElement);
        }

        internal bool RequestKey() {
            
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            PressedKeyChar = pressedKey.KeyChar;

            if (PressedKeyChar == '\0') {
                if (pressedKey.Key == ConsoleKey.Backspace) {
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
            (int, int) TrackCursor = Console.GetCursorPosition();

            if (TrackCursor.Item1 >= TextElement.X + TextElement.Width - 1) {
                Console.SetCursorPosition(TextElement.X + 1, TrackCursor.Item2 + 1);
            }
            Text[0] += PressedKeyChar;
            Console.Write(PressedKeyChar);

            // You need now To create a "pipeline" that will be used to affect char[] Text
            // and TextWindow.Lines/Content, directly with the user, while also handling
            // CurrentCursor's variables, to keep track of the changes.

            // At the end of the day, there's a lot of "internal dependency" (I'm not sure
            // if this term is accurate), so please, keep track of it, so you can start
            // using a more integral architecture (abstracted as a whole), built by you.
        }
        
        private void HandleNavigation(ConsoleKeyInfo input) {
            (int, int) TrackCursor = Console.GetCursorPosition();

            if (input.Key == ConsoleKey.LeftArrow) {
                if (TrackCursor.Item1 < TextElement.X + 2) {
                    if (TrackCursor.Item2 > 0) {
                        if (TextElement.Lines != null) {
                            Console.SetCursorPosition(TextElement.Lines[TrackCursor.Item2 - TextElement.Y].Length - 1, TrackCursor.Item2 - 1);
                        }
                    }
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2);
                }            
            } else if (input.Key == ConsoleKey.RightArrow) {
                if (TextElement.Lines != null && TrackCursor.Item2 >= TextElement.Lines[TrackCursor.Item2 - TextElement.Y].Length) {
                    if (TrackCursor.Item2 <= TextElement.Lines.Length) {
                        Console.SetCursorPosition(TrackCursor.Item1 - 1, TrackCursor.Item2 + 1);
                    }
                } else {
                    Console.SetCursorPosition(TrackCursor.Item1 + 1, TrackCursor.Item2);
                }
            } else if (input.Key == ConsoleKey.UpArrow) {
                Console.SetCursorPosition(TrackCursor.Item1, TrackCursor.Item2 - 1);
            } else if (input.Key == ConsoleKey.DownArrow) {
                Console.SetCursorPosition(TrackCursor.Item1, TrackCursor.Item2 + 1);
            }
        }

        void PasteContentToCharArray(string pastedContent) {
            char[] result = new char[Text.Length + pastedContent.Length];
            ArrayTooling.OverwriteCharArray(ref result, Text, default, CurrentCursor.ObjectiveCharArrayPosition);

            if (CurrentCursor.ObjectiveCharArrayPosition < Text.Length) {
                ArrayTooling.OverwriteCharArray(ref result, ArrayTooling.StringToCharArray(pastedContent), CurrentCursor.ObjectiveCharArrayPosition, pastedContent.Length);
                ArrayTooling.OverwriteCharArray(ref result, Text, CurrentCursor.ObjectiveCharArrayPosition + pastedContent.Length);

            } else {
                ArrayTooling.OverwriteCharArray(ref result, ArrayTooling.StringToCharArray(pastedContent), pastedContent.Length);
            }

            CurrentCursor.ObjectiveCharArrayPosition += pastedContent.Length;
        }
        
        void PasteContentToCharArray(char pastedChar) {
            char[] result = new char[Text.Length + 1];
            ArrayTooling.OverwriteCharArray(ref result, Text, default, CurrentCursor.ObjectiveCharArrayPosition);

            if (CurrentCursor.ObjectiveCharArrayPosition < Text.Length) {
                ArrayTooling.ShiftChars(ref result, pastedChar, CurrentCursor.ObjectiveCharArrayPosition);
                ArrayTooling.OverwriteCharArray(ref result, Text, CurrentCursor.ObjectiveCharArrayPosition + 1);
            } else {
                ArrayTooling.ShiftChars(ref result, pastedChar, CurrentCursor.ObjectiveCharArrayPosition);
            }

            CurrentCursor.ObjectiveCharArrayPosition++;
        }

        void UpdateCursorWithinElement(short moveBy = 1) {
            if (TextElement.Lines != null) {
                short accumulatedLength = 0;
                byte lineJump = 0;

                while (accumulatedLength < moveBy) {
                    accumulatedLength += (short)TextElement.Lines[CurrentCursor.ObjectiveCurrentLine - 1 + lineJump].Length;
                    lineJump++;
                }
                accumulatedLength -= (short)(accumulatedLength - moveBy);

            }
        }



        void PushInput() {

        }
        
        public string[] ParseToEditor(string input) {
            return ParseToEditor(ArrayTooling.StringToCharArray(input));
        }

        public string[] ParseToEditor(char[] input) {
            List<string> result = new List<string> ();
            int lineLength = TextElement.Width - 2;    
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

    public class TextPointer {
            // Please make sure that until You're within the StartEditing loop
            // that You're changing the values here as well, since this will
            // allow You to keep track of the various important variables.
        internal short CoordinateX {get; set;}
        internal short CoordinateY {get; set;}
        internal short CurrentColumn {get; set;}
        internal short CurrentLine {get; set;}
        internal short ObjectiveCurrentLine {get; set;}
        internal int ObjectiveCharArrayPosition {get; set;}

        public TextPointer(TextEditorWindow textElement, char[] charArray) {
            if (textElement.Lines != null) {
                ObjectiveCurrentLine = (short)(textElement.Lines.Length < 32766 ? textElement.Lines.Length : 0);
                CoordinateX = (short)(textElement.Lines[ObjectiveCurrentLine - 1].Length);
                CoordinateY = (short)(ObjectiveCurrentLine - 1);
                ObjectiveCharArrayPosition = charArray.Length;

            } else {
                ObjectiveCurrentLine = (short) 0;
                CoordinateX = (short)(textElement.X + 1);
                CoordinateY = (short)(textElement.Y + 1);
                ObjectiveCharArrayPosition = 0;
            }
        }

        public bool MovePointer(short targetX, short targetY) {
            if (targetX < Console.BufferWidth && targetY < Console.BufferHeight) {
                Console.SetCursorPosition(targetX, targetY);
                CoordinateX = targetX;
                CoordinateY = targetY;
                return true;
            }
            return false;
        }

        private void FindCurrentLine() {

        }

        void ShiftPointer(short xShiftBy, short yShiftBy = 0) {

        }
    }

    static class ArrayTooling {
        static public char[] StringToCharArray(string input) {
            List<char> result = new List<char> ();
            for (int i = 0; i < input.Length; i++) {
                result.Add(input[i]);
            }
            return result.ToArray();
        }

        internal static void ShiftChars(ref char[] assignToArray, char pasteThis, int startHere) {
            
        }

        internal static void OverwriteCharArray(ref char[] assignToArray, char[] pasteThis, int startHere = 0, int endHere = 0) {
            if (endHere <= startHere) endHere = pasteThis.Length;

            for (int i = 0; i < endHere; i++) {
                assignToArray[i + startHere] = pasteThis[i];
            }
        }

    }
}