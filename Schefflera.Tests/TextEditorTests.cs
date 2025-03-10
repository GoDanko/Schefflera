using Xunit;
using TextMod;

namespace TextEditorTesting {
    public class ArrayToolingTest {

        [Fact]
        public void StringToCharArray_ShouldReturnACharArray() {
            string input = "Hello World!";
            char[] expected = new char[] {'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd', '!'};

            char[] result = ArrayTooling.StringToCharArray(input);

            Assert.Equal(result, expected);
        }
    }
}