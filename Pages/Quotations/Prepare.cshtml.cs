using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;
using InterportCargo.BusinessLogic.Interfaces;
using System.Text.Json;
using System.Text;

namespace InterportCargo.Pages.Quotations
{
    public class PrepareModel : PageModel
    {
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private readonly IQuotationDetailsRepository _quotationDetailsRepository;
        private readonly IRateScheduleRepository _rateScheduleRepository;
        private readonly IDiscountService _discountService;
        private static int _quotationCounter = 1;

        public PrepareModel(
            IQuotationRequestRepository quotationRequestRepository,
            IQuotationDetailsRepository quotationDetailsRepository,
            IRateScheduleRepository rateScheduleRepository,
            IDiscountService discountService)
        {
            _quotationRequestRepository = quotationRequestRepository;
            _quotationDetailsRepository = quotationDetailsRepository;
            _rateScheduleRepository = rateScheduleRepository;
            _discountService = discountService;
        }

        [BindProperty]
        public int QuotationRequestId { get; set; }

        [BindProperty]
        public string QuotationNumber { get; set; } = string.Empty;

        [BindProperty]
        public DateTime DateIssued { get; set; }

        [BindProperty]
        public string ContainerType { get; set; } = string.Empty;

        [BindProperty]
        public string Scope { get; set; } = string.Empty;

        [BindProperty]
        public decimal CalculatedSubtotal { get; set; }

        [BindProperty]
        public decimal CalculatedDiscountPercentage { get; set; }

        [BindProperty]
        public decimal CalculatedDiscountAmount { get; set; }

        [BindProperty]
        public decimal CalculatedGST { get; set; }

        [BindProperty]
        public decimal CalculatedTotalAmount { get; set; }

        // Display properties
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public int EstimatedContainerCount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<string> AvailableContainerTypes { get; set; } = new() { "20 Feet Container", "40 Feet Container" };
        public string SuggestedScope { get; set; } = string.Empty;
        public QuotationRequest? QuotationRequest { get; set; }

        public IActionResult OnGet(int id)
        {
            // Check authentication
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Check if user is employee
            var userType = HttpContext.Session.GetString("UserType") ?? string.Empty;
            var isEmployee = !string.IsNullOrEmpty(userType) && userType != "Customer";

            if (!isEmployee)
            {
                return RedirectToPage("/Quotations/Index");
            }

            QuotationRequestId = id;
            
            // Get quotation request
            QuotationRequest = _quotationRequestRepository.GetById(id);
            
            if (QuotationRequest == null)
            {
                TempData["ErrorMessage"] = "Quotation request not found.";
                return RedirectToPage("/Quotations/Index");
            }

            // Check if quotation is already prepared
            if (_quotationDetailsRepository.GetByQuotationRequestId(id) != null)
            {
                TempData["ErrorMessage"] = "A quotation has already been prepared for this request.";
                return RedirectToPage("/Quotations/Details", new { id = id });
            }

            // Populate display properties
            CustomerName = QuotationRequest.CustomerName;
            CustomerEmail = QuotationRequest.CustomerEmail;
            EstimatedContainerCount = QuotationRequest.NumberOfContainers;
            
            // Generate quotation number
            QuotationNumber = $"QT{DateTime.Now:yyyyMMdd}{_quotationCounter++:D4}";
            DateIssued = DateTime.UtcNow;

            // Calculate discount
            DiscountPercentage = _discountService.CalculateDiscount(QuotationRequest);

            SuggestedScope = $"Transport {QuotationRequest.NatureOfPackage} from {QuotationRequest.Source} to {QuotationRequest.Destination}. " +
                           $"{QuotationRequest.ImportOrExport} with {QuotationRequest.PackingOrUnpacking} service.";

            return Page();
        }

        public IActionResult OnPost()
        {
            // Check authentication
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Validate
            if (!ModelState.IsValid)
            {
                // Reload data and return page
                QuotationRequest = _quotationRequestRepository.GetById(QuotationRequestId);
                if (QuotationRequest != null)
                {
                    CustomerName = QuotationRequest.CustomerName;
                    CustomerEmail = QuotationRequest.CustomerEmail;
                    EstimatedContainerCount = QuotationRequest.NumberOfContainers;
                    DiscountPercentage = _discountService.CalculateDiscount(QuotationRequest);
                    SuggestedScope = $"Transport {QuotationRequest.NatureOfPackage} from {QuotationRequest.Source} to {QuotationRequest.Destination}. " +
                                   $"{QuotationRequest.ImportOrExport} with {QuotationRequest.PackingOrUnpacking} service.";
                }
                return Page();
            }

            // Get quotation request
            QuotationRequest = _quotationRequestRepository.GetById(QuotationRequestId);
            if (QuotationRequest == null)
            {
                TempData["ErrorMessage"] = "Quotation request not found.";
                return RedirectToPage("/Quotations/Index");
            }

            // Get officer info
            var officerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var officerName = HttpContext.Session.GetString("UserName") ?? "Officer";

            // Create quotation details
            var quotationDetails = new QuotationDetails
            {
                QuotationNumber = QuotationNumber,
                QuotationRequestId = QuotationRequestId,
                OfficerId = officerId,
                OfficerName = officerName,
                DateIssued = DateIssued,
                ContainerType = ContainerType,
                Scope = Scope,
                Subtotal = CalculatedSubtotal,
                DiscountPercentage = CalculatedDiscountPercentage,
                DiscountAmount = CalculatedDiscountAmount,
                AmountAfterDiscount = CalculatedSubtotal - CalculatedDiscountAmount,
                GST = CalculatedGST,
                TotalAmount = CalculatedTotalAmount,
                Status = "Pending",
                CreatedDate = DateTime.UtcNow
            };

            _quotationDetailsRepository.Add(quotationDetails);

            TempData["SuccessMessage"] = "Quotation prepared successfully!";
            return RedirectToPage("/Quotations/Details", new { id = QuotationRequestId });
        }

        public IActionResult OnGetGetRates(int id, string containerType)
        {
            if (string.IsNullOrEmpty(containerType))
            {
                return new JsonResult(new { rates = new List<object>() });
            }

            var rates = _rateScheduleRepository.GetActiveRates()
                .Where(r => !string.Equals(r.ServiceType, "GST", StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    serviceType = r.ServiceType,
                    rate20Feet = r.Rate20Feet,
                    rate40Feet = r.Rate40Feet,
                    description = r.Description
                })
                .ToList();

            return new JsonResult(new { rates = rates });
        }
    }
}

