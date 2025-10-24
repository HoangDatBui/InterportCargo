using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Pages.Outturn
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // TODO: Check if user is authenticated
            // For now, redirect to login page
            return RedirectToPage("/Account/Login");
        }
    }
}
