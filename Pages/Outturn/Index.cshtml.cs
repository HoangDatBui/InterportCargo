using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Pages.Outturn
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
