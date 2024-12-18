using SqlValidator.Identifiers;

namespace Identifiers;

public class QuotedIdValidatorTests
{
    public static (bool expected, string identifier, string expectedRemaining)[] TestCases { get; } =
    [
        (true, "\"ZeroSpaceWord\"", string.Empty),
        (false, "\"Name With Spaces\"", string.Empty),
        (true, "ZeroSpaceWordNoQuote", string.Empty),
        (true, "Name With Spaces", " With Spaces"),
        (true, "Name As \"Other Name\"", " As \"Other Name\""),
        (true, "@StartWith_AT", string.Empty),
        (true, "#StartWith_NR", string.Empty),
        (false, "_StartWith_AT", string.Empty),
        (false, "0StartWith_Digit", string.Empty),
        (false, "\"BadQuote", string.Empty),
        (false, "BadEnd\"", string.Empty),
        (false, "BadEndChar}", string.Empty),
    ];

    /// <summary>
    ///     http://teiid.github.io/teiid-documents/9.0.x/content/reference/BNF_for_SQL_Grammar.html#token_ID
    /// </summary>
    [Fact]
    public void Validate_QuotedIdentifiers()
    {
        foreach ((bool expected, string identifier, string expectedRemaining) in TestCases)
        {
            if (expected)
            {
                Assert.True(QuotedIdValidator.Validate(identifier, out ReadOnlySpan<char> remaining), $"Result should be true with {identifier}");
                Assert.Equal(expectedRemaining, remaining);
                continue;
            }

            Assert.False(QuotedIdValidator.Validate(identifier, out _), $"Result should be false with {identifier}");
        }
    }
}
