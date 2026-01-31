using HighLowGame;

try
{
    Console.Title = "High-Low Guessing Game";
}
catch
{
    // Console.Title may not be supported on all platforms
}

try
{
    var game = new Game();
    game.Run();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    Console.ResetColor();
    Environment.Exit(1);
}
