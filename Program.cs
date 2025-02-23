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
            
            TextEditorWindow editorWindow = new TextEditorWindow(3, 6, 56, 28);
            DrawController.ReinitialiseDisplayBuffer('.');
            DrawController.CastOnDisplayBuffer(commands);
            Console.WriteLine("It didn't fail yet");
            DrawController.CastOnDisplayBuffer(editorWindow);
            Console.WriteLine("It didn't fail yet");
            DrawController.Draw();

            editorWindow.AccessEditor();
        }
    }
}