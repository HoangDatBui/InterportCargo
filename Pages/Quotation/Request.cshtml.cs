using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;

namespace InterportCargo.Pages.Quotation
{
    public class RequestModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private static int _requestCounter = 1;

        public RequestModel(ICustomerRepository customerRepository, IQuotationRequestRepository quotationRequestRepository)
        {
            _customerRepository = customerRepository;
            _quotationRequestRepository = quotationRequestRepository;
        }

        [BindProperty]
        public QuotationRequestViewModel QuotationRequest { get; set; } = new();

        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Get user information from session
            UserName = HttpContext.Session.GetString("UserName") ?? string.Empty;
            UserEmail = HttpContext.Session.GetString("UserEmail") ?? string.Empty;

            return Page();
        }

        public IActionResult OnPost()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                UserName = HttpContext.Session.GetString("UserName") ?? string.Empty;
                UserEmail = HttpContext.Session.GetString("UserEmail") ?? string.Empty;
                return Page();
            }

            // Get user information from session
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            UserName = HttpContext.Session.GetString("UserName") ?? string.Empty;
            UserEmail = HttpContext.Session.GetString("UserEmail") ?? string.Empty;

            // Generate unique Request ID
            var requestId = $"QR{DateTime.Now:yyyyMMdd}{_requestCounter++:D4}";

            // Create quotation request entity
            var quotationRequest = new QuotationRequest
            {
                RequestId = requestId,
                CustomerId = userId,
                CustomerName = UserName,
                CustomerEmail = UserEmail,
                Source = QuotationRequest.Source,
                Destination = QuotationRequest.Destination,
                NumberOfContainers = QuotationRequest.NumberOfContainers,
                NatureOfPackage = QuotationRequest.NatureOfPackage,
                PackageWidth = QuotationRequest.PackageWidth,
                PackageHeight = QuotationRequest.PackageHeight,
                PackageDepth = QuotationRequest.PackageDepth,
                ImportOrExport = QuotationRequest.ImportOrExport,
                PackingOrUnpacking = QuotationRequest.PackingOrUnpacking,
                IsQuarantineRequired = QuotationRequest.IsQuarantineRequired,
                QuarantineDetails = QuotationRequest.IsQuarantineRequired ? QuotationRequest.QuarantineDetails : null,
                IsFumigationRequired = QuotationRequest.IsFumigationRequired,
                FumigationDetails = QuotationRequest.IsFumigationRequired ? QuotationRequest.FumigationDetails : null,
                AdditionalRequirements = QuotationRequest.AdditionalRequirements,
                Status = "Pending",
                CreatedDate = DateTime.UtcNow
            };

            // Save to database
            _quotationRequestRepository.Add(quotationRequest);

            TempData["SuccessMessage"] = "Your quotation request has been submitted successfully!";
            return RedirectToPage("/Index");
        }
    }

    public class QuotationRequestViewModel
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

        [Required(ErrorMessage = "Package width is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Width must be greater than 0")]
        public decimal PackageWidth { get; set; }

        [Required(ErrorMessage = "Package height is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Height must be greater than 0")]
        public decimal PackageHeight { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Depth must be greater than 0")]
        public decimal? PackageDepth { get; set; }

        [Required(ErrorMessage = "Please select Import or Export")]
        public string ImportOrExport { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select Packing or Unpacking")]
        public string PackingOrUnpacking { get; set; } = string.Empty;

        public bool IsQuarantineRequired { get; set; }
        
        [StringLength(500, ErrorMessage = "Quarantine details cannot exceed 500 characters")]
        public string? QuarantineDetails { get; set; }

        public bool IsFumigationRequired { get; set; }
        
        [StringLength(500, ErrorMessage = "Fumigation details cannot exceed 500 characters")]
        public string? FumigationDetails { get; set; }

        [StringLength(1000, ErrorMessage = "Additional requirements cannot exceed 1000 characters")]
        public string? AdditionalRequirements { get; set; }
    }
}
