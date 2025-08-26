using System;
using VOutput;

namespace WithUserCommunication {

    class Manifest {
        const ushort MaxX = 64;
        const ushort MinX = 32;
        const ushort padding = 5;
        ushort instanceX;
        ushort instanceY;
        List<string> contentsToOutput;
        
        Manifest(List<string> manifestContent) {
            contentsToOutput = manifestContent;
        }

        static public Manifest Call(List<string> manifestContent) {

            Manifest manifestMsg = new Manifest(manifestContent);
            if (manifestContent.Count > 0) { 

                ushort LongestText = 0;
                for (ushort i = 0; i < manifestContent.Count; i++) {
                    if (manifestContent.Count > LongestText) { LongestText = (ushort)manifestContent.Count; }
                }

                manifestMsg.instanceY = (ushort)(LongestText > MaxX - padding ? LongestText / (MaxX - padding) + padding : 1 + padding);
                manifestMsg.instanceX = (ushort)(MaxX + padding < Display.XConsoleBuffer ? MaxX : Display.XConsoleBuffer - padding);
                
                if (manifestMsg.instanceX < MinX) { return manifestMsg; }

                (ushort x, ushort y) expectedPosition = ((ushort)(Display.XConsoleBuffer / 2 - manifestMsg.instanceX / 2), (ushort)(Display.YConsoleBuffer / 2 - manifestMsg.instanceY / 2));
                Div manifestBody = Display.CreateDiv((expectedPosition), (manifestMsg.instanceX, manifestMsg.instanceY));
                manifestBody.content = Display.CreateFrame(manifestMsg.instanceX, manifestMsg.instanceY);

                manifestBody.Draw();
                
                Console.ReadKey(true);

                // while (true) {}
            }

            return manifestMsg;
        }
    }
    
    internal static class COutPreset 
    {
        internal static void ConsoleOutBytesInArray(byte[] input, string seperator = ", ") {
            if (input.Length > 0) {
                for (int i = 0; i < input.Length; i++) {
                    Console.Write($"{input[i]}{seperator}");
                }
            }
        }
        
        internal static void ConsoleOutStringsInArray(string[] input, string seperator = "\n") {
            if (input.Length > 0) {
                for (int i = 0; i < input.Length; i++) {
                    Console.Write($"{input[i]}{seperator}");
                }
            }
        }
    }
}