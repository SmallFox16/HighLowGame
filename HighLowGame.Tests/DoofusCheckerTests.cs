using HighLowGame;

namespace HighLowGame.Tests;

public class DoofusCheckerTests
{
    // ===== SUCCESS CASES (NOT DOOFUS) =====

    [Fact]
    public void Check_ValidGuessInRange_ReturnsNotDoofus()
    {
        var state = new GameState();
        // Secret doesn't matter for this test, just checking bounds

        var (isDoofus, reason) = DoofusChecker.Check(5, state);

        Assert.False(isDoofus);
        Assert.Null(reason);
    }

    [Fact]
    public void Check_GuessAtLowBound_ReturnsNotDoofus()
    {
        var state = new GameState();
        state.LowBound = 3;

        var (isDoofus, reason) = DoofusChecker.Check(3, state);

        Assert.False(isDoofus);
        Assert.Null(reason);
    }

    [Fact]
    public void Check_GuessAtHighBound_ReturnsNotDoofus()
    {
        var state = new GameState();
        state.HighBound = 7;

        var (isDoofus, reason) = DoofusChecker.Check(7, state);

        Assert.False(isDoofus);
        Assert.Null(reason);
    }

    [Fact]
    public void Check_DifferentGuessFromHistory_ReturnsNotDoofus()
    {
        var state = new GameState();
        state.GuessHistory.Add(3);
        state.GuessHistory.Add(7);

        var (isDoofus, reason) = DoofusChecker.Check(5, state);

        Assert.False(isDoofus);
        Assert.Null(reason);
    }

    // ===== FAILURE CASES (DOOFUS DETECTED) =====

    [Fact]
    public void Check_GuessBelowLowBound_ReturnsDoofus()
    {
        var state = new GameState();
        state.LowBound = 5; // User knows it's > 4

        var (isDoofus, reason) = DoofusChecker.Check(3, state);

        Assert.True(isDoofus);
        Assert.Equal("You already know it's higher than 4!", reason);
    }

    [Fact]
    public void Check_GuessAboveHighBound_ReturnsDoofus()
    {
        var state = new GameState();
        state.HighBound = 5; // User knows it's < 6

        var (isDoofus, reason) = DoofusChecker.Check(7, state);

        Assert.True(isDoofus);
        Assert.Equal("You already know it's lower than 6!", reason);
    }

    [Fact]
    public void Check_RepeatLastGuess_ReturnsDoofusJustGuessedThat()
    {
        var state = new GameState();
        state.GuessHistory.Add(5);

        var (isDoofus, reason) = DoofusChecker.Check(5, state);

        Assert.True(isDoofus);
        Assert.Equal("You just guessed that!", reason);
    }

    [Fact]
    public void Check_RepeatOlderGuess_ReturnsDoofusAlreadyTried()
    {
        var state = new GameState();
        state.GuessHistory.Add(5);
        state.GuessHistory.Add(7); // Last guess is 7

        var (isDoofus, reason) = DoofusChecker.Check(5, state);

        Assert.True(isDoofus);
        Assert.Equal("You already tried that number!", reason);
    }

    [Fact]
    public void Check_GuessOneBelowLowBound_ReturnsDoofus()
    {
        var state = new GameState();
        state.LowBound = 6;

        var (isDoofus, reason) = DoofusChecker.Check(5, state);

        Assert.True(isDoofus);
        Assert.Equal("You already know it's higher than 5!", reason);
    }

    [Fact]
    public void Check_GuessOneAboveHighBound_ReturnsDoofus()
    {
        var state = new GameState();
        state.HighBound = 4;

        var (isDoofus, reason) = DoofusChecker.Check(5, state);

        Assert.True(isDoofus);
        Assert.Equal("You already know it's lower than 5!", reason);
    }
}
