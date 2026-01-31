using HighLowGame;

namespace HighLowGame.Tests;

public class InputValidatorTests
{
    // ===== SUCCESS CASES =====

    [Theory]
    [InlineData("1", 1)]
    [InlineData("5", 5)]
    [InlineData("10", 10)]
    public void Validate_ValidNumbers_ReturnsSuccess(string input, int expected)
    {
        var (isValid, parsedNumber, errorMessage) = InputValidator.Validate(input);

        Assert.True(isValid);
        Assert.Equal(expected, parsedNumber);
        Assert.Null(errorMessage);
    }

    [Theory]
    [InlineData("  5  ", 5)]
    [InlineData(" 1", 1)]
    [InlineData("10 ", 10)]
    [InlineData("   7   ", 7)]
    public void Validate_NumberWithWhitespace_TrimsAndReturnsSuccess(string input, int expected)
    {
        var (isValid, parsedNumber, errorMessage) = InputValidator.Validate(input);

        Assert.True(isValid);
        Assert.Equal(expected, parsedNumber);
        Assert.Null(errorMessage);
    }

    // ===== FAILURE CASES =====

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Validate_NullOrWhitespace_ReturnsError(string? input)
    {
        var (isValid, parsedNumber, errorMessage) = InputValidator.Validate(input);

        Assert.False(isValid);
        Assert.Null(parsedNumber);
        Assert.Equal("Please enter something!", errorMessage);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("five")]
    [InlineData("1.5")]
    [InlineData("1,5")]
    [InlineData("!@#")]
    [InlineData("1a")]
    [InlineData("a1")]
    [InlineData("-")]
    [InlineData("+")]
    public void Validate_NonNumericInput_ReturnsNotANumberError(string input)
    {
        var (isValid, parsedNumber, errorMessage) = InputValidator.Validate(input);

        Assert.False(isValid);
        Assert.Null(parsedNumber);
        Assert.Equal("That's not a number!", errorMessage);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("-5")]
    [InlineData("11")]
    [InlineData("100")]
    [InlineData("999")]
    [InlineData("-100")]
    public void Validate_OutOfRange_ReturnsRangeError(string input)
    {
        var (isValid, parsedNumber, errorMessage) = InputValidator.Validate(input);

        Assert.False(isValid);
        Assert.Null(parsedNumber);
        Assert.Equal("Pick a number between 1 and 10!", errorMessage);
    }
}
