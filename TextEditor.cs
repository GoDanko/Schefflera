using System;
using System.Net.Sockets;
using System.Reflection;
using VOutput;
using FileManager;

namespace TextMod
{
    class TextEditor {
        private bool shutDown = false;
        public bool ShutDown { get {return shutDown;} private set {shutDown = value;} }
        internal Div EditorBody;
        internal FileHandler? editedFile;
        public TextEditor() {
            EditorBody = Display.CreateDiv((0, 2), (Display.XConsoleBuffer, (ushort)(Display.YConsoleBuffer - 2)), Display.ConsoleSpace);
        }

        internal static TextEditor CallEditor(string? filePath = null) {
            TextEditor editor = new TextEditor();

            if (filePath != null) editor.editedFile = FileHandler.NewFile(filePath, true);

            return editor;
        }

        internal void DismissEditor() {
            ShutDown = true;
        }
    }

    static class TextHandler {

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