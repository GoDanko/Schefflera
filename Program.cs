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
            
            TextEditorWindow editorWindow = new TextEditorWindow(48, 6, 4, 6);

            DrawController.ReinitialiseDisplayBuffer('x');
            DrawController.CastOnDisplayBuffer(commands);
            DrawController.CastOnDisplayBuffer(editorWindow);   // This ain't working, because of the Fixed content being incorrectly drawn
            DrawController.Draw();

        }
    }
}