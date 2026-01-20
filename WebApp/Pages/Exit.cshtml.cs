using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Exit : PageModel
{
    private readonly IHostApplicationLifetime _life;

    public Exit(IHostApplicationLifetime life)
    {
        _life = life;
    }

    public IActionResult OnGet()
    {
        _life.StopApplication();
        return new EmptyResult();
    }
}
