using SqlValidator;

namespace Tests;
public class NextTokenTests
{
    [Fact]
    public void NextTokenString()
    {
        Assert.True(Helper.TryGetNextToken("\t'Hello World'", out ReadOnlySpan<char> token));
        Assert.Equal("'Hello World'", token);

        Assert.True(Helper.TryGetNextToken("'Hello World'\t", out token));
        Assert.Equal("'Hello World'", token);

        Assert.True(Helper.TryGetNextToken("\t'Hello ''World'''", out token));
        Assert.Equal("'Hello ''World'''", token);

        Assert.False(Helper.TryGetNextToken("\t'Hello World''", out _));
        Assert.False(Helper.TryGetNextToken("\t''Hello World'", out _));

        Assert.True(Helper.TryGetNextToken("Set Namespace 'Hello My Name' As \"George\"", out token));
        Assert.Equal("Set", token);

        Assert.True(Helper.TryGetNextToken(" Namespace 'Hello My Name' As \"George\"", out token));
        Assert.Equal("Namespace", token);

        Assert.True(Helper.TryGetNextToken(" 'Hello My Name' As \"George\"", out token));
        Assert.Equal("'Hello My Name'", token);

        Assert.True(Helper.TryGetNextToken(" As \"George\"", out token));
        Assert.Equal("As", token);

        Assert.True(Helper.TryGetNextToken(" \"George\"", out token));
        Assert.Equal("\"George\"", token);
    }

    [Fact]
    public void HasNextToken()
    {
        Assert.True("Set Namespace".AsSpan().HasNextToken("SET", out ReadOnlySpan<char> remaining));
        Assert.Equal(" Namespace", remaining);

        Assert.False("Set Namespace".AsSpan().HasNextToken("NAMESPACE", out _));
    }

    [Fact]
    public void HasAnyNextToken()
    {
        Assert.True("Set Namespace".AsSpan().HasAnyNextToken(out ReadOnlySpan<char> remaining, "Set", "Create", "Namespace"));
        Assert.Equal(" Namespace", remaining);

        Assert.False("Set Namespace".AsSpan().HasAnyNextToken(out _, "NAMESPACE"));
    }
}
