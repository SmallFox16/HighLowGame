namespace HighLowGame;

public static class Display
{
    public static void ShowWelcome()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║       HIGH-LOW GUESSING GAME           ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        ShowRules();
        Console.WriteLine();
    }

    public static void ShowRules()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("RULES:");
        Console.ResetColor();
        Console.WriteLine("• I'm thinking of a number between 1 and 10");
        Console.WriteLine("• Guess the number - I'll tell you if it's higher or lower");
        Console.WriteLine("• You have 5 valid guesses to find it!");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("DOOFUS RULES (Don't be a doofus!):");
        Console.ResetColor();
        Console.WriteLine("• Don't guess outside the narrowed range");
        Console.WriteLine("• Don't repeat a guess you already made");
        Console.WriteLine("• 3 doofus moves IN A ROW = You're kicked out!");
        Console.WriteLine("• Invalid input (letters, out of 1-10) counts as a doofus move!");
        Console.WriteLine();
    }

    public static void PromptGuess(GameState state)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"Guess a number between {state.LowBound} and {state.HighBound}");
        Console.ResetColor();
        Console.Write($" (Guess {state.ValidGuessCount + 1}/5): ");
    }

    public static void ShowResult(GuessResult result, string? details)
    {
        switch (result)
        {
            case GuessResult.TooLow:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("📈 Higher! The number is higher.");
                break;
            case GuessResult.TooHigh:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("📉 Lower! The number is lower.");
                break;
            case GuessResult.Correct:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("🎉 CORRECT!");
                break;
            case GuessResult.DoofusDetected:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"🤦 DOOFUS MOVE! {details}");
                break;
        }
        Console.ResetColor();
    }

    public static void ShowDoofusWarning(int streak)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        int remaining = 3 - streak;
        Console.WriteLine($"⚠️  Strike {streak}! {remaining} more doofus move{(remaining == 1 ? "" : "s")} and you're out!");
        Console.ResetColor();
    }

    public static void ShowDoofusKick()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  THREE DOOFUS MOVES IN A ROW!          ║");
        Console.WriteLine("║  YOU'RE KICKED OUT! 🚪👋               ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
    }

    public static void ShowWin(int attempts, int secretNumber)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║           🎊 YOU WIN! 🎊               ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine($"The number was {secretNumber}!");
        Console.WriteLine($"You got it in {attempts} guess{(attempts == 1 ? "" : "es")}!");
        Console.ResetColor();
    }

    public static void ShowOutOfGuesses(int secretNumber)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║      OUT OF GUESSES! GAME OVER         ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine($"The number was {secretNumber}.");
        Console.ResetColor();
    }

    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ {message}");
        Console.ResetColor();
    }

    public static void ShowPlayAgain(int currentWins, int maxWins)
    {
        Console.WriteLine();
        int remaining = maxWins - currentWins;
        Console.WriteLine($"You have {remaining} replay{(remaining == 1 ? "" : "s")} remaining!");
        Console.Write("Play again? (Y/N): ");
    }

    public static void ShowMaxWinsReached(int totalWins)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║      🏆 CHAMPION! 🏆                   ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine($"You won {totalWins} games! No more replays available.");
        Console.ResetColor();
    }

    public static void ShowGoodbye()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Thanks for playing! Goodbye! 👋");
        Console.ResetColor();
    }

    public static void ShowSecretReveal(int secretNumber)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"(The secret number was {secretNumber})");
        Console.ResetColor();
    }
}
