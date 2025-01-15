using SqlValidator;
namespace Tests;
public class Helper_GetNextWord_Tests
{
    [Theory]
    [InlineData(true, "'Hello'", "'Hello'", "")]
    [InlineData(true, "   'World'", "'World'", "")]
    [InlineData(true, " 'Oh, my!'  ", "'Oh, my!'", "  ")]
    [InlineData(false, "'Oh, oh!", "", "'Oh, oh!")]
    [InlineData(true, "'SpongeBob''s Pineapple'", "'SpongeBob''s Pineapple'", "")]
    [InlineData(true, "'a''b''c' 'd''e'", "'a''b''c'", " 'd''e'")]
    [InlineData(true, "'v\\'w' 'x\\'y\\'z", "'v\\'w'", " 'x\\'y\\'z")]
    [InlineData(true, "'\"GG EZ\" they typed in the chat.' '\"I think pineapple belongs on pizza!\" is what actually shows to others.'", "'\"GG EZ\" they typed in the chat.'", " '\"I think pineapple belongs on pizza!\" is what actually shows to others.'")]
    [InlineData(false, "'escape!''", "", "'escape!''")]
    [InlineData(true, "''", "''", "")]
    public void Test_SingleQuote(bool expected, string input, string expectedWord, string expectedRest)
    {
        bool actual = Helper.GetNextWord(input, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWord, word);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "\"Usagi\"", "\"Usagi\"", "")]
    [InlineData(true, "  \"Tsukino\"", "\"Tsukino\"", "")]
    [InlineData(true, " \"To. Late!\"  ", "\"To. Late!\"", "  ")]
    [InlineData(false, "\"Oh, nooo!", "", "\"Oh, nooo!")]
    [InlineData(true, "\"Hmm\"\"Hmmmmm\"", "\"Hmm\"\"Hmmmmm\"", "")]
    [InlineData(true, "\"a\"\"b\"\"c\" \"d\"\"e\"", "\"a\"\"b\"\"c\"", " \"d\"\"e\"")]
    [InlineData(true, "\"v\\\"w\" \"x\\\"y\\\"z\"", "\"v\\\"w\"", " \"x\\\"y\\\"z\"")]
    [InlineData(true, "\"'GG EZ' they typed in the chat.\" \"'I think pineapple belongs on pizza!' is what actually shows to others.\"", "\"'GG EZ' they typed in the chat.\"", " \"'I think pineapple belongs on pizza!' is what actually shows to others.\"")]
    [InlineData(false, "\"escape!\"\"", "", "\"escape!\"\"")]
    [InlineData(true, "\"\"", "\"\"", "")]
    public void Test_DoubleQuote(bool expected, string input, string expectedWord, string expectedRest)
    {
        bool actual = Helper.GetNextWord(input, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWord, word);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "Golf", "Golf", "")]
    [InlineData(true, "Cream Cheese", "Cream", " Cheese")]
    [InlineData(true, "  Help", "Help", "")]
    [InlineData(true, " Error  ", "Error", "  ")]
    [InlineData(true, "Gross!", "Gross", "!")]
    [InlineData(true, "True100", "True100", "")]
    [InlineData(true, "1a", "1a", "")]
    [InlineData(true, "a_b_1_4 817sjq", "a_b_1_4", " 817sjq")]
    [InlineData(true, "_1_", "_1_", "")]
    public void Test_Word(bool expected, string input, string expectedWord, string expectedRest)
    {
        bool actual = Helper.GetNextWord(input, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWord, word);
        Assert.Equal(expectedRest, rest);
    }

    [Theory]
    [InlineData(true, "!", "!", "")]
    public void Test_Special(bool expected, string input, string expectedWord, string expectedRest)
    {
        bool actual = Helper.GetNextWord(input, out ReadOnlySpan<char> word, out ReadOnlySpan<char> rest);
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWord, word);
        Assert.Equal(expectedRest, rest);
    }
}
