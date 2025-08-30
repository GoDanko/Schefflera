using System;
using TextMod;
using VOutput;
using WithUserCommunication;

namespace Schefflera
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Display.SetUp();

            TextEditor editorRuntimeObject = TextEditor.CallEditor(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Schefflera", "cache.txt"));
            
            while (!editorRuntimeObject.ShutDown) {
                List<string> ManifestCollection = new List<string> ();

                Manifest.Call(ManifestCollection);

                if (editorRuntimeObject.editedFile != null) editorRuntimeObject.editedFile.Write("Writing");

                editorRuntimeObject.DismissEditor();
            }
        }
    }
}