namespace HighLowGame;

public static class InputValidator
{
    public static (bool isValid, int? parsedNumber, string? errorMessage) Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (false, null, "Please enter something!");
        }

        string trimmed = input.Trim();

        if (!int.TryParse(trimmed, out int number))
        {
            return (false, null, "That's not a number!");
        }

        if (number < 1 || number > 10)
        {
            return (false, null, "Pick a number between 1 and 10!");
        }

        return (true, number, null);
    }
}
