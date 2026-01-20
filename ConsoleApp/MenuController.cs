using BLL;
using DAL;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp;

public abstract class MenuController
{
    private static bool _gameMenuLoop = true;
    public static void MenuRunner(GameState gameState)
    {
        using var dbContext = GetDbContext();
        //dbContext.Database.EnsureDeleted();
        IRepository<GameState> configRepository = new ConfigRepositoryEF(dbContext)!;

        var gameConfiguration = new GameConfiguration(gameState);
        var gameController = new GameController(gameState, configRepository);
        
        var presets = new Presets(gameState);
        
        var returnBack = new MenuItem { Name = "Return Back", Hotkey = "rb" };
        var returnToMenu = new MenuItem { Name = "Return to Menu", Hotkey = "rm" };
        var exit = new MenuItem { Name = "Exit", Hotkey = "e" };
        
        var mainMenu = new Menu("Main Menu", EMenuLevel.Root);

        mainMenu.AddMenuItem("New Game", "n");
        mainMenu.AddMenuItem("Load Game", "l");
        mainMenu.AddMenuItem("Options", "o");
        mainMenu.Items.Add(exit);

        var optionsMenu = new Menu("Options Menu", EMenuLevel.First);

        optionsMenu.AddMenuItem($"Board", "b",    () => $"{gameState.BoardWidth}x{gameState.BoardHeight}");
        optionsMenu.AddMenuItem($"Winning size", "c", () => $"{gameState.CheckersWinningSize}");
        optionsMenu.AddMenuItem($"Board Shape", "s", () => $"{gameState.BoardShape}");
        optionsMenu.AddMenuItem("Save System", "sa", () => configRepository.GetType().Name == "ConfigRepositoryEF" ? "Entity Framework" : "JSON");
        optionsMenu.Items.AddRange(returnBack, exit);

        var gameModeMenu = new Menu("Game Mode", EMenuLevel.First);

        gameModeMenu.AddMenuItem("Human vs Human", "hh");
        gameModeMenu.AddMenuItem("Human vs Ai", "ha");
        gameModeMenu.AddMenuItem("Ai vs Ai", "aa");
        gameModeMenu.Items.AddRange(returnBack, exit);
        
        var humanVsHumanMenu = new Menu("Human vs Human", EMenuLevel.Deep);
        
        humanVsHumanMenu.AddMenuItem("Start Game", "s");
        humanVsHumanMenu.AddMenuItem("Preset Games", "p");
        humanVsHumanMenu.Items.AddRange( returnBack, returnToMenu, exit);
        
        var humanVsAiMenu = new Menu("Human vs Ai", EMenuLevel.Deep);
        
        humanVsAiMenu.AddMenuItem("Start Game", "sg");
        humanVsAiMenu.AddMenuItem("Preset Games", "pg");
        humanVsAiMenu.Items.AddRange(returnBack, returnToMenu, exit);
        
        var presetsMenu = new Menu("Choose Preset Game", EMenuLevel.Deep);
        
        presetsMenu.AddMenuItem("Connect 4 rectangle", "c");
        presetsMenu.AddMenuItem("Connect 5 rectangle", "cr");
        presetsMenu.AddMenuItem("Connect 4 Cylinder", "cy");
        presetsMenu.Items.AddRange(returnBack, returnToMenu, exit);
        
        var aiDifficulty = new Menu("AI Difficulty", EMenuLevel.Deep);
        
        aiDifficulty.AddMenuItem("Easy", "ea");
        aiDifficulty.AddMenuItem("Medium", "m");
        aiDifficulty.AddMenuItem("Hard", "h");
        aiDifficulty.Items.AddRange(returnBack, returnToMenu, exit);
        
        var menuStack = new Stack<Menu>();
        menuStack.Push(mainMenu);

        while (_gameMenuLoop)
        {
            var currentMenu = menuStack.Peek();
            var userChoice = currentMenu.Run().ToLower();

            switch (userChoice)
            {
                case "n" when currentMenu.Level == EMenuLevel.Root:
                    menuStack.Push(gameModeMenu);
                    break;
                case "l" when currentMenu.Level == EMenuLevel.Root:
                    _gameMenuLoop = false;
                    SaveFiles(gameController, configRepository);
                    break;
                case "o" when currentMenu.Level == EMenuLevel.Root:
                    menuStack.Push(optionsMenu);
                    break;
                case "b" when currentMenu.Title == "Options Menu":
                    _gameMenuLoop = gameConfiguration.BoardConfiguration();
                    break;
                case "c" when currentMenu.Title == "Options Menu":
                    _gameMenuLoop = gameConfiguration.CheckersWinningSizeConfiguration();
                    break;
                case "s" when currentMenu.Title == "Options Menu":
                    _gameMenuLoop = gameConfiguration.BoardShapeConfiguration();
                    break;
                case "sa" when currentMenu.Title == "Options Menu":
                    configRepository = ConfigRepositoryChoice(dbContext);
                    gameController.SetConfigRepository(configRepository);
                    break;
                case "hh" when currentMenu.Title == "Game Mode":
                    gameState.GameMode = "HumanVsHuman";
                    menuStack.Push(humanVsHumanMenu);
                    break;
                case "s" when currentMenu.Title == "Human vs Human":
                    ExecuteGame(gameController, menuStack);
                    break;
                case "p" when currentMenu.Title == "Human vs Human":
                    menuStack.Push(presetsMenu);
                    break;
                case "c" when currentMenu.Title == "Choose Preset Game":
                    presets.Connect4Preset();
                    ExecuteGame(gameController, menuStack);
                    break;
                case "cr" when currentMenu.Title == "Choose Preset Game":
                    presets.Connect5Preset();
                    ExecuteGame(gameController, menuStack);
                    break;
                case "cy" when currentMenu.Title == "Choose Preset Game":
                    presets.CylinderPreset();
                    ExecuteGame(gameController, menuStack);
                    break;
                case "ha" when currentMenu.Title == "Game Mode":
                    gameState.GameMode = "HumanVsAi";
                    menuStack.Push(aiDifficulty);
                    break;
                case "ea" when currentMenu.Title == "AI Difficulty":
                    gameState.AiDifficulty = "easy";
                    menuStack.Push(humanVsAiMenu);
                    break;
                case "m" when currentMenu.Title == "AI Difficulty":
                    gameState.AiDifficulty = "medium";
                    menuStack.Push(humanVsAiMenu);
                    break;
                case "h" when currentMenu.Title == "AI Difficulty":
                    gameState.AiDifficulty = "hard";
                    menuStack.Push(humanVsAiMenu);
                    break;
                case "sg" when currentMenu.Title == "Human vs Ai":
                    ExecuteGame(gameController, menuStack);
                    break;
                case "pg" when currentMenu.Title == "Human vs Ai":
                    gameState.GameMode = "HumanVsAi";
                    menuStack.Push(presetsMenu);
                    break;
                case "aa" when currentMenu.Title == "Game Mode":
                    gameState.AiDifficulty = "hard";
                    gameState.GameMode = "AiVsAi";
                    ExecuteGame(gameController, menuStack);
                    break;
                case "rb" when currentMenu.Level is EMenuLevel.First or EMenuLevel.Deep:
                    menuStack.Pop();
                    break;
                case "rm" when currentMenu.Level == EMenuLevel.Deep:
                    while (menuStack.Count > 1) {
                        menuStack.Pop();
                    }
                    break;
                case "e":
                    Console.Clear();
                    _gameMenuLoop = false;
                    Console.WriteLine("Exiting...");
                    break;
            }
        }
    }

