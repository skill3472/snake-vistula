using System.Numerics;

class Program {

    // Game settings
    public const int GAME_TICK = 200;
    public const int sizeX = 32;
    public const int sizeY = 24;
    public const bool debugMode = true;

    public static int[] snakeHeadPosition = {sizeX/2, sizeY/2};
    public static int[] snakeHeadDirection = {0, 1};

    public static bool playing = true;

    public static void Main(string[] args)
    {
        int[,] GameState = new int[sizeY, sizeX];

        while(playing)
        {
            DrawGame(GameState);
            GameState = TickUpdate(GameState);
            Thread.Sleep(GAME_TICK);
        }
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
        switch (state)
        {
            case -1: // Border
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                return;
            case 0: // Empty space
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            case 1: // Snake
                Console.BackgroundColor = ConsoleColor.Green;
                return;
            case 2: // Apple
                Console.BackgroundColor = ConsoleColor.Red;
                return;
            default: // If undefined, return empty space
                Console.BackgroundColor = ConsoleColor.Black;
                return;
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

    public static int[,] TickUpdate(int[,] GameState)
    {
        snakeHeadPosition[0] += snakeHeadDirection[0];
        snakeHeadPosition[1] += snakeHeadDirection[1];
        if(snakeHeadPosition[1] > GameState.GetLength(0)-1 || snakeHeadPosition[0] > GameState.GetLength(1)-1)
        {
            playing = false;
            return GameState;
        }
        if(snakeHeadPosition[0] < 0 || snakeHeadPosition[1] < 0)
        {
            playing = false;
            return GameState;
        }
        for(int x = 0; x < GameState.GetLength(1); x++)
        {
            for(int y = 0; y < GameState.GetLength(0); y++)
            {
                if(GameState[y, x] != 2)
                    GameState[y, x] = 0;
            }
        }
        GameState[snakeHeadPosition[1], snakeHeadPosition[0]] = 1;
        if(debugMode) Console.WriteLine($"Snake position: {snakeHeadPosition[0]}, {snakeHeadPosition[1]}");
        return GameState;
    }
}