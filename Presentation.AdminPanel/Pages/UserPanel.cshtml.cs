using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "User")]
public class UserPanelModel : PageModel
{
    public void OnGet()
    {
    }
}
