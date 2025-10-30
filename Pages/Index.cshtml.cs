using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // If user is authenticated, redirect to Quotations page
            if (HttpContext.Session.GetString("IsAuthenticated") == "true")
            {
                return RedirectToPage("/Quotations/Index");
            }

            return Page();
        }
    }
}
