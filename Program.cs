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

            UIElement textWindow = new UIElement(2, 6);
            textWindow.Content = new string[] {"--------------------------------------------",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "|                                          |",
                "--------------------------------------------"};
            
            TextEditorWindow editorWindow = new TextEditorWindow(48, 6, 16, 8);
            editorWindow.AffirmFixedContent();  // This leads to the Casting on the display not working. Figure out why and fix it lol

            DrawController.ClearDisplayBuffer('x');
            DrawController.CastOnDisplayBuffer(commands);
            DrawController.CastOnDisplayBuffer(textWindow);
            DrawController.CastOnDisplayBuffer(editorWindow);   // This ain't working, because of the Fixed content being incorrectly drawn
            DrawController.Draw();

        }
    }
}