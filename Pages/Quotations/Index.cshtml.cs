using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;

namespace InterportCargo.Pages.Quotations
{
    public class IndexModel : PageModel
    {
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private readonly IQuotationResponseRepository _quotationResponseRepository;
        private readonly IQuotationDetailsRepository _quotationDetailsRepository;

        public IndexModel(
            IQuotationRequestRepository quotationRequestRepository, 
            IQuotationResponseRepository quotationResponseRepository,
            IQuotationDetailsRepository quotationDetailsRepository)
        {
            _quotationRequestRepository = quotationRequestRepository;
            _quotationResponseRepository = quotationResponseRepository;
            _quotationDetailsRepository = quotationDetailsRepository;
        }

        public List<QuotationRequest> QuotationRequests { get; set; } = new();
        public bool IsEmployee { get; set; }
        public int UnreadNotificationCount { get; set; }
        public int UnreadOfficerNotificationCount { get; set; }
        public Dictionary<int, bool> HasQuotationDetails { get; set; } = new();

        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Check if user is employee (quotation officer)
            var userType = HttpContext.Session.GetString("UserType") ?? string.Empty;
            IsEmployee = !string.IsNullOrEmpty(userType) && userType != "Customer";

            if (IsEmployee)
            {
                // Get all quotations for quotation officers
                QuotationRequests = _quotationRequestRepository.GetAll();
                
                // Get unread notification count for officers (customer responses)
                var officerId = HttpContext.Session.GetInt32("UserId");
                UnreadOfficerNotificationCount = _quotationResponseRepository.GetUnreadNotificationsForOfficers(officerId).Count;
            }
            else
            {
                // Get only customer's quotations
                var customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
                if (customerId > 0)
                {
                    QuotationRequests = _quotationRequestRepository.GetByCustomerId(customerId);
                    
                    // Get unread notification count
                    UnreadNotificationCount = _quotationResponseRepository.GetUnreadNotificationsByCustomerId(customerId).Count;
                    
                    // Check which quotations have been prepared
                    foreach (var quotation in QuotationRequests)
                    {
                        HasQuotationDetails[quotation.Id] = _quotationDetailsRepository.GetByQuotationRequestId(quotation.Id) != null;
                    }
                }
            }

            return Page();
        }
    }
}