    private static void ExecuteGame(GameController gameController, Stack<Menu> menuStack) {
        _gameMenuLoop = false;
        Console.Clear();
        _gameMenuLoop = gameController.GameStart();

        if (!_gameMenuLoop) return;
        while (menuStack.Count > 1) {
            menuStack.Pop();
        }
    }

    private static IRepository<GameState> ConfigRepositoryChoice(AppDbContext dbContext)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Connect X!");  
            Console.WriteLine("--------------------------");
            Console.WriteLine("Select save system:");
            Console.WriteLine("1. JSON");
            Console.WriteLine("2. Entity Framework");
            Console.Write("Enter choice: ");

            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    return new ConfigRepositoryJson();
                case "2":
                    return new ConfigRepositoryEF(dbContext);
            }
        }
    }

    private static void SaveFiles(GameController gameController, IRepository<GameState> configRepository)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Load Game ===\n");
            
            var saves = configRepository.List();
            
            if (saves.Count == 0)
            {
                Console.WriteLine("No saved games found.");
                Console.Write("Press any key to continue...");
                Console.ReadLine();
                _gameMenuLoop = true;
                return;
            }
           
            DisplaySaveFiles(saves);
        
            Console.Write("\nEnter save id or rm (return to menu): ");
            var userInput = Console.ReadLine();
        
            if (userInput == "rm")
            {
                _gameMenuLoop = true;
                return;
            }
        
            if (!int.TryParse(userInput, out var index) || index < 1 || index > saves.Count)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }
        
            var selectedFile = saves[int.Parse(userInput) - 1];
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(selectedFile.id);
        
            Console.WriteLine($"\nYou selected: {fileNameWithoutExt}\n");

            Console.Write("Do you want to load or delete this file: ");
            var userAction = Console.ReadLine();

            if (userAction != null) ExecuteAction(userAction, configRepository, gameController, fileNameWithoutExt);
        }
    }
    
    private static void DisplaySaveFiles(List<(string id, string description)> saves)
    {
        for (var i = 1; i < saves.Count + 1; i++)
        {
            Console.WriteLine($"{i}. {saves[i - 1]}");   
        }
    }

    private static bool ConfirmAction(string message)
    {
        while (true)
        {
            Console.Clear();
            Console.Write($"{message}? (y/n): ");
            var input = Console.ReadLine()?.Trim().ToLower();
            switch (input)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    break;
            }
        }
    }

    private static void ExecuteAction(string userAction,IRepository<GameState> configRepositoryJson, GameController gameController, string fileNameWithoutExt)
    {
        switch (userAction) {
            case "load":
                if (ConfirmAction("Are you sure you want to load this game")) {
                    LoadGame(gameController, configRepositoryJson, fileNameWithoutExt);
                }
                break;
            
            case "delete":
                DeleteGame(gameController, configRepositoryJson, fileNameWithoutExt);
                break;
        }
    }
    
    private static void LoadGame(GameController gameController, IRepository<GameState> configRepositoryJson,
        string fileNameWithoutExt)
    {
        var loadedState = configRepositoryJson.Load(fileNameWithoutExt);
        gameController.SetGameState(loadedState);
        gameController.GameStart();
    }
    
    private static void DeleteGame(GameController gameController, IRepository<GameState> configRepositoryJson, string fileNameWithoutExt) {
        configRepositoryJson.Delete(fileNameWithoutExt);
        SaveFiles(gameController, configRepositoryJson);
    }


    private static AppDbContext GetDbContext() {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        homeDirectory += Path.DirectorySeparatorChar;
    
        var connectionString = $"Data Source={homeDirectory}app.db";

        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            //.LogTo(Console.WriteLine)
            .Options;
    
        var dbContext = new AppDbContext(contextOptions);
    
        dbContext.Database.Migrate();
    
        return dbContext;
    }
}
