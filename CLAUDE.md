High-Low Guessing Game — Claude Code Implementation Plan
Project Overview
Build a C# console application where:

Computer generates a random number 1-10 (inclusive)
User guesses until correct or gets kicked out for 3 consecutive "doofus" moves
Doofus moves: guessing outside the narrowed range, or repeating the same guess
Bulletproof input handling


Step 1: Project Setup
Create a new C# console application:
- Create directory `HighLowGame`
- Initialize with `dotnet new console`
- Target .NET 8 or later
- Name the main file `Program.cs`

Step 2: Define the Game State Class
Create a GameState class to track all game data:
Properties needed:
- SecretNumber (int) — the answer
- LowBound (int) — current minimum valid guess, starts at 1
- HighBound (int) — current maximum valid guess, starts at 10
- GuessHistory (List<int>) — all guesses made
- DoofusStreak (int) — consecutive doofus moves, resets on valid guess
- IsGameOver (bool)
- Won (bool)

Methods:
- Reset() — reinitialize for new game

Step 3: Define the Doofus Rule Checker
Create a DoofusChecker class or method:
Input: proposed guess, current GameState
Output: (bool isDoofus, string reason)

Check these conditions:
1. Guess < LowBound → doofus ("You already know it's higher than {LowBound - 1}!")
2. Guess > HighBound → doofus ("You already know it's lower than {HighBound + 1}!")
3. Guess equals the last guess → doofus ("You just guessed that!")
4. Guess already in GuessHistory AND bounds haven't changed since → doofus ("You already tried that number!")

If not doofus, return (false, null)

Step 4: Define the Input Validator
Create an InputValidator class or method:
Input: raw string from Console.ReadLine()
Output: (bool isValid, int? parsedNumber, string errorMessage)

Validation steps:
1. Check for null/empty/whitespace → "Please enter something!"
2. Check if it's a valid integer → "That's not a number!"
3. Check if it's in range 1-10 → "Pick a number between 1 and 10!"

Return parsed int if valid, error message if not

Step 5: Define the Guess Processor
Create a GuessProcessor class or method:
Input: valid guess (int), GameState
Output: GuessResult enum { TooLow, TooHigh, Correct, DoofusDetected }

Logic:
1. Run DoofusChecker first
   - If doofus: increment DoofusStreak, check if >= 3 (game over), return DoofusDetected
2. If not doofus: reset DoofusStreak to 0
3. Add guess to GuessHistory
4. Compare to SecretNumber:
   - If guess < SecretNumber: update LowBound = max(LowBound, guess + 1), return TooLow
   - If guess > SecretNumber: update HighBound = min(HighBound, guess - 1), return TooHigh
   - If equal: set Won = true, IsGameOver = true, return Correct

Step 6: Define the Display Helper
Create a Display static class for all console output:
Methods:
- ShowWelcome() — game title, rules
- ShowRules() — explain doofus rules
- PromptGuess(GameState state) — show current valid range, ask for guess
- ShowResult(GuessResult result, string details) — "Higher!", "Lower!", "Correct!", or doofus message
- ShowDoofusWarning(int streak) — "Strike {streak}! Two more and you're out!"
- ShowDoofusKick() — "Three doofus moves in a row! You're out!"
- ShowWin(int attempts) — celebration message
- ShowPlayAgain() — ask if they want to play again
- ShowGoodbye()

Step 7: Define the Game Loop
Create the main Game class:
Method: Run()

Outer loop (play again):
  1. Display welcome
    welcome needs full rules to the game
  2. Initialize GameState with new random number
  
  Inner loop (guessing):
    1. Display prompt with current range
    2. Read input
    3. Validate input
       - If invalid: show error, continue loop (don't count as doofus)
    4. Process guess
       - If doofus: show warning or kick message
       - If too low/high: show hint, continue
       - If correct: show win, break
    5. Check if game over (doofus kick)
  
  3. Ask play again
     - Validate Y/N input
     - If yes: continue outer loop
     - If no: break

Display goodbye

Step 8: Wire Up Program.cs
Main():
  - Set console title
  - Create Game instance
  - Call Game.Run()
  - Handle any uncaught exceptions gracefully

Step 9: Add Edge Case Handling
Make sure these scenarios work:
Test cases to verify:
1. User types "abc" → error, reprompt ( a doofus strike)
2. User types "0" → error, reprompt ( a doofus strike)  
3. User types "11" → error, reprompt ( a doofus strike)
4. User types "  5  " → should work (trim whitespace)
5. User types "" and hits enter → error, reprompt
6. User guesses 3 when they know it's > 5 → doofus strike
7. User guesses same number 3x in a row → kicked
8. User guesses 5, then 3 (when answer > 5), then 2 → kicked (3 doofus in a row)
9. User makes 2 doofus moves, then 1 valid, then 1 doofus → NOT kicked (streak reset)
10. Ctrl+C → graceful exit

Step 10: Final Structure
HighLowGame/
├── Program.cs          — entry point
├── Game.cs             — main game loop
├── GameState.cs        — tracks all game data
├── GuessProcessor.cs   — processes guesses, updates state
├── DoofusChecker.cs    — detects doofus moves
├── InputValidator.cs   — validates console input
├── Display.cs          — all console output
└── GuessResult.cs      — enum for guess outcomes

Implementation Order

GuessResult.cs (enum — no dependencies)
GameState.cs (data class — no dependencies)
InputValidator.cs (standalone)
DoofusChecker.cs (depends on GameState)
Display.cs (standalone)
GuessProcessor.cs (depends on GameState, DoofusChecker, GuessResult)
Game.cs (depends on everything)
Program.cs (entry point)
Test all edge cases
Add final polish (colors, formatting)


Optional Enhancements (if time permits)



limit the user to 5 valid guesses

