using System;
using System.Reflection;
using VOutput;

namespace TextMod
{
    class TextEditor {
        private bool shutDown = false;
        public bool ShutDown { get {return shutDown;} private set {shutDown = value;} }
        internal Div EditorBody;
        public TextEditor() {
            EditorBody = Display.CreateDiv((0, 2), (Display.XConsoleBuffer, (ushort)(Display.YConsoleBuffer - 2)), Display.ConsoleSpace);
        }

        static internal TextEditor CallEditor() {
            TextEditor editor = new TextEditor();

            return editor;
        }
    }

    static class StringTooling 
    {
        public static string[] SplitStringForLength(string input, int splitAtLength) {
            List<string> result = new List<string> ();
            if (input.Length <= splitAtLength) return new string[1] {input};

            string txtLine = "";
            string word = "";
            for (int i = 0; i < input.Length; i++) {
                
                if (input[i] != ' ' && word.Length == splitAtLength - 1) {
                    word += '-';
                    txtLine += word;
                    word = "";
                }

                word += input[i];

                if (input[i] == ' ') { 
                    txtLine += word;
                    word = "";
                }
 
                if (txtLine.Length + word.Length >= splitAtLength) {
                    result.Add(txtLine);
                    txtLine = "";
                }

                if (i == input.Length -1) {
                    result.Add(txtLine += word);
                }
            }
            
            return result.ToArray();
        }
    }
}