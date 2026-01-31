using HighLowGame;

namespace HighLowGame.Tests;

public class GameStateTests
{
    // ===== INITIALIZATION TESTS =====

    [Fact]
    public void Constructor_InitializesWithCorrectDefaults()
    {
        var state = new GameState();

        Assert.Equal(1, state.LowBound);
        Assert.Equal(10, state.HighBound);
        Assert.Empty(state.GuessHistory);
        Assert.Equal(0, state.DoofusStreak);
        Assert.False(state.IsGameOver);
        Assert.False(state.Won);
        Assert.Equal(0, state.ValidGuessCount);
    }

    [Fact]
    public void Constructor_GeneratesSecretInRange()
    {
        // Run multiple times to check randomness stays in bounds
        for (int i = 0; i < 100; i++)
        {
            var state = new GameState();
            Assert.InRange(state.SecretNumber, 1, 10);
        }
    }

    // ===== RESET TESTS =====

    [Fact]
    public void Reset_ClearsGuessHistory()
    {
        var state = new GameState();
        state.GuessHistory.Add(1);
        state.GuessHistory.Add(2);
        state.GuessHistory.Add(3);

        state.Reset();

        Assert.Empty(state.GuessHistory);
    }

    [Fact]
    public void Reset_RestoresBounds()
    {
        var state = new GameState();
        state.LowBound = 5;
        state.HighBound = 7;

        state.Reset();

        Assert.Equal(1, state.LowBound);
        Assert.Equal(10, state.HighBound);
    }

    [Fact]
    public void Reset_ClearsDoofusStreak()
    {
        var state = new GameState();
        state.DoofusStreak = 2;

        state.Reset();

        Assert.Equal(0, state.DoofusStreak);
    }

    [Fact]
    public void Reset_ClearsGameOverAndWon()
    {
        var state = new GameState();
        state.IsGameOver = true;
        state.Won = true;

        state.Reset();

        Assert.False(state.IsGameOver);
        Assert.False(state.Won);
    }

    [Fact]
    public void Reset_ClearsValidGuessCount()
    {
        var state = new GameState();
        state.ValidGuessCount = 4;

        state.Reset();

        Assert.Equal(0, state.ValidGuessCount);
    }

    [Fact]
    public void Reset_GeneratesNewSecretNumber()
    {
        var state = new GameState();
        var secrets = new HashSet<int>();

        // Reset many times and collect secrets
        for (int i = 0; i < 100; i++)
        {
            state.Reset();
            secrets.Add(state.SecretNumber);
        }

        // Should have generated multiple different secrets (statistically)
        Assert.True(secrets.Count > 1, "Reset should generate different secret numbers");
    }

    // ===== STATE MODIFICATION TESTS =====

    [Fact]
    public void LowBound_CanBeModified()
    {
        var state = new GameState();
        state.LowBound = 5;

        Assert.Equal(5, state.LowBound);
    }

    [Fact]
    public void HighBound_CanBeModified()
    {
        var state = new GameState();
        state.HighBound = 7;

        Assert.Equal(7, state.HighBound);
    }

    [Fact]
    public void DoofusStreak_CanBeIncremented()
    {
        var state = new GameState();

        state.DoofusStreak++;
        Assert.Equal(1, state.DoofusStreak);

        state.DoofusStreak++;
        Assert.Equal(2, state.DoofusStreak);
    }

    [Fact]
    public void GuessHistory_CanAddItems()
    {
        var state = new GameState();

        state.GuessHistory.Add(5);
        state.GuessHistory.Add(3);
        state.GuessHistory.Add(7);

        Assert.Equal(3, state.GuessHistory.Count);
        Assert.Equal(new[] { 5, 3, 7 }, state.GuessHistory);
    }

    [Fact]
    public void ValidGuessCount_CanBeIncremented()
    {
        var state = new GameState();

        state.ValidGuessCount++;
        state.ValidGuessCount++;
        state.ValidGuessCount++;

        Assert.Equal(3, state.ValidGuessCount);
    }
}
