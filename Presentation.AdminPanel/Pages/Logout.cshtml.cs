using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    public async Task OnGet()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        Response.Redirect("/");
    }
}
