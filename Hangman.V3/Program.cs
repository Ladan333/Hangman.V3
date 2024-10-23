using System.Text.RegularExpressions;

namespace Hangman.V3;

internal class Program
{
    #region Main
    static int theme = 0;
    static void Main(string[] args)
    {
        GameInfo gameInfo = new GameInfo();
        bool appRunning = true;
        do
        {
            try
            {
                while (!MainMenu()) ;
                GameInfo info = GameSetup(gameInfo);
                do
                {
                    Guessing(info);
                    GameOverCheck(info);
                } while (!info.GameOver);
                GameIsOver(info);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"Something went wrong: {e.Message}\n\n" +
                    $"Press any button to return to main menu.");
                Console.ReadKey();
            }
        } while (appRunning);

    }
    #endregion

    #region Main menu
    static bool MainMenu()
    {
        Console.Clear();
        Graphic.MenuLineUpper(theme, 0, -10);
        Graphic.WriteTitle(theme, 0, -8);
        Graphic.MenuOptions(theme, 0, 0);
        Graphic.MenuLineLower(theme, 0, 5);
        return MenuButtons(theme);
    }

    static bool MenuButtons(int theme)
    {
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1: return true;
            case ConsoleKey.D2: ThemeSelect(); return false;
            case ConsoleKey.D3: ViewList(theme); return false;
            case ConsoleKey.Q: Environment.Exit(0); return false;
            default: return false;
        }
    }

    #endregion

    #region Word txt options
    static void ViewList(int theme)
    {
        string? filePath = null;
        bool inMenu = true;
        int position = 0;
        do
        {
            if (ValidateWordList())
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TextFiles\\WordList.txt");
            }
            List<string> wordList = new List<string>(File.ReadAllLines(filePath!));
            wordList.Sort();
            int maxViewable = wordList.Count;

            if (position + 5 > maxViewable)
                position = maxViewable - 5;
            if (position - 5 < 0)
                position = 0;
            Graphic.WordList(theme, wordList, position, maxViewable, 0, -7);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1: position += 5; break;
                case ConsoleKey.D2: position -= 5; break;
                case ConsoleKey.D3: AddWord(theme, wordList); break;
                case ConsoleKey.D4: RemoveWord(theme, wordList, position, maxViewable); break;
                case ConsoleKey.Q: inMenu = false; break;
                default: break;
            }
        } while (inMenu);
    }

    static void AddWord(int theme, List<string> wordList)
    {
        string? wordToAdd = null;
        bool inMenu = true;
        do
        {
            bool addingWord = true;
            do
            {
                Console.Clear();
                Graphic.AddWordGraphic(theme, 0, -2);
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 - 2);
                string input = Console.ReadLine()!;
                if (IsAlphabetical(input) && input.Length >= 3)
                {
                    addingWord = false;
                    wordToAdd = input;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"The word needs to contain only alphabetical letters and has to be 3 characters or longer\n\n" +
                        $"Press any key to try again.");
                    Console.ReadKey();
                }
            } while (addingWord);
            Console.Clear();
            Graphic.AddWordConfirmGraphic(theme, wordToAdd!);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1: UpdateAddWordList(theme, wordList, wordToAdd!); Graphic.WordAddedGraphic(theme, wordToAdd!); Console.ReadKey(); inMenu = false;  break;
                case ConsoleKey.D2: inMenu = false; break;
                default: break;
            }
        } while (inMenu);
    }

    static void RemoveWord(int theme, List<string> wordList, int position, int maxViewable)
    {
        bool inMenu = true;
        do
        {
            if (position + 5 > maxViewable)
                position = maxViewable - 5;
            if (position - 5 < 0)
                position = 0;
            Graphic.RemoveWordGraphic(theme, wordList, position, maxViewable, 0, -2);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1: position += 5; break;
                case ConsoleKey.D2: position -= 5; break;
                case ConsoleKey.D3: ConfirmRemoval(theme, wordList, wordList[position]); Console.ReadKey(); inMenu = false; break;
                case ConsoleKey.D4: ConfirmRemoval(theme, wordList, wordList[position+1]); Console.ReadKey(); inMenu = false; break;
                case ConsoleKey.D5: ConfirmRemoval(theme, wordList, wordList[position+2]); Console.ReadKey(); inMenu = false; break;
                case ConsoleKey.D6: ConfirmRemoval(theme, wordList, wordList[position+3]); Console.ReadKey(); inMenu = false; break;
                case ConsoleKey.D7: ConfirmRemoval(theme, wordList, wordList[position+4]); Console.ReadKey(); inMenu = false; break;
                case ConsoleKey.Q: inMenu = false; break;
                default: break;
            }
        } while (inMenu);
    }

    static void ConfirmRemoval(int theme, List<string> wordList, string input)
    {
        bool inMenu = false;
        do
        {
            Graphic.RemoveWordConfirmGraphic(theme, input, 0, 0);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1: UpdateRemoveWordList(theme, wordList, input); inMenu = false;  break;
                case ConsoleKey.D2: inMenu = false; break;
                default: break;
            }
        } while (inMenu);
    }

    static void UpdateAddWordList(int theme, List<string> wordList, string input)
    {
        string? filePath = null;
        wordList.Add(input);
        wordList.Sort();
        if (ValidateWordList())
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TextFiles\\WordList.txt");
        }

        File.WriteAllLines(filePath!, wordList);
    }

    static void UpdateRemoveWordList(int theme, List<string> wordList, string input)
    {
        string? filePath = null;
        wordList.Remove(input);
        wordList.Sort();
        if (ValidateWordList())
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TextFiles\\WordList.txt");
        }

        File.WriteAllLines(filePath!, wordList);

        Graphic.WordRemovedGraphic(theme, input);
    }

    #endregion

    #region Theme
    static void ThemeSelect()
    {
        bool inMenu = true;
        do
        {
            Graphic.ThemeSelectGraphics(theme, 0, -4);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D0: theme = 0; break;
                case ConsoleKey.D1: theme = 1; break;
                case ConsoleKey.D2: theme = 2; break;
                case ConsoleKey.D3: theme = 3; break;
                case ConsoleKey.D4: theme = 4; break;
                case ConsoleKey.D5: theme = 5; break;
                case ConsoleKey.M: inMenu = false; break;
                default: break;
            }
        } while (inMenu);
    }
    #endregion

    #region Game setup
    static GameInfo GameSetup(GameInfo info)
    {
        info.GameOver = false;
        info.Win = false;
        info.Loss = false;
        info.Lives = 6;
        info.Wrongs = 0;
        info.GuessedLetters.Clear();
        NameRequest(info);
        Gamemode(info);
        return info;
    }
    static void Gamemode(GameInfo info)
    {
        bool pickingMode = true;
        do
        {
            Graphic.GamemodeGraphic(theme, -1, 0);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1: WordRequest(info); pickingMode = false; break;
                case ConsoleKey.D2: RandomWord(info); pickingMode = false; break;
                default: break;
            }
        } while (pickingMode);
    }
    static GameInfo NameRequest(GameInfo info)
    {
        bool validName = false;
        do
        {
            Graphic.NameRequestGraphic(theme);
            Console.SetCursorPosition(Console.WindowWidth / 2 +6, Console.WindowHeight / 2);
            string userInput = Console.ReadLine()!;
            if (!IsAlphabetical(userInput) || userInput.Length <= 2)
            {
                Graphic.InvalidNameGraphic(theme);
            }
            else
            {
                info.Name = userInput;
                validName = true;
            }
        } while (!validName);
        return info;
    }

    static GameInfo WordRequest(GameInfo info)
    {
        bool validWord = false;
        do
        {
            Graphic.WordSelectGraphic(theme);
            string userInput = Console.ReadLine()!;
            if (!IsAlphabetical(userInput) || userInput.Length <= 2)
            {
                Graphic.InvalidWordGraphic(theme);
            }
            else
            {
                info.Word = userInput;
                info.HiddenWord = userInput.ToUpper().ToCharArray();
                info.VisualWord = userInput.ToUpper().ToCharArray();
                for (int i = 0; i < info.VisualWord.Length; i++)
                {
                    info.VisualWord[i] = '_';
                }
                validWord = true;
            }
        } while (!validWord);

        return info;
    }

    static GameInfo RandomWord(GameInfo info)
    {
        string? filePath = null;
        if (ValidateWordList())
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TextFiles\\WordList.txt");
        }

        string[] wordList = File.ReadAllLines(filePath!);
        Random rng = new Random();
        int index = rng.Next(1, wordList.Length);
        info.Word = wordList[index];
        info.HiddenWord = info.Word.ToUpper().ToCharArray();
        info.VisualWord = info.Word.ToUpper().ToCharArray();
        for (int i = 0; i < info.VisualWord.Length; i++)
        {
            info.VisualWord[i] = '_';
        }
        return info;
    }
    #endregion

    #region Gameplay
    static void Guessing(GameInfo info)
    {
        bool validGuess = false;
        do
        {
            Graphic.GuessingGraphic(theme, info, -4, 0);
            char rawGuess = Console.ReadKey().KeyChar;
            string guess = rawGuess.ToString().ToUpper();
            if (!IsAlphabetical(guess) || info.GuessedLetters.Contains(char.ToUpper(rawGuess)))
            {
                Graphic.InvalidGuessGraphic(theme);
            }
            else
            {
                validGuess = CorrectCheck(rawGuess, info);
            }
        } while (!validGuess); 
    }

    static bool CorrectCheck(char rawGuess, GameInfo info)
    {
        char guess = char.ToUpper(rawGuess);
        bool validGuess = false;
        for (int i = 0; i < info.HiddenWord!.Length; i++)
        {
            if (info.HiddenWord[i] == guess && !info.GuessedLetters.Contains(guess))
            {
                info.GuessedLetters.Add(guess);
                info.VisualWord![i] = guess;
                info.HiddenWord[i] = '_';
                validGuess = true;
            }
            else if (info.HiddenWord[i] == guess && info.GuessedLetters.Contains(guess))
            {
                info.VisualWord![i] = guess;
                info.HiddenWord[i] = '_';
            }
            else if (!info.HiddenWord.Contains(guess) && !info.GuessedLetters.Contains(guess))
            {
                info.GuessedLetters.Add(guess);
                info.Lives--;
                info.Wrongs++;
                validGuess = true;
            }
        }
        return validGuess;
    }

    static void GameOverCheck(GameInfo info)
    {
        if (info.Lives == 0)
        {
            info.GameOver = true;
            info.Loss = true; 
        }
        if (!info.VisualWord!.Contains('_'))
        {
            info.GameOver = true;
            info.Win = true;
        }
    }

    static void GameIsOver(GameInfo info)
    {
        if (info.Win)
        {
            Graphic.WinGraphic(theme, info, 0, 0);
        }
        if (info.Loss)
        {
            Graphic.LossGraphic(theme, info, 0, 0);
        }
    }
    #endregion

    #region Utility
    private static bool IsAlphabetical(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-ZåöäÅÖÄ]+$");
    }

    static bool ValidateWordList()
    {
        if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TextFiles\\WordList.txt")))
            throw new Exception(@"Could not find the file 'WordList.txt'");
        else
            return true;
    }
    #endregion
}
