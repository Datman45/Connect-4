# Connect-4

## How to install

- Clone or download the project.
- Open the solution in Visual Studio (or another C# IDE).
- Set ConsoleApp as the startup project.
- Build and run the solution.

## How to play

1. Run console ConsoleApp
2. The Main Menu will appear.
3. Navigate the menu by typing the shown keyboard hotkeys.

## Starting a new game
1. In main menu press "n" and enter to start a game
2. Choose one of 3 gamemodes
- Human vs Human
- Human vs Ai
- Ai vs Ai
3. Depending on the selected game mode
- You may start the game directly
- You may choose precreated game

## Ai difficulties
1. Ai has 3 difficulties
- Easy: ai make a random move
- Medium: ai may make an winning move, block his opponent`s winning move and make a random move
- Hard: ai uses minimax alghorithm with alpha-beta pruning

## Playing the game
1. Choose your preferable gamemode
2. The board will appear
3. You have 3 options: enter your move, save or rm (return to menu)
4. To make a move you need to write a column where you want to put your piece
5. After you make a move
- The second player takes a turn
- The ai make automatic move

## Game End
1. Game can be end when
- One of the player or ai win
- The game ends in a draw
2. After win or draw
- You may return to the main menu by pressing "y"
- Yuit the console app by pressing "n"

## Additional information
1. In options menu 
- you may customize your game
- You may change the save system:
  - JSON - saves games to files
  - Entity Framework - saves games to the database
2. In load game menu
- you may load or delete game (Available saves depend on the selected save system)
