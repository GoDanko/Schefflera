using System;

using LayoutMod;
using TextMod;

namespace Schefflera
{
    internal class Program
    {
        static void Main(string[] args)
        {

            UIElement commands = new UIElement(2, 1);
            commands.Content = new string[] {"--------------------------------",
            "|  Enter-continue Escape-Quit  |",
            "--------------------------------"};
            
            TextEditorWindow editorWindow = new TextEditorWindow(3, 6, 64, 16);
            Console.WriteLine("It didn't fail yet");

            DrawController.ReinitialiseDisplayBuffer('.');
            DrawController.CastOnDisplayBuffer(commands);
            DrawController.CastOnDisplayBuffer(editorWindow);
            DrawController.Draw();

            editorWindow.AccessEditor();
        }
    }
}