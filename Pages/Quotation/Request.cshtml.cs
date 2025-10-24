using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace InterportCargo.Pages.Quotation
{
    public class RequestModel : PageModel
    {
        [BindProperty]
        public QuotationRequestModel QuotationRequest { get; set; } = new();

        public IActionResult OnGet()
        {
            // TODO: Check if user is authenticated
            // For now, redirect to login page
            return RedirectToPage("/Account/Login");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Implement actual quotation request logic
            // For now, redirect to home page
            return RedirectToPage("/Index");
        }
    }

    public class QuotationRequestModel
    {
        [Required(ErrorMessage = "Source location is required")]
        public string Source { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination location is required")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Number of containers is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of containers must be at least 1")]
        public int NumberOfContainers { get; set; }

        [Required(ErrorMessage = "Nature of package is required")]
        public string NatureOfPackage { get; set; } = string.Empty;

        public bool IsImport { get; set; }
        public bool IsExport { get; set; }
        public bool IsPacking { get; set; }
        public bool IsUnpacking { get; set; }
        public bool IsQuarantineRequired { get; set; }

        public string? AdditionalRequirements { get; set; }
    }
}
