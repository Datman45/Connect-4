using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class GameMode : PageModel
{
    
    public RedirectToPageResult OnGetHumanVsHuman()
    {
        HttpContext.Session.SetString("GameMode", "HumanVsHuman");
        return RedirectToPage("./GamePlay");
    }
    public RedirectToPageResult OnGetAiVsAi()
    {
        HttpContext.Session.SetString("GameMode", "AiVsAi");
        return RedirectToPage("./GamePlay");
    }
}
