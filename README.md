# Connect-4


## How to install

- Clone or download the project.
- Open the solution in Visual Studio (or another C# IDE).
- Set ConsoleApp as the startup project.
- Build and run the solution.


## How to play (Console App)

### Launch game
- Run console ConsoleApp
- The Main Menu will appear.
- Navigate the menu by typing the shown keyboard hotkeys.

### Starting a new game
- In main menu press "n" and enter to start a game
- Choose one of 3 game modes
  - Human vs Human
  - Human vs Ai
  - AI vs AI
- Depending on the selected game mode
  - You may start the game directly
  - You may choose precreated game

### AI difficulties
- AI has 3 difficulties
  - Easy: AI make a random move
  - Medium: AI may make a winning move, block his opponent`s winning move and make a random move
  - Hard: AI uses minimax algorithm with alpha-beta pruning

### Playing the game
- Choose your preferable gamemode
- The board will appear
- You have 3 options: enter your move, save or rm (return to menu)
- To make a move you need to write a column where you want to put your piece
- After you make a move
  - The second player takes a turn
  - The AI make automatic move

### Game End
- Game can end when
  - One of the player or AI win
  - The game ends in a draw
- After win or draw
  - You may return to the main menu by pressing "y"
  - Quit the console app by pressing "n"

### Additional information
- In options menu 
  - you may customize your game
  - You may change the save system:
    - JSON - saves games to files
    - Entity Framework - saves games to the database
- In load game menu
  - you may load or delete game (Available saves depend on the selected save system)


## How to play (Web App)

### Launch game
- Run the WebApp and open it in your browser (localhost)
- The Main Menu will appear.
- Navigate the menu by clicking left click on the mouse.

### Starting a new game
- In main menu click on new game
- Choose one of 3 game modes
  - Player vs Player
  - Player vs Bot
  - Bot vs Bot

### Ai difficulties
- AI has 3 difficulties
  - Easy: AI make a random move
  - Medium: AI may make a winning move, block his opponent`s winning move and make a random move
  - Hard: AI uses minimax alghorithm with alpha-beta pruning

### Playing the game
- Choose your preferable gamemode
- The board will appear
- You have 3 options
  - Make a move
  - Return to menu
  - Save game
- To make a move you may click on the column
- After you make a move
  - The second player takes a turn
  - The AI make automatic move

### Game End
- Game can end when
  - One of the player or AI win
- After win or draw
  - You may return to the main menu by clicking on the return to menu button

### Multiplayer
- First player create a game and waiting for other player
- Second player join the game by choosing available in find game
- How to play
  - First player make a move
  - Second player refresh a page
  - Second Player make a move
  - First player refresh a page

### Additional information
- In options menu 
  - you may customize your game
- In load game menu
  - you may load or delete game (Available saves depend on the selected save system)
