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
            
            DrawController.ReinitialiseDisplayBuffer('.');
            DrawController.CastOnDisplayBuffer(commands);
            DrawController.Draw();
            Console.WriteLine("MammaMIA! Avanti! FIGURATTI!");

            TextEditor editingLogic = new TextEditor(3, 6, 56, 28);
        }
    }
}