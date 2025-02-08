using System;

using Layout;

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

            UIElement textWindow = new UIElement(12, 6);
            textWindow.Content = new string[] {"--------------------------------",
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
                "--------------------------------"};

            DrawController.CastOnDisplay(commands);
            DrawController.CastOnDisplay(textWindow);

            DrawController.Draw();

            string? input = Console.ReadLine();
            if (input == null || input.ToLower() == "cls") {
                Console.Clear();
            }
        }
    }
}