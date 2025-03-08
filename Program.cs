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
            Console.WriteLine("Testing-gu");

            TextEditor editingLogic = new TextEditor(3, 6, 56, 28, "Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Nemo enim ipsam voluptatem, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem eum fugiat, quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus, qui blanditiis praesentium voluptatum deleniti atque corrupti.");
            
            Thread.Sleep(3000);

            editingLogic.StartEditing();
        }
    }
}