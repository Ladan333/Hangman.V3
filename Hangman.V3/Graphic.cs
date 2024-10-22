using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hangman.V3;
internal class Graphic
{
    #region Themes
    static void ForegroundOne(int theme)
    {
        switch (theme)
        {
            case 0: Console.ForegroundColor = ConsoleColor.White; break;
            case 1: Console.ForegroundColor = ConsoleColor.Yellow; break;
            case 2: Console.ForegroundColor = ConsoleColor.Gray; break;
            case 3: Console.ForegroundColor = ConsoleColor.Green; break;
            case 4: Console.ForegroundColor = ConsoleColor.Blue; break;
            case 5: Console.ForegroundColor = ConsoleColor.Magenta; break;

        }
    }

    static void ForegroundTwo(int theme)
    {
        switch (theme)
        {
            case 0: Console.ForegroundColor = ConsoleColor.White; break;
            case 1: Console.ForegroundColor = ConsoleColor.Green; break;
            case 2: Console.ForegroundColor = ConsoleColor.Red; break;
            case 3: Console.ForegroundColor = ConsoleColor.Red; break;
            case 4: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
            case 5: Console.ForegroundColor = ConsoleColor.DarkRed; break;
        }
    }

    static void ForegroundThree(int theme)
    {
        switch (theme)
        {
            case 0: Console.ForegroundColor = ConsoleColor.White; break;
            case 1: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
            case 2: Console.ForegroundColor = ConsoleColor.DarkRed; break;
            case 3: Console.ForegroundColor = ConsoleColor.Blue; break;
            case 4: Console.ForegroundColor = ConsoleColor.Magenta; break;
            case 5: Console.ForegroundColor = ConsoleColor.Gray; break;
        }
    }

    static void ForegroundFour(int theme)
    {
        switch (theme)
        {
            case 0: Console.ForegroundColor = ConsoleColor.White; break;
            case 1: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
            case 2: Console.ForegroundColor = ConsoleColor.DarkGray; break;
            case 3: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
            case 4: Console.ForegroundColor = ConsoleColor.Cyan; break;
            case 5: Console.ForegroundColor = ConsoleColor.Red; break;
        }
    }

