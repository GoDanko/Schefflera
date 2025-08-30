using System;
using TextMod;
using VOutput;

namespace WithUserCommunication {

    class Manifest {
        const ushort MaxX = 64;
        const ushort MinX = 32;
        const ushort LimitLines = 4;
        const ushort padding = 6;
        ushort instanceX;
        ushort instanceY;
        List<string> manifestContent;
        ushort printIndex;
        
        Manifest(List<string> ContentsToOutput) {
            manifestContent = ContentsToOutput;
        }

        public static Manifest Call(List<string> ContentsToOutput) {

            Manifest manifestMsg = new Manifest(ContentsToOutput);

            if (manifestMsg.manifestContent.Count > 0) { 

                // ushort LongestText = 0;
                // for (ushort i = 0; i < manifestMsg.manifestContent.Count; i++) {
                //     if (manifestMsg.manifestContent[i].Length > LongestText) { LongestText = (ushort)manifestMsg.manifestContent[i].Length; }
                // }

                manifestMsg.instanceX = (ushort)(MaxX + padding < Display.XConsoleBuffer ? MaxX : Display.XConsoleBuffer - padding);
                
                ushort mostLinesQuota = 0;
                for (ushort i = 0; i < manifestMsg.manifestContent.Count; i++) {
                    ushort mostLinesForElement = (ushort)StringTooling.SplitStringForLength(manifestMsg.manifestContent[i], manifestMsg.instanceX - padding).Length;
                    if (mostLinesForElement > mostLinesQuota) mostLinesQuota = mostLinesForElement;
                }
                
                // manifestMsg.instanceY = (ushort)(LongestText > MaxX - padding ? LongestText / (MaxX - padding) + padding : 1 + padding);
                manifestMsg.instanceY = (ushort)(mostLinesQuota > LimitLines ? LimitLines + padding : mostLinesQuota + padding);
                // if (manifestMsg.instanceY > LimitLines + padding) { manifestMsg.instanceY = LimitLines + padding ; }

                (ushort x, ushort y) expectedPosition = ((ushort)(Display.XConsoleBuffer / 2 - manifestMsg.instanceX / 2), (ushort)(Display.YConsoleBuffer / 2 - manifestMsg.instanceY / 2));
                Div manifestBody = Display.CreateDiv(expectedPosition, (manifestMsg.instanceX, manifestMsg.instanceY));
                
                manifestMsg.ManifestRuntime(manifestBody);
            }

            return manifestMsg;
        }

        void ManifestRuntime(Div manifestBody) {
            printIndex = 0;
            ushort? scrollValue = null;
            bool leave = false;

            do {
                manifestBody.content = ConstructManifestBody(manifestBody, ref scrollValue);
                manifestBody.Draw();

                ManifestNavigation(ref leave, ref scrollValue);
            } while (!leave);

            manifestContent = new List<string> ();
            Console.Clear();
        }

        char[,] ConstructManifestBody(Div manifestBody, ref ushort? scrollValue) {

            string[] preparedText = StringTooling.SplitStringForLength(manifestContent[printIndex], manifestBody.Size.x - 7);
            (ushort x, ushort y) overwriteUntil = ((ushort)(manifestBody.Size.x - padding), (ushort)preparedText.Length);
            
            if (preparedText.Length > LimitLines) {
                if (!scrollValue.HasValue) scrollValue = 0;
                else if (scrollValue > ushort.MaxValue / 2) scrollValue = 0;
                else if (scrollValue > preparedText.Length - LimitLines) scrollValue = (ushort)(preparedText.Length - LimitLines);

            } else {
                if (preparedText.Length + padding - 1 > manifestBody.Size.y) {
                    manifestBody.Size.y += (ushort)(preparedText.Length + padding - 1 - manifestBody.Size.y);
                    manifestBody.Position.y += (ushort)(preparedText.Length + padding - 1 - manifestBody.Size.y);
                }
                scrollValue = null;
            }
            
            char[,] result = new char[manifestBody.Size.x, manifestBody.Size.y];
            result = manifestBody.content = Display.BoxContent(manifestBody.Size.x, manifestBody.Size.y);
            
            if (!scrollValue.HasValue) {
                for (ushort y = 0; y < overwriteUntil.y; y++) {
                    for (ushort x = 0; x < preparedText[y].Length; x++) {
                        result[x + 3, y + 2] = preparedText[y][x];
                    }
                }

            } else {
                for (ushort y = 0; y < LimitLines; y++) {
                    for (ushort x = 0; x < preparedText[(ushort)(y + scrollValue)].Length; x++) {
                        result[x + 3, y + 2] = preparedText[(ushort)(y + scrollValue)][x];
                    }
                }
            }

            string navigationArrows =  $"<{printIndex + 1}/{manifestContent.Count}>";
            for (ushort x = 0; x < navigationArrows.Length; x++) {
                result[manifestBody.Size.x - (5 + navigationArrows.Length) + x, manifestBody.Size.y - (padding / 2)] = navigationArrows[x];
            }

            if (scrollValue.HasValue) {
                if (scrollValue != 0) result[manifestBody.Size.x - 3, manifestBody.Size.y - (LimitLines + 4)] = 'A';
                if (scrollValue != preparedText.Length - LimitLines) result[manifestBody.Size.x - 3, manifestBody.Size.y - 5] = 'v';
            }

            return result;
        }

        ConsoleKeyInfo ManifestNavigation(ref bool leave, ref ushort? scrollValue) {
            ConsoleKeyInfo input = Console.ReadKey(true);
            if (input.Key == ConsoleKey.Escape || input.Key == ConsoleKey.Enter) { leave = true; }
            else if (input.Key == ConsoleKey.RightArrow) { printIndex = printIndex < manifestContent.Count - 1 ? ++printIndex : printIndex; }
            else if (input.Key == ConsoleKey.LeftArrow) { printIndex = printIndex != 0 ? --printIndex : printIndex; }
            else if (input.Key == ConsoleKey.DownArrow) { scrollValue = scrollValue.HasValue ? ++scrollValue : null; }
            else if (input.Key == ConsoleKey.UpArrow) { scrollValue = scrollValue.HasValue ? --scrollValue : null; }
            return input;
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