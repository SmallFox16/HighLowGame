namespace HighLowGame;

public static class GuessProcessor
{
    public static (GuessResult result, string? details) Process(int guess, GameState state)
    {
        var (isDoofus, reason) = DoofusChecker.Check(guess, state);

        if (isDoofus)
        {
            state.DoofusStreak++;
            if (state.DoofusStreak >= 3)
            {
                state.IsGameOver = true;
            }
            return (GuessResult.DoofusDetected, reason);
        }

        state.DoofusStreak = 0;
        state.GuessHistory.Add(guess);
        state.ValidGuessCount++;

        if (guess < state.SecretNumber)
        {
            state.LowBound = Math.Max(state.LowBound, guess + 1);
            return (GuessResult.TooLow, null);
        }

        if (guess > state.SecretNumber)
        {
            state.HighBound = Math.Min(state.HighBound, guess - 1);
            return (GuessResult.TooHigh, null);
        }

        state.Won = true;
        state.IsGameOver = true;
        return (GuessResult.Correct, null);
    }
}