    public static void ThemeSelectGraphics(int theme, int y, int x)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-4);
        ForegroundTwo(theme);
        Center("Select a theme:", y, x-1);
        Center("[0] Default", y-1, x+1);
        Center("[1] Jungle", y-2, x+2);
        Center("[2] Crimson", y-1, x+3);
        Center("[3] Matrix", y-2, x+4);
        Center("[4] Nebula", y-2, x+5);
        Center("[5] Valentine", y, x+6);
        Center("[M] Main menu", y, x+7);
        MenuLineLower(theme, y, x+9);
    }

    #endregion

    #region Main Menu
    public static void MenuLineLower(int theme, int x, int y)
    {
        ForegroundOne(theme);
        Center(@"\_________________________________________________________/", x, y);
    }
    public static void MenuLineUpper(int theme, int x, int y)
    {
        ForegroundOne(theme);
        Center(@" _________________________________________________________ ", x, y);
        Center(@"/                                                         \", x, y+1);
    }

    public static void WriteTitle(int theme, int x, int y)
    {
        ForegroundTwo(theme);
        Center(@"        ___  __ __  __ __                           ", x-2, y);
        Center(@"       / _ \/ //_/ / // ___ ____ ___ ___ _ ___ ____ ", x-2, y+1);
        Center(@"      / , _/ ,<   / _  / _ `/ _ / _ `/  ' / _ `/ _ \", x-2, y+2);
        Center(@"     /_/|_/_/|_| /_//_/\_,_/_//_\_, /_/_/_\_,_/_//_/", x-2, y+3);
        Center(@"                               /___/                ", x-2, y+4);
    }

    public static void MenuOptions(int theme, int x, int y)
    {
        ForegroundThree(theme);
        Center(@"[1] Play.", x-8, y);
        Center(@"[2] Themes.", x-7, y+1);
        Center(@"[3] Word list options.", x-2, y+2);
        Center(@"[Q] Quit.", x-8, y+3);
    }

    #endregion

    #region Game setup
    public static void NameRequestGraphic(int theme)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        ForegroundTwo(theme);
        Center(@" Please enter your name:", -7, 0);
        MenuLineLower(theme, 0, 4);
    }

    public static void InvalidNameGraphic(int theme)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        ForegroundTwo(theme);
        Center(@"Name can only be letters and more than 1 character long.", 0, 0);
        MenuLineLower(theme, 0, 4);
        Thread.Sleep(2500);
    }

    public static void WordSelectGraphic(int theme)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        ForegroundTwo(theme);
        Center(@"Please enter a word", 0, 0);
        Center(@"that will be used for the game:", 0 ,1);
        MenuLineLower(theme, 0, 5);
    }

    public static void InvalidWordGraphic(int theme)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        Center(@"Please input a valid word.", 0, 0);
        MenuLineLower(theme, 0, 4);
        Thread.Sleep(2500);
    }
    public static void GamemodeGraphic(int theme, int x, int y)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-3);
        ForegroundTwo(theme);
        Center("Choose a gamemode:", x, y);
        Center("[1] Pick a word", x, y+1);
        Center("[2] random word", x, y+2);
        MenuLineLower(theme, y, x+6);
        Thread.Sleep(2500);
    }

    #endregion

    #region Guessing
    public static void GuessingGraphic(int theme, GameInfo info, int x, int y)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-6);
        HealthBar(info, theme, y, x+4);
        ForegroundTwo(theme);
        StringBuilder visualString = new StringBuilder();
        foreach (char letter in info.VisualWord!)
        {
              visualString.Append($"{letter} ");
        }
        Center(visualString.ToString(), y, x+3);
        ForegroundThree(theme);
        Center($"Previous guesses:", y-5, x+5);
        ForegroundFour(theme);
        StringBuilder guessedString = new StringBuilder();
        foreach (char letter in info.GuessedLetters!)
            { guessedString.Append($"{letter} "); }
        Center(guessedString.ToString(), y+(info.GuessedLetters.Count + 4), x+5);
        ForegroundTwo(theme);
        Center(@"Guess a letter", y, x+7);
        MenuLineLower(theme, y, x+10);
    }

    public static void InvalidGuessGraphic(int theme)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        ForegroundThree(theme);
        Center(@"Please guess a new and valid letter", 0, 0);
        MenuLineLower(theme, 0, 4);
        Thread.Sleep(2500);
    }
    #endregion

    #region GameOver
    public static void WinGraphic(int theme, GameInfo info, int x, int y)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-7);
        ForegroundTwo(theme);
        Center(@"/¨¨\/¨¨\", y, x-5);
        Center(@"\      /", y ,x-4);
        Center(@" \    /", y, x-3);
        Center(@"  \__/  ", y, x-2);
        ForegroundTwo(theme);
        Center($"The word was: {info.Word}", x, y);
        Center("Congratulations, you won!", x ,y+2);
        Center("Press any key to go back to the main menu", x, y+4);
        MenuLineLower(theme, y, x+6);
        Console.ReadKey();
    }

    public static void LossGraphic(int theme, GameInfo info, int x, int y)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-7);
        ForegroundThree(theme);
        Center(@"/¨¨\/¨¨\", y, x-5);
        Center(@"\  /   /", y, x-4);
        Center(@" \ \  / ", y, x-3);
        Center(@"  \/_/  ", y, x-2);
        ForegroundTwo(theme);
        Center($"The word was: {info.Word}", y, x);
        Center("Sorry, you lost!", y, x+2);
        Center("Press any key to go back to the main menu", y, x+4);
        MenuLineLower(theme, y, x+6);
        Console.ReadKey();
    }

    #endregion

    #region Healthbars

    public static void HealthBar(GameInfo info, int theme, int y, int x)
    {
        switch (theme)
        {
            case 1: JungleHealthbar(theme, info, y, x); break;
            case 2: CrimsonHealthbar(theme, info, y, x); break;
            case 3: MatrixHealthBar(theme, info, y, x); break;
            case 4: NebulaHealthbar(theme, info, y, x); break;
            case 5: DefaultHealthBar(theme, info, y, x); break;
            default: DefaultHealthBar(theme, info, y, x); break;
        }
    }
    public static void DefaultHealthBar(int theme, GameInfo info, int y, int x)
    {
        int writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"/¨¨\/¨¨\" + " ", y-22 + writtenCount * 9, x-7);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"/¨¨\/¨¨\" + " ", y-22 + writtenCount * 9, x-7);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"\      /" + " ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"\  /   /" + " ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@" \    /" + "  ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@" \ \  / " + " ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"  \__/  " + " ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"  \/_/  " + " ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
    }

    public static void MatrixHealthBar(int theme, GameInfo info, int y, int x)
    {
        int writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"    ___ ", y - 22 + writtenCount * 9, x - 7 );
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"    ___ ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"   /  / ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"   /  / ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"  /--/  ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"  /--/  ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@" /__/   ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@" /__/   ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
    }

    public static void CrimsonHealthbar(int theme, GameInfo info, int y, int x)
    {
        int writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@" _| |_   ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"  | |    ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"|__ __|  ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@" _| |_   ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"  | |    ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"|_   _|  ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"  |_|    ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"  |_|    ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
    }

    public static void NebulaHealthbar(int theme, GameInfo info, int y, int x)
    {
        int writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"   /\    ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"        ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"__/  \__ ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"  *      ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"\_    _/ ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"     *   ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"  \/\/   ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"   *     ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
    }

    public static void JungleHealthbar(int theme, GameInfo info, int y, int x)
    {
        int writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@" __  __  ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"         ", y - 22 + writtenCount * 9, x - 7);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@"(  )(  ) ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@"         ", y - 22 + writtenCount * 9, x - 6);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@" ||  ||  ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@" __  __  ", y - 22 + writtenCount * 9, x - 5);
            writtenCount++;
        }
        writtenCount = 0;
        for (int i = info.Lives; i > 0; i--)
        {
            ForegroundTwo(theme);
            Thread.Sleep(20);
            Center(@" ||  ||  ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
        for (int i = info.Wrongs; i > 0; i--)
        {
            ForegroundThree(theme);
            Thread.Sleep(20);
            Center(@" ||  ||  ", y - 22 + writtenCount * 9, x - 4);
            writtenCount++;
        }
    }

    #endregion

    #region WordList

    public static void WordList(int theme, List<string> wordList, int position, int maxViewable, int y, int x)
    {
            Console.Clear();
            MenuLineUpper(theme, y, x-4);
            ForegroundThree(theme);
            Center($"Total words in list: {maxViewable}", y, x);
            ForegroundTwo(theme);
        int writtenCount = 0;
            for (int i = position; i <= position + 4; i++)
            {
                if (i <= maxViewable && i >= 0)
            {
                if (i < 9)
                {
                    Center($"[0{i + 1}] {wordList[i]}", y - 5 + wordList[i].Length / 2, x + 2 + writtenCount);
                    writtenCount++;
                }
                else
                {
                    Center($"[{i + 1}] {wordList[i]}", y - 5 + wordList[i].Length / 2, x + 2 + writtenCount);
                    writtenCount++;
                }
            }
            }
            ForegroundThree(theme);
            Center("[1] Next page", y-1, x + 9);
            Center("[2] Previous page", y+1, x + 10);
            Center("[3] Add a word", y-1, x + 11);
            Center("[4] Remove a word", y+1, x+12);
            Center("[Q] Back", y-4, x+13);
            MenuLineLower(theme, y, x+15);
    }

    public static void WordListOptionsMenu(int theme, int y, int x)
    {
        Console.Clear();
        Graphic.MenuLineUpper(theme, y, x-4);
        ForegroundThree(theme);
        Center($"[1] View all words", y, x);
        Center($"[2] Exit", y-3, x+1);
        Graphic.MenuLineLower(theme, y, x+4);
    }

    public static void AddWordGraphic(int theme, int y, int x)
    {
        MenuLineUpper(theme, y, x-4);
        ForegroundThree(theme);
        Center("Please enter a word you want to add:", y-8, x);
        MenuLineLower(theme, y, x+4);
    }

    public static void AddWordConfirmGraphic(int theme, string input)
    {
        MenuLineUpper(theme, 0, -4);
        ForegroundThree(theme);
        Center($"Are you sure you want to add {input}?", 0, -1);
        Center($"[1] Yes.", 0, 2);
        Center($"[2] No", -1, 3);
        MenuLineLower(theme, 0, 5);
    }

    public static void WordAddedGraphic(int theme, string input)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        ForegroundThree(theme);
        Center($"{input} was added!", 0, -1);
        Center("Press any key to continue", 0, 1);
        MenuLineLower(theme, 0, 4);
    }

    public static void RemoveWordGraphic(int theme, List<string> wordList, int position, int maxViewable, int y, int x)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-8);
        ForegroundThree(theme);
        Center($"Total words in list: {maxViewable}", y, x-4);
        Center($"Select the number of the word you want to remove", y, x-2);
        ForegroundTwo(theme);
        int listNumber = 3;
        int writtenCount = 0;
            for (int i = position; i <= position + 4; i++)
            {
                if (i <= maxViewable && i >= 0)
            {
                Center($"[0{listNumber}] {wordList[i]}", y - 5 + wordList[i].Length / 2, x + 1 + writtenCount);
                writtenCount++;
                listNumber++;
            }
            }
        ForegroundThree(theme);
        Center($"[1] Next page", y-1, x+7);
        Center($"[2] Previous page", y+1 ,x+8);
        Center($"[Q] Back", y-4 ,x+9);
        MenuLineLower(theme, y, x+11);
    }

    public static void RemoveWordConfirmGraphic(int theme, string input, int y, int x)
    {
        Console.Clear();
        MenuLineUpper(theme, y, x-4);
        ForegroundThree(theme);
            Center($"Are you sure you want to remove {input}?", y, x);
            Center($"[1] Yes.", y, x+2);
            Center($"[2] No", y-1, x+3);
        MenuLineLower(theme, y, x+5);
    }

    public static void WordRemovedGraphic(int theme, string input)
    {
        Console.Clear();
        MenuLineUpper(theme, 0, -4);
        ForegroundThree(theme);
        Center($"{input} was removed!", 0, 0);
        Center("Press any key to continue", 0, 1);
        MenuLineLower(theme, 0, 4);
    }

    #endregion

    #region Utility
    
    public static void Center(string text, int x, int y)
    {
        int windowWidth = Console.WindowWidth;
        int windowHeight = Console.WindowHeight;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;

        Console.SetCursorPosition(spaces + x, (windowHeight / 2) + y);
        Console.WriteLine(text);
    }

    #endregion
}