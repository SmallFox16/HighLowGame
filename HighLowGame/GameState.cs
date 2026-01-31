namespace HighLowGame;

public class GameState
{
    private static readonly Random _random = new();

    public int SecretNumber { get; private set; }
    public int LowBound { get; set; }
    public int HighBound { get; set; }
    public List<int> GuessHistory { get; private set; }
    public int DoofusStreak { get; set; }
    public bool IsGameOver { get; set; }
    public bool Won { get; set; }
    public int ValidGuessCount { get; set; }

    public GameState()
    {
        GuessHistory = new List<int>();
        Reset();
    }

    public void Reset()
    {
        SecretNumber = _random.Next(1, 11);
        LowBound = 1;
        HighBound = 10;
        GuessHistory.Clear();
        DoofusStreak = 0;
        IsGameOver = false;
        Won = false;
        ValidGuessCount = 0;
    }
}
