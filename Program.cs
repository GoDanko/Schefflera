using System;

using Layout;

namespace Schefflera
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UIElement commands = new UIElement(new string[] {"--------------------------------",
            "|  Enter-continue Escape-Quit  |",
            "--------------------------------"}, 2, 1);

            UIElement textWindow = new UIElement(new string[] {"--------------------------------",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "|                              |",
                "--------------------------------"}, 12, 6);

            commands.CastOnDisplay();
            textWindow.CastOnDisplay();

            DrawController.Draw();

            string? input = Console.ReadLine();
            if (input == null || input.ToLower() == "cls") {
                Console.Clear();
            }
        }
    }
}