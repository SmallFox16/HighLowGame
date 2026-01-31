using HighLowGame;

namespace HighLowGame.Tests;

/// <summary>
/// Integration tests that simulate full game scenarios from CLAUDE.md
/// </summary>
public class IntegrationTests
{
    // ===== SCENARIO TESTS FROM CLAUDE.md =====

    [Fact]
    public void Scenario_UserGuessesCorrectOnFirstTry()
    {
        var state = CreateStateWithSecret(5);

        var (result, _) = GuessProcessor.Process(5, state);

        Assert.Equal(GuessResult.Correct, result);
        Assert.True(state.Won);
        Assert.True(state.IsGameOver);
        Assert.Equal(1, state.ValidGuessCount);
    }

    [Fact]
    public void Scenario_UserNarrowsDownAndWins()
    {
        var state = CreateStateWithSecret(7);

        // Guess 5 - too low
        var (r1, _) = GuessProcessor.Process(5, state);
        Assert.Equal(GuessResult.TooLow, r1);
        Assert.Equal(6, state.LowBound);

        // Guess 9 - too high
        var (r2, _) = GuessProcessor.Process(9, state);
        Assert.Equal(GuessResult.TooHigh, r2);
        Assert.Equal(8, state.HighBound);

        // Guess 7 - correct!
        var (r3, _) = GuessProcessor.Process(7, state);
        Assert.Equal(GuessResult.Correct, r3);
        Assert.True(state.Won);
        Assert.Equal(3, state.ValidGuessCount);
    }

    [Fact]
    public void Scenario_UserGuesses3WhenKnowsHigherThan5_DoofusStrike()
    {
        // From CLAUDE.md test case #6
        var state = CreateStateWithSecret(8);

        // First guess 5, told it's higher
        GuessProcessor.Process(5, state);
        Assert.Equal(6, state.LowBound);

        // Now guess 3 - doofus!
        var (result, reason) = GuessProcessor.Process(3, state);

        Assert.Equal(GuessResult.DoofusDetected, result);
        Assert.Equal("You already know it's higher than 5!", reason);
        Assert.Equal(1, state.DoofusStreak);
    }

    [Fact]
    public void Scenario_UserGuessesSameNumber3xInARow_Kicked()
    {
        // From CLAUDE.md test case #7
        var state = CreateStateWithSecret(5);

        // First guess 3 - valid
        GuessProcessor.Process(3, state);
        Assert.Equal(0, state.DoofusStreak);

        // Guess 3 again - doofus
        GuessProcessor.Process(3, state);
        Assert.Equal(1, state.DoofusStreak);

        // Guess 3 again - doofus
        GuessProcessor.Process(3, state);
        Assert.Equal(2, state.DoofusStreak);

        // Guess 3 again - doofus, KICKED
        var (result, _) = GuessProcessor.Process(3, state);
        Assert.Equal(GuessResult.DoofusDetected, result);
        Assert.Equal(3, state.DoofusStreak);
        Assert.True(state.IsGameOver);
        Assert.False(state.Won);
    }

    [Fact]
    public void Scenario_Guess5Then3Then2WhenAnswerHigher_Kicked()
    {
        // From CLAUDE.md test case #8
        var state = CreateStateWithSecret(8);

        // Guess 5 - too low (valid)
        GuessProcessor.Process(5, state);
        Assert.Equal(6, state.LowBound);
        Assert.Equal(0, state.DoofusStreak);

        // Guess 3 - doofus (below low bound)
        GuessProcessor.Process(3, state);
        Assert.Equal(1, state.DoofusStreak);

        // Guess 2 - doofus
        GuessProcessor.Process(2, state);
        Assert.Equal(2, state.DoofusStreak);

        // Guess 1 - doofus, KICKED
        var (result, _) = GuessProcessor.Process(1, state);
        Assert.Equal(3, state.DoofusStreak);
        Assert.True(state.IsGameOver);
    }

    [Fact]
    public void Scenario_2DoofusThen1ValidThen1Doofus_NotKicked()
    {
        // From CLAUDE.md test case #9
        var state = CreateStateWithSecret(8);

        // Guess 5 - valid, too low
        GuessProcessor.Process(5, state);
        Assert.Equal(6, state.LowBound);

        // Guess 3 - doofus 1
        GuessProcessor.Process(3, state);
        Assert.Equal(1, state.DoofusStreak);

        // Guess 2 - doofus 2
        GuessProcessor.Process(2, state);
        Assert.Equal(2, state.DoofusStreak);

        // Guess 7 - valid! Streak resets
        GuessProcessor.Process(7, state);
        Assert.Equal(0, state.DoofusStreak);
        Assert.False(state.IsGameOver);

        // Guess 7 again - doofus 1 (new streak)
        GuessProcessor.Process(7, state);
        Assert.Equal(1, state.DoofusStreak);
        Assert.False(state.IsGameOver); // NOT kicked!
    }

    // ===== INPUT VALIDATION INTEGRATION =====

    [Fact]
    public void Scenario_InputAbc_ReturnsError()
    {
        // From CLAUDE.md test case #1
        var (isValid, _, errorMessage) = InputValidator.Validate("abc");

        Assert.False(isValid);
        Assert.Equal("That's not a number!", errorMessage);
    }

    [Fact]
    public void Scenario_Input0_ReturnsOutOfRangeError()
    {
        // From CLAUDE.md test case #2
        var (isValid, _, errorMessage) = InputValidator.Validate("0");

        Assert.False(isValid);
        Assert.Equal("Pick a number between 1 and 10!", errorMessage);
    }

    [Fact]
    public void Scenario_Input11_ReturnsOutOfRangeError()
    {
        // From CLAUDE.md test case #3
        var (isValid, _, errorMessage) = InputValidator.Validate("11");

        Assert.False(isValid);
        Assert.Equal("Pick a number between 1 and 10!", errorMessage);
    }

    [Fact]
    public void Scenario_InputWithSpaces_TrimsAndWorks()
    {
        // From CLAUDE.md test case #4
        var (isValid, parsedNumber, _) = InputValidator.Validate("  5  ");

        Assert.True(isValid);
        Assert.Equal(5, parsedNumber);
    }

    [Fact]
    public void Scenario_EmptyInput_ReturnsError()
    {
        // From CLAUDE.md test case #5
        var (isValid, _, errorMessage) = InputValidator.Validate("");

        Assert.False(isValid);
        Assert.Equal("Please enter something!", errorMessage);
    }

    // ===== 5 GUESS LIMIT TESTS =====

    [Fact]
    public void Scenario_UseAll5GuessesWithoutWinning()
    {
        var state = CreateStateWithSecret(10);

        // Make 5 valid guesses, none correct
        GuessProcessor.Process(1, state);
        GuessProcessor.Process(2, state);
        GuessProcessor.Process(3, state);
        GuessProcessor.Process(4, state);
        GuessProcessor.Process(5, state);

        Assert.Equal(5, state.ValidGuessCount);
        Assert.False(state.Won);
        // Note: Game.cs checks ValidGuessCount >= 5 and ends the game
    }

    [Fact]
    public void Scenario_WinOnExactly5thGuess()
    {
        var state = CreateStateWithSecret(5);

        GuessProcessor.Process(1, state);
        GuessProcessor.Process(2, state);
        GuessProcessor.Process(3, state);
        GuessProcessor.Process(4, state);
        var (result, _) = GuessProcessor.Process(5, state);

        Assert.Equal(GuessResult.Correct, result);
        Assert.Equal(5, state.ValidGuessCount);
        Assert.True(state.Won);
    }

    [Fact]
    public void Scenario_DoofusMovesDoNotCountTowardGuessLimit()
    {
        var state = CreateStateWithSecret(8);

        // Valid guess
        GuessProcessor.Process(5, state);
        Assert.Equal(1, state.ValidGuessCount);

        // Doofus moves (guessing below new low bound of 6)
        GuessProcessor.Process(3, state);
        GuessProcessor.Process(4, state);
        Assert.Equal(1, state.ValidGuessCount); // Still 1!

        // More valid guesses
        GuessProcessor.Process(6, state);
        GuessProcessor.Process(7, state);
        Assert.Equal(3, state.ValidGuessCount);
    }

    // ===== BOUNDARY NARROWING TESTS =====

    [Fact]
    public void Scenario_BoundsNarrowCorrectly()
    {
        var state = CreateStateWithSecret(5);

        // Start: 1-10
        Assert.Equal(1, state.LowBound);
        Assert.Equal(10, state.HighBound);

        // Guess 3, too low
        GuessProcessor.Process(3, state);
        Assert.Equal(4, state.LowBound); // 3+1
        Assert.Equal(10, state.HighBound);

        // Guess 8, too high
        GuessProcessor.Process(8, state);
        Assert.Equal(4, state.LowBound);
        Assert.Equal(7, state.HighBound); // 8-1

        // Guess 4, too low
        GuessProcessor.Process(4, state);
        Assert.Equal(5, state.LowBound); // 4+1
        Assert.Equal(7, state.HighBound);

        // Now range is 5-7, secret is 5
    }

    [Fact]
    public void Scenario_GuessAtExactBoundIsValid()
    {
        var state = CreateStateWithSecret(5);
        state.LowBound = 5;
        state.HighBound = 5;

        // Only valid guess is 5
        var (result, _) = GuessProcessor.Process(5, state);

        Assert.Equal(GuessResult.Correct, result);
    }

    // Helper methods
    private GameState CreateStateWithSecret(int secret)
    {
        var state = new GameState();
        var prop = typeof(GameState).GetProperty("SecretNumber");
        prop!.SetValue(state, secret);
        return state;
    }
}
