using System;

using LayoutMod;
using TextMod;

namespace Schefflera
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UIElement commands = new UIElement(3, 1);
            commands.Content = new string[] {"--------------------------------",
            "|     C# Text Editor V.0023    |",
            "--------------------------------"};
            
            TextEditorWindow editorWindow = new TextEditorWindow(3, 6, 42, 28);
            Console.WriteLine("It didn't fail yet");

            DrawController.ReinitialiseDisplayBuffer('.');
            DrawController.CastOnDisplayBuffer(commands);
            DrawController.CastOnDisplayBuffer(editorWindow);
            DrawController.Draw();

            editorWindow.AccessEditor();
        }
    }
}