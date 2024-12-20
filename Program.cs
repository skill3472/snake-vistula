using System;
using System.IO;
using System.Threading;

class Program {

    public const int GAME_TICK = 200;
    public const int sizeX = 16;
    public const int sizeY = 12;
    public static int seed = 779136039;

    public const string FILE_PATH = "../../../MOVES";

    public static Random r = new Random();



    public static int[] ZERO_DIR = { 0, 0 }; // Would do readonly here, but I don't think it was in classes, so whatever
    public static int[] snakeHeadPosition = { sizeX / 2, sizeY / 2 };

    /* Directions in this implementation go as follows:
    /  (-1, 0) LEFT [L]
    /  (0, 1)  DOWN [D]
    /  (0, -1) UP [U]
    /  (1, 0)  RIGHT [R]
    /  (0, 0) NONE [W]
    */
    public static int[] snakeHeadDirection = { 1, 0 };

    public static int snakeLength = 3;

    public static bool playing = true;
    public static int ticksToWait = 0;

    public static int Main(string[] args)
    {
        int[,] GameState = new int[sizeY, sizeX];


        if(!File.Exists(FILE_PATH))
        {
            File.Create(FILE_PATH);
            return 0;
        }

        StreamReader sr = new StreamReader(FILE_PATH);
        

        int.TryParse(sr.ReadLine(), out seed);
        r = new Random(seed);

        PlaceApple(GameState);

        string? line;
        while(playing)
        {
            DrawGame(GameState);
            Thread.Sleep(GAME_TICK);
            if(ticksToWait == 0 && (line = sr.ReadLine()) != null)
                GameState = TickUpdate(GameState, line);
            else
                GameState = TickUpdate(GameState, "W");
        }
        if(snakeLength > 0)
            Console.WriteLine($"\nGG! Your score: {snakeLength}");
        else
            Console.WriteLine("\nIt seems you didn't provide any instructions? Check the README file for more info on that.");
        return 0;
    }

    public static void DrawGame(int[,] GameState)
    {
        Console.Clear();
        DrawHorizontalBorder(GameState.GetLength(1) + 2);
        Console.WriteLine();
        for(int y = 0; y < GameState.GetLength(0); y++)
        {
            SetColorByState(-1);
            Console.Write(" ");
            Console.ResetColor();
            for(int x = 0; x < GameState.GetLength(1); x++)
            {
                SetColorByState(GameState[y, x]);
                Console.Write(" ");
            }
            SetColorByState(-1);
            Console.Write(" ");
            Console.ResetColor();
            Console.WriteLine();
        }
        DrawHorizontalBorder(GameState.GetLength(1) + 2);
    }

    public static void SetColorByState(int state)
    {
        if (state == -1)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }
        else if (state == 0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }
        else if(state == 2)
        {
            Console.BackgroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Green;
        }
    }

    public static void DrawHorizontalBorder(int size)
    {
        SetColorByState(-1);
        for(int i = 0; i < size; i++)
        {
            Console.Write(" ");
        }
        Console.ResetColor();
    }

    public static int[,] PlaceApple(int[,] GameState)
    {
        GameState[r.Next(0, sizeY-1), r.Next(0, sizeX-1)] = 2;
        return GameState;
    }

    public static int[] InputToDirection(string input)
    {
        if(input == "U") 
        {
            return new int[] { 0, -1};
        } 
        else if(input == "D") 
        {
            return new int[] { 0, 1};
        } 
        else if(input == "L") 
        {
            return new int[] { -1, 0};
        } 
        else if(input == "R") 
        {
            return new int[] { 1, 0};
        } 
        else 
        {
            return ZERO_DIR;
        }
    }

    public static bool StringHasNumbers(string str)
    {
        foreach(char c in str)
        {
            if(char.IsDigit(c))
                return true;
        }
        return false;
    }

    public static int ExtractIntFromString(string str)
    {
        string numberStr = "";
        foreach(char c in str)
        {
            if(char.IsDigit(c))
                numberStr += c;
        }
        return int.Parse(numberStr);
    }

    public static int[,] TickUpdate(int[,] GameState, string input)
    {
        if(StringHasNumbers(input))
        {
            ticksToWait += ExtractIntFromString(input);
        }
        if(InputToDirection(input) != ZERO_DIR && ticksToWait == 0)
        {
            snakeHeadDirection = InputToDirection(input);
        }
        else
        {
            ticksToWait--;
        }
        snakeHeadPosition[0] += snakeHeadDirection[0];
        snakeHeadPosition[1] += snakeHeadDirection[1];
        if(snakeHeadPosition[1] > GameState.GetLength(0)-1 || snakeHeadPosition[0] > GameState.GetLength(1)-1 || snakeHeadPosition[1] < 0 || snakeHeadPosition[0] < 0)
        {
            playing = false;
            return GameState;
        }
        if(GameState[snakeHeadPosition[1], snakeHeadPosition[0]] >= 1000)
        {
            playing = false;
            return GameState;
        }
        for(int x = 0; x < GameState.GetLength(1); x++)
        {
            for(int y = 0; y < GameState.GetLength(0); y++)
            {
                if(GameState[y, x] > 1000)
                {
                    GameState[y, x]--;
                }
                else if(GameState[y, x] != 2)
                {
                    GameState[y, x] = 0;
                }
            }
        }
        if(GameState[snakeHeadPosition[1], snakeHeadPosition[0]] == 2)
        {
            snakeLength++;
            GameState = PlaceApple(GameState);
        }
        GameState[snakeHeadPosition[1], snakeHeadPosition[0]] = 1000 + snakeLength;
        return GameState;
    }
}