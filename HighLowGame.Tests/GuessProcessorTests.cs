using HighLowGame;

namespace HighLowGame.Tests;

public class GuessProcessorTests
{
    // ===== SUCCESS CASES =====

    [Fact]
    public void Process_CorrectGuess_ReturnsCorrectAndWins()
    {
        var state = CreateStateWithSecret(5);

        var (result, details) = GuessProcessor.Process(5, state);

        Assert.Equal(GuessResult.Correct, result);
        Assert.True(state.Won);
        Assert.True(state.IsGameOver);
        Assert.Equal(1, state.ValidGuessCount);
        Assert.Contains(5, state.GuessHistory);
    }

    [Fact]
    public void Process_GuessTooLow_ReturnsTooLowAndUpdatesLowBound()
    {
        var state = CreateStateWithSecret(7);

        var (result, details) = GuessProcessor.Process(3, state);

        Assert.Equal(GuessResult.TooLow, result);
        Assert.Equal(4, state.LowBound); // Should be guess + 1
        Assert.False(state.IsGameOver);
        Assert.Equal(1, state.ValidGuessCount);
    }

    [Fact]
    public void Process_GuessTooHigh_ReturnsTooHighAndUpdatesHighBound()
    {
        var state = CreateStateWithSecret(3);

        var (result, details) = GuessProcessor.Process(7, state);

        Assert.Equal(GuessResult.TooHigh, result);
        Assert.Equal(6, state.HighBound); // Should be guess - 1
        Assert.False(state.IsGameOver);
        Assert.Equal(1, state.ValidGuessCount);
    }

    [Fact]
    public void Process_ValidGuess_ResetsDoofusStreak()
    {
        var state = CreateStateWithSecret(5);
        state.DoofusStreak = 2; // Had 2 doofus moves

        GuessProcessor.Process(3, state); // Valid guess (too low)

        Assert.Equal(0, state.DoofusStreak);
    }

    [Fact]
    public void Process_ValidGuess_AddsToHistory()
    {
        var state = CreateStateWithSecret(5);

        GuessProcessor.Process(3, state);
        GuessProcessor.Process(7, state);

        Assert.Equal(2, state.GuessHistory.Count);
        Assert.Contains(3, state.GuessHistory);
        Assert.Contains(7, state.GuessHistory);
    }

    [Fact]
    public void Process_MultipleTooLow_LowBoundOnlyIncreases()
    {
        var state = CreateStateWithSecret(8);

        GuessProcessor.Process(2, state);
        Assert.Equal(3, state.LowBound);

        GuessProcessor.Process(5, state);
        Assert.Equal(6, state.LowBound);

        GuessProcessor.Process(6, state);
        Assert.Equal(7, state.LowBound);
    }

    [Fact]
    public void Process_MultipleTooHigh_HighBoundOnlyDecreases()
    {
        var state = CreateStateWithSecret(2);

        GuessProcessor.Process(9, state);
        Assert.Equal(8, state.HighBound);

        GuessProcessor.Process(5, state);
        Assert.Equal(4, state.HighBound);

        GuessProcessor.Process(4, state);
        Assert.Equal(3, state.HighBound);
    }

    // ===== FAILURE CASES (DOOFUS) =====

    [Fact]
    public void Process_DoofusGuess_IncrementsStreak()
    {
        var state = CreateStateWithSecret(5);
        state.LowBound = 4; // User knows it's > 3

        var (result, details) = GuessProcessor.Process(2, state); // Doofus - below low bound

        Assert.Equal(GuessResult.DoofusDetected, result);
        Assert.Equal(1, state.DoofusStreak);
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Process_ThreeConsecutiveDoofus_GameOver()
    {
        var state = CreateStateWithSecret(5);
        state.LowBound = 4;

        GuessProcessor.Process(2, state); // Doofus 1
        Assert.Equal(1, state.DoofusStreak);
        Assert.False(state.IsGameOver);

        GuessProcessor.Process(3, state); // Doofus 2
        Assert.Equal(2, state.DoofusStreak);
        Assert.False(state.IsGameOver);

        GuessProcessor.Process(1, state); // Doofus 3
        Assert.Equal(3, state.DoofusStreak);
        Assert.True(state.IsGameOver);
        Assert.False(state.Won);
    }

    [Fact]
    public void Process_TwoDoofusThenValid_StreakResets()
    {
        var state = CreateStateWithSecret(8);
        state.LowBound = 4;

        GuessProcessor.Process(2, state); // Doofus 1
        GuessProcessor.Process(3, state); // Doofus 2
        Assert.Equal(2, state.DoofusStreak);

        GuessProcessor.Process(5, state); // Valid guess (too low)
        Assert.Equal(0, state.DoofusStreak);
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Process_TwoDoofusThenValidThenDoofus_NotKicked()
    {
        var state = CreateStateWithSecret(8);
        state.LowBound = 4;

        GuessProcessor.Process(2, state); // Doofus 1
        GuessProcessor.Process(3, state); // Doofus 2
        GuessProcessor.Process(5, state); // Valid (resets streak)

        state.LowBound = 6; // Now low bound is higher
        GuessProcessor.Process(4, state); // Doofus 1 (new streak)

        Assert.Equal(1, state.DoofusStreak);
        Assert.False(state.IsGameOver); // NOT kicked - streak was reset
    }

    [Fact]
    public void Process_DoofusGuess_DoesNotAddToHistory()
    {
        var state = CreateStateWithSecret(5);
        state.GuessHistory.Add(3); // Previous guess

        var (result, _) = GuessProcessor.Process(3, state); // Repeat guess - doofus

        Assert.Equal(GuessResult.DoofusDetected, result);
        Assert.Single(state.GuessHistory); // Still only 1 item
        Assert.Equal(0, state.ValidGuessCount);
    }

    [Fact]
    public void Process_DoofusGuess_DoesNotUpdateBounds()
    {
        var state = CreateStateWithSecret(5);
        state.LowBound = 3;
        state.HighBound = 8;

        GuessProcessor.Process(2, state); // Below low bound - doofus

        Assert.Equal(3, state.LowBound); // Unchanged
        Assert.Equal(8, state.HighBound); // Unchanged
    }

    // ===== EDGE CASES =====

    [Fact]
    public void Process_GuessExactlyAtBoundary_NotDoofus()
    {
        var state = CreateStateWithSecret(5);
        state.LowBound = 3;
        state.HighBound = 7;

        var (resultLow, _) = GuessProcessor.Process(3, state);
        Assert.Equal(GuessResult.TooLow, resultLow);

        state.Reset();
        state.LowBound = 3;
        state.HighBound = 7;
        // Need to manually set secret since Reset() randomizes it
        SetSecret(state, 5);

        var (resultHigh, _) = GuessProcessor.Process(7, state);
        Assert.Equal(GuessResult.TooHigh, resultHigh);
    }

    [Fact]
    public void Process_WinOnFirstGuess_SingleAttempt()
    {
        var state = CreateStateWithSecret(5);

        var (result, _) = GuessProcessor.Process(5, state);

        Assert.Equal(GuessResult.Correct, result);
        Assert.Equal(1, state.ValidGuessCount);
        Assert.True(state.Won);
    }

    // Helper methods
    private GameState CreateStateWithSecret(int secret)
    {
        var state = new GameState();
        SetSecret(state, secret);
        return state;
    }

    private void SetSecret(GameState state, int secret)
    {
        // Use reflection to set the private SecretNumber
        var prop = typeof(GameState).GetProperty("SecretNumber");
        prop!.SetValue(state, secret);
    }
}
