using System;

namespace BLL;

public class GameConfiguration(GameState gameState)
{

    public bool BoardConfiguration()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Board width: " + gameState.BoardWidth + " Board height: " + gameState.BoardHeight);
            Console.Write("Do you want to change board size? (y/n):");
            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "y":
                    BoardUpdate();
                    break;
                case "n":
                    Console.Clear();
                    return true;
                default:
                    UserInvalidInput(userInput!);
                    break;
            }
        }
    }

    private void BoardUpdate()
    {
        Console.Write("Please enter new board width: ");
        var userWidth = Console.ReadLine();
        if (string.IsNullOrEmpty(userWidth) || !int.TryParse(userWidth, out var width) || width < 4)
        {
            UserInvalidInput(userWidth!);
            return;
        }

        gameState.BoardWidth = int.Parse(userWidth);


        Console.Write("Please enter new board height: ");
        var userHeight = Console.ReadLine();
        if (string.IsNullOrEmpty(userHeight) || !int.TryParse(userHeight, out var height) || height < 4)
        {
            UserInvalidInput(userHeight!);
            return;
        }

        gameState.BoardHeight = int.Parse(userHeight);

        gameState.Resize();

        Console.Clear();
    }
    
    public bool CheckersWinningSizeConfiguration()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Checkers winning size: " + gameState.CheckersWinningSize);
            Console.Write("Do you want to change checkers winning size? (y/n):");
            var userInput = Console.ReadLine();
            switch (userInput)
            {
                case "y":
                    CheckersWinningSizeUpdate();
                    break;
                case "n":
                    Console.Clear();
                    return true;
                default:
                    UserInvalidInput(userInput!);
                    break;
            }
        }
    }

    private void CheckersWinningSizeUpdate()
    {
        Console.Write("Please enter new checkers winning size: ");
        var userInput = Console.ReadLine();
        if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out var size) || size < 2)
        {
            UserInvalidInput(userInput!);
            return;
        }
        
        gameState.CheckersWinningSize = int.Parse(userInput);
        Console.Clear();
    }

    public bool BoardShapeConfiguration()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Board shape is " + gameState.BoardShape + ". (There are only two shapes Rectangle and Cylinder." +
                              " If you input ‘y’, it switches to the other one.)");
            Console.Write("Do you want to change board shape? (y/n):");
            var userInput = Console.ReadLine();
            switch (userInput)
            {
                case "y":
                    gameState.BoardShape = gameState.BoardShape == "rectangle" ? "cylinder" : "rectangle";
                    break;
                case "n":
                    return true;
                default:
                    UserInvalidInput(userInput!);
                    break;
            }
        }
    }

    private static void UserInvalidInput(string userInput)
    {
        Console.WriteLine("\nInvalid input: " + userInput + ". Please enter a valid number.\n");
    }

    public override string ToString()
    {
        return $"BoardWidth: {gameState.BoardWidth}, BoardHeight: {gameState.BoardHeight}";
    }
}
