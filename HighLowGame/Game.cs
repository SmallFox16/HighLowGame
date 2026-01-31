namespace HighLowGame;

public class Game
{
    private readonly GameState _state;
    private const int MaxValidGuesses = 5;
    private const int MaxWins = 3;
    private int _winCount = 0;

    public Game()
    {
        _state = new GameState();
    }

    public void Run()
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            Console.WriteLine();
            Display.ShowGoodbye();
            Environment.Exit(0);
        };

        bool keepPlaying = true;

        while (keepPlaying)
        {
            _state.Reset();
            Display.ShowWelcome();

            while (!_state.IsGameOver)
            {
                if (_state.ValidGuessCount >= MaxValidGuesses)
                {
                    _state.IsGameOver = true;
                    Display.ShowOutOfGuesses(_state.SecretNumber);
                    break;
                }

                Display.PromptGuess(_state);
                string? input = Console.ReadLine();

                var (isValid, parsedNumber, errorMessage) = InputValidator.Validate(input);

                if (!isValid)
                {
                    Display.ShowError(errorMessage!);
                    _state.DoofusStreak++;

                    if (_state.DoofusStreak >= 3)
                    {
                        Display.ShowDoofusKick();
                        Display.ShowSecretReveal(_state.SecretNumber);
                        _state.IsGameOver = true;
                    }
                    else
                    {
                        Display.ShowDoofusWarning(_state.DoofusStreak);
                    }
                    continue;
                }

                var (result, details) = GuessProcessor.Process(parsedNumber!.Value, _state);
                Display.ShowResult(result, details);

                switch (result)
                {
                    case GuessResult.Correct:
                        Display.ShowWin(_state.ValidGuessCount, _state.SecretNumber);
                        break;
                    case GuessResult.DoofusDetected:
                        if (_state.IsGameOver)
                        {
                            Display.ShowDoofusKick();
                            Display.ShowSecretReveal(_state.SecretNumber);
                        }
                        else
                        {
                            Display.ShowDoofusWarning(_state.DoofusStreak);
                        }
                        break;
                }
            }

            if (_state.Won)
            {
                _winCount++;
                if (_winCount >= MaxWins)
                {
                    Display.ShowMaxWinsReached(_winCount);
                    keepPlaying = false;
                }
                else
                {
                    keepPlaying = AskPlayAgain(_winCount);
                }
            }
            else
            {
                keepPlaying = false;
            }
        }

        Display.ShowGoodbye();
    }

    private bool AskPlayAgain(int currentWins)
    {
        while (true)
        {
            Display.ShowPlayAgain(currentWins, MaxWins);
            string? input = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (input == "Y" || input == "YES")
            {
                return true;
            }

            if (input == "N" || input == "NO")
            {
                return false;
            }

            Console.WriteLine("Please enter Y or N.");
        }
    }
}
