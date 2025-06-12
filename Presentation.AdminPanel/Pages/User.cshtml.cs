using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "User")]
public class UserModel : PageModel
{
    public void OnGet()
    {
    }
}
