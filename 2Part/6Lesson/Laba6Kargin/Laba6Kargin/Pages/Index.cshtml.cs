using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    public string? Greeting { get; set; }

    public void OnPost()
    {
        if (!string.IsNullOrWhiteSpace(Username))
        {
            Greeting = $"Привет, {Username}!";
        }
    }
}