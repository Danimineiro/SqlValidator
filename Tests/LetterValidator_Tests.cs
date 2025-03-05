using SqlValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;
public class LetterValidator_Tests
{
    [Theory]
    [InlineData([true, 'a'])]
    [InlineData([true, 'A'])]
    [InlineData([true, 'z'])]
    [InlineData([true, 'Z'])]
    [InlineData([true, 'e'])]
    [InlineData([true, 'J'])]
    [InlineData([true, 'k'])]
    [InlineData([true, 'L'])]
    [InlineData([false, '_'])]
    [InlineData([false, '.'])]
    [InlineData([false, '#'])]
    [InlineData([false, '*'])]
    [InlineData([false, '&'])]
    [InlineData([false, '?'])]
    [InlineData([false, '\''])]
    [InlineData([false, '"'])]
    public void Test_SingleChar(bool expected, char input)
    {
        bool result = LetterValidator.Validate(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData([true, "Hello"])]
    [InlineData([true, "AbcdefgHijklmnopQrstuvq"])]
    [InlineData([true, "fjnsdnjkfkjdsnjfjhbdfsbhjsdfbhjdsfbhdfshbj"])]
    [InlineData([true, "vsrboeivseoinrucrevuioneiuovcmmr"])]
    [InlineData([true, "QWERTZUISDFGHJKCVBNMFGHJ"])]
    [InlineData([false, "wertzuiopölkjhgfdsxcvbhjiolmnbvftzu"])]
    [InlineData([false, "öüöäüöäüöäü"])]
    [InlineData([false, "ààáàáÁá"])]
    [InlineData([false, "èèé"])]
    [InlineData([false, "Helfnsjfdsjfk_jfsjefejj"])]
    [InlineData([false, "Vierhundert400"])]
    [InlineData([false, "Zwei(2)"])]
    [InlineData([false, "OO__OO"])]
    [InlineData([false, ">,...,<"])]
    [InlineData([false, "L337"])]
    [InlineData([false, "Great Job Mate"])]
    public void Test_MultipleChars(bool expected, string input)
    {
        bool result = LetterValidator.ValidateLetters(input);
        Assert.Equal(expected, result);
    }
}
