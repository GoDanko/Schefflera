using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace VOutput
{
    static class Display
    {
        public static ushort YConsoleBuffer { get {return (ushort)Console.BufferHeight;} }
        public static ushort XConsoleBuffer { get {return (ushort)Console.BufferWidth;} }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        internal static Div ConsoleSpace;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        internal static void SetUp() {
            ConsoleSpace = new Div() {
                Position = (0, 0),
                Size = (XConsoleBuffer, YConsoleBuffer)
            };
        }

        public static Div CreateDiv ((ushort x, ushort y) position, (ushort x, ushort y) size, Div? nestedWithin = null) {

            Div result = new Div() {
                NestedWithin = nestedWithin != null ? nestedWithin : ConsoleSpace,
            };

            (ushort toPosition, ushort toSize) xCorrection = Div.ValidateDivParameters(ref position.x, ref size.x, (ushort)(result.NestedWithin.Position.x + result.NestedWithin.Size.x));
            (ushort toPosition, ushort toSize) yCorrection = Div.ValidateDivParameters(ref position.y, ref size.y, (ushort)(result.NestedWithin.Position.y + result.NestedWithin.Size.y));

            result.Position = (position.x, position.y);
            result.Size = (size.x, size.y);
            return result;
        }

        public static char[,] CreateFrame(ushort sizeX, ushort sizeY) {
            char[,] result = new char[sizeX, sizeY];

            for (ushort y = 0; y < sizeY; y++) {
                result[0, y] = '|';
                result[sizeX - 1, y] = '-';
            }
            for (ushort x = 0; x < sizeX; x++) {
                result[x, 0] = '-';
                result[x, sizeY - 1] = '-';
            }

            return result;
        }
    }

    class Div {
        internal (ushort x, ushort y) Position;
        internal (ushort x, ushort y) Size;
        internal Div? NestedWithin;
        internal char[,] content;

        public Div() {
            content = new char[Size.x, Size.y];
        }
        public Div((ushort x, ushort y) position, (ushort x, ushort y) size) {
            content = new char[size.x, size.y];
            Position = position;
            Size = size;
        }

        internal static (ushort toPosition, ushort toSize) ValidateDivParameters(ref ushort positionDimension, ref ushort sizeDimension, ushort dimensionBoundary) {

            (ushort toPosition, ushort toSize) correction = (0, 0);

            if (sizeDimension < positionDimension) {
                ushort int16 = positionDimension;
                positionDimension = sizeDimension;
                sizeDimension = int16;
            }
            
            if (positionDimension + sizeDimension > dimensionBoundary) {
                ushort moveTargetBy = (ushort)(positionDimension + sizeDimension - dimensionBoundary);
                correction.toPosition = moveTargetBy;
                if (positionDimension > moveTargetBy) { positionDimension -= moveTargetBy; }
                else {
                    moveTargetBy -= positionDimension;
                    positionDimension = 0;
                    correction.toPosition -= moveTargetBy;
                    sizeDimension = sizeDimension > moveTargetBy ? sizeDimension -= moveTargetBy : dimensionBoundary;
                    correction.toSize = moveTargetBy;
                }
            }
            
            return correction;
        }

        internal void Draw() {
            Span<char> Line = stackalloc char[Size.x];
            for (ushort y = 0; y < Size.y; y++) {

                for (ushort x = 0; x < Size.x; x++) {
                    Line[x] = content[x, y + Position.y] != '\0' ? content[x, y + Position.y] : ' ';
                    // Index was outside the bounds of the array ^
                }
                Console.WriteLine($"{Line}");
                Console.SetCursorPosition(Position.x, y + Position.y);
            }
        }
    }

    static class TestsVOuput {
        internal static byte[] TEST_ValidateDivParameters() {

            List<byte> faliurePoints = new List<byte> ();

            ushort TEST_boundary = 16;
            ushort TEST_DimensionPosition = 2;
            ushort TEST_DimensionSize = 8;

            (ushort CorrectionToPosition, ushort CorrectionToSize) testID1 = Div.ValidateDivParameters(ref TEST_DimensionPosition, ref TEST_DimensionSize, TEST_boundary);
            if (testID1.CorrectionToPosition != 0 || testID1.CorrectionToSize != 0) { faliurePoints.Add(1); }

            TEST_DimensionPosition = 10;
            (ushort CorrectionToPosition, ushort CorrectionToSize) testID2 = Div.ValidateDivParameters(ref TEST_DimensionPosition, ref TEST_DimensionSize, TEST_boundary);
            if (testID2.CorrectionToPosition != 2 || testID2.CorrectionToSize > 0) { faliurePoints.Add(2); }

            TEST_DimensionPosition = 18;
            (ushort CorrectionToPosition, ushort CorrectionToSize) testID3 = Div.ValidateDivParameters(ref TEST_DimensionPosition, ref TEST_DimensionSize, TEST_boundary);
            if (testID3.CorrectionToPosition != 10 || testID3.CorrectionToSize != 2) { faliurePoints.Add(3); }

            return faliurePoints.ToArray();
        }
    }
}