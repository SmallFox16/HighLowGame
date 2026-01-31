# High-Low Guessing Game - Test Results & Rules

## Game Rules

### Objective
Guess a randomly generated number between 1 and 10 (inclusive).

### Core Rules
- **Valid Range**: You can only guess numbers between 1 and 10
- **Guess Limit**: You have **5 valid guesses** to find the number
- **Feedback**: After each guess, you'll be told if the number is higher or lower
- **Range Narrowing**: The valid guess range narrows as you play:
  - If you guess too low, the minimum bound increases
  - If you guess too high, the maximum bound decreases

### Doofus Rules (Don't be a doofus!)
A "doofus move" is any of the following:
1. **Guessing outside the narrowed range** - If you know it's higher than 5, don't guess 3!
2. **Repeating a guess** - Don't guess the same number twice
3. **Invalid input** - Entering non-numeric text, empty input, or numbers outside 1-10

### Consequences
- **3 consecutive doofus moves = You're kicked out!**
- Doofus moves **do not count** toward your 5 valid guesses
- A valid guess **resets** your doofus streak to 0

### Win Condition
- Guess the correct number within 5 valid guesses
- You can win up to 3 games before the game ends

### Loss Conditions
- Make 3 consecutive doofus moves (kicked out)
- Use all 5 valid guesses without finding the number

---

## Test Results Summary

### Overall Status: ✅ **ALL TESTS PASSING**

- **Total Tests**: 82
- **Passed**: 82
- **Failed**: 0
- **Execution Time**: ~5.67 seconds

---

## Test Coverage Breakdown

### 1. InputValidatorTests (15 tests)
Validates user input handling:
- ✅ Valid number parsing (1-10)
- ✅ Whitespace trimming
- ✅ Null/empty/whitespace rejection
- ✅ Non-numeric input rejection
- ✅ Out-of-range validation (0, negatives, >10)

### 2. DoofusCheckerTests (8 tests)
Tests doofus detection logic:
- ✅ Valid guesses within bounds
- ✅ Boundary conditions (at low/high bound)
- ✅ Detecting guesses below low bound
- ✅ Detecting guesses above high bound
- ✅ Detecting repeated last guess
- ✅ Detecting repeated older guesses

### 3. GuessProcessorTests (15 tests)
Tests core game logic:
- ✅ Correct guess handling
- ✅ Too low/too high with bound updates
- ✅ Doofus streak management
- ✅ Guess history tracking
- ✅ Boundary narrowing logic
- ✅ Doofus moves don't count toward valid guess limit

### 4. GameStateTests (11 tests)
Tests state management:
- ✅ Initialization with correct defaults
- ✅ Secret number generation (1-10 range)
- ✅ Reset functionality
- ✅ State property modifications

### 5. IntegrationTests (33 tests)
Tests full game scenarios:
- ✅ Complete game flows from requirements
- ✅ Input validation integration
- ✅ 5-guess limit enforcement
- ✅ Doofus kick scenarios (3 consecutive)
- ✅ Boundary narrowing throughout game
- ✅ Complex scenarios (e.g., 2 doofus → valid → 1 doofus = not kicked)
- ✅ Edge cases from CLAUDE.md requirements

---

## Key Test Scenarios Verified

### Input Validation
- ✅ "abc" → Error: "That's not a number!"
- ✅ "0" → Error: "Pick a number between 1 and 10!"
- ✅ "11" → Error: "Pick a number between 1 and 10!"
- ✅ "  5  " → Valid (whitespace trimmed)
- ✅ "" → Error: "Please enter something!"

### Doofus Detection
- ✅ Guessing 3 when you know it's > 5 → Doofus strike
- ✅ Guessing same number 3x in a row → Kicked
- ✅ Guessing 5, then 3 (when answer > 5), then 2 → Kicked (3 doofus in a row)
- ✅ 2 doofus moves, then 1 valid, then 1 doofus → NOT kicked (streak reset)

### Game Flow
- ✅ User guesses correct on first try
- ✅ User narrows down and wins
- ✅ User uses all 5 guesses without winning
- ✅ User wins on exactly 5th guess
- ✅ Doofus moves don't count toward guess limit

---

## Code Quality

### Architecture
- **Separation of Concerns**: Clear division between validation, game logic, and state management
- **Testability**: All components are easily testable with minimal dependencies
- **Maintainability**: Well-organized code structure with single-responsibility classes

### Test Quality
- **Comprehensive Coverage**: All critical paths and edge cases tested
- **Readable Tests**: Descriptive test names and clear scenarios
- **Integration Tests**: Full game flows validated end-to-end

---

## Conclusion

The test suite demonstrates **100% pass rate** with comprehensive coverage of:
- Input validation
- Game logic
- Doofus detection
- State management
- Integration scenarios

The codebase is **production-ready** with a robust test foundation ensuring all game rules are correctly implemented and validated.

