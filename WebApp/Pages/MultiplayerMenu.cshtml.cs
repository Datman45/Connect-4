using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class MultiplayerMenu(IRepository<GameState> configRepository) : PageModel
{
    public Task<IActionResult> OnPostCreateGame() {
        var game = new GameState
        {
            BoardHeight = HttpContext.Session.GetInt32("BoardHeight") ?? 6,
            BoardWidth = HttpContext.Session.GetInt32("BoardWidth") ?? 6,
            CheckersWinningSize = HttpContext.Session.GetInt32("CheckersWinningSize") ?? 4,
            BoardShape = HttpContext.Session.GetString("BoardShape") ?? "rectangle",
            GameMode = "Multiplayer",
        };
        
        var playerId = Guid.NewGuid();

        game.Player1Id = playerId;

        var jsonPath = configRepository.Save(game);
        
        HttpContext.Session.SetString("Player1Id", playerId.ToString());
        
        if (!Path.GetFileName(jsonPath).Contains(".json"))
            return Task.FromResult<IActionResult>(RedirectToPage("./MultiplayerGameplay", new { id = game.Id }));
        
        var fileName = Path.GetFileName(jsonPath); 
        return Task.FromResult<IActionResult>(RedirectToPage("./MultiplayerGameplay", new { id = fileName.Replace(".json", "") }));
        
    }
}
