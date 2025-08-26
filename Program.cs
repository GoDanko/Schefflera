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
            
            // COutPreset.ConsoleOutBytesInArray(TestsVOuput.TEST_ValidateDivParameters());
            // COutPreset.ConsoleOutStringsInArray(StringTooling.SplitStringForLength("Praesentium temporibus sed sunt rem facere voluptates. Dolorum voluptatem at hic error iste porro amet ratione. Autem perferendis nostrum sit eum neque vel in dolores. Qui accusamus fugit minus ad voluptatem sint illum eos. Nihilcupiditate sed sunt excepturi in. Necessitatibus alias rem fugiat doloremque qui repellendus ut enim", 331));
            

            TextEditor editorRuntimeObject = TextEditor.CallEditor();
            
            while (!editorRuntimeObject.ShutDown) {
                List<string> ManifestCollection = new List<string> ();
                ManifestCollection.Add("The value of the expression is compared with the values of each case");
                ManifestCollection.Add("The break and default keywords will be described later in this chapter");
                ManifestCollection.Add("The switch expression is evaluated once");

                Manifest.Call(ManifestCollection);
            }
        }
    }
}