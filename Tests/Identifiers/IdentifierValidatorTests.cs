using SqlValidator.Identifiers;

namespace Identifiers;
public class IdentifierValidatorTests
{   
    public static (bool expected, string identifier, string expectedRemaining)[] TestCases { get; } =
    [
        (true, "\"ZeroSpaceWord\".Jimmy.@Willhelm", string.Empty),
        (true, "\"ZeroSpaceWord\".#Jimmy..Willhelm", string.Empty),
        (true, "\"ZeroSpaceWord\".tempDB..#Willhelm", string.Empty),
        (false, "\"Multi Space Word\".tempDB..\"#Willhelm der Große\"", string.Empty)
    ];

    public static IEnumerable<(bool expected, string identifier, string expectedRemaining)> AllTestCases =>
        TestCases.Concat(QuotedIdValidatorTests.TestCases);

    /// <summary>
    ///     http://teiid.github.io/teiid-documents/9.0.x/content/reference/BNF_for_SQL_Grammar.html#id
    /// </summary>
    [Fact]
    public void Validate_Identifiers()
    {
        foreach ((bool expected, string identifier, string expectedRemaining) in AllTestCases)
        {
            if (expected)
            {
                Assert.True(IdentifierValidator.Validate(identifier, out ReadOnlySpan<char> remaining), $"Result should be true with {identifier}");
                Assert.Equal(expectedRemaining, remaining);
                continue;
            }

            Assert.False(IdentifierValidator.Validate(identifier, out _), $"Result should be false with {identifier}");
        }
    }
}
