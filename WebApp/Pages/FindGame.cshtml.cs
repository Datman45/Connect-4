using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class FindGame(IRepository<GameState> configRepository) : PageModel
{
    [BindProperty]
    public string GameId { get; set; } = null!;
    public SelectList ListOfGames { get; set; } = null!;
    public async Task OnGetAsync()
    {
        await LoadDataAsync();
    }
    
    private async Task LoadDataAsync(){
        var data = await configRepository.ListAsync();
        var data2 = data.Select(i => new
        {
            i.id,
            value = i.description
        }).ToList();
        
        ListOfGames = new SelectList(data2, "id", "value");
    }

    public async Task<IActionResult> OnPostConnectAsync(){
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }
        
        if (!string.IsNullOrEmpty(GameId) && GameId.Contains(".json")) {
            GameId = GameId.Replace(".json", "");
        }

        var game = configRepository.Load(GameId);

        if (game.Player2Id == null)
        {
            var playerId = Guid.NewGuid();
            game.Player2Id = playerId;
            configRepository.Save(game);
            HttpContext.Session.SetString("Player2Id", playerId.ToString());
        }
        
        return RedirectToPage("./MultiplayerGameplay", new {id = GameId});
    }
}
