namespace HighLowGame;

public static class DoofusChecker
{
    public static (bool isDoofus, string? reason) Check(int guess, GameState state)
    {
        if (guess < state.LowBound)
        {
            return (true, $"You already know it's higher than {state.LowBound - 1}!");
        }

        if (guess > state.HighBound)
        {
            return (true, $"You already know it's lower than {state.HighBound + 1}!");
        }

        if (state.GuessHistory.Count > 0 && state.GuessHistory[^1] == guess)
        {
            return (true, "You just guessed that!");
        }

        if (state.GuessHistory.Contains(guess))
        {
            return (true, "You already tried that number!");
        }

        return (false, null);
    }
}
