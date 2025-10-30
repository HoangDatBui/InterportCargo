using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;

namespace InterportCargo.Pages.Quotations
{
    /// <summary>
    /// Page model for viewing customer response notifications for quotation officers
    /// </summary>
    public class OfficerNotificationsModel : PageModel
    {
        private readonly IQuotationResponseRepository _quotationResponseRepository;
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private readonly IQuotationDetailsRepository _quotationDetailsRepository;

        /// <summary>
        /// Initializes a new instance of the OfficerNotificationsModel
        /// </summary>
        public OfficerNotificationsModel(
            IQuotationResponseRepository quotationResponseRepository,
            IQuotationRequestRepository quotationRequestRepository,
            IQuotationDetailsRepository quotationDetailsRepository)
        {
            _quotationResponseRepository = quotationResponseRepository;
            _quotationRequestRepository = quotationRequestRepository;
            _quotationDetailsRepository = quotationDetailsRepository;
        }

        public List<QuotationResponse> CustomerResponses { get; set; } = new();
        public int UnreadCount { get; set; }

        /// <summary>
        /// Handles GET requests to display customer responses
        /// </summary>
        public IActionResult OnGet()
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

            // Get officer ID
            var officerId = HttpContext.Session.GetInt32("UserId");

            // Get all customer responses (both read and unread) for this officer or all officers
            CustomerResponses = _quotationResponseRepository.GetUnreadNotificationsForOfficers(officerId).ToList();
            
            // Also include read responses from all customer responses
            var allCustomerResponses = _quotationResponseRepository.GetAll()
                .Where(qr => qr.ResponseType == "Customer")
                .OrderByDescending(qr => qr.CreatedDate)
                .ToList();
            
            // If officerId is provided, filter by quotations prepared by that officer
            if (officerId.HasValue)
            {
                var details = _quotationDetailsRepository.GetAll()
                    .Where(qd => qd.OfficerId == officerId.Value)
                    .ToList();
                
                var officerRequestIds = details.Select(d => d.QuotationRequestId).ToList();
                
                CustomerResponses = allCustomerResponses
                    .Where(cr => officerRequestIds.Contains(cr.QuotationRequestId))
                    .OrderByDescending(cr => cr.CreatedDate)
                    .ToList();
            }
            else
            {
                CustomerResponses = allCustomerResponses;
            }

            // Load quotation requests for display
            foreach (var response in CustomerResponses)
            {
                response.QuotationRequest = _quotationRequestRepository.GetById(response.QuotationRequestId);
                
                // Get quotation number from quotation details
                if (response.QuotationRequest != null)
                {
                    var details = _quotationDetailsRepository.GetByQuotationRequestId(response.QuotationRequestId);
                    if (details != null)
                    {
                        response.QuotationNumber = details.QuotationNumber;
                    }
                }
            }

            UnreadCount = CustomerResponses.Count(cr => !cr.IsRead);

            return Page();
        }

        /// <summary>
        /// Handles POST requests to mark notifications as read
        /// </summary>
        public IActionResult OnPostMarkAsRead(int id)
        {
            // Check authentication
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            _quotationResponseRepository.MarkAsRead(id);
            TempData["SuccessMessage"] = "Notification marked as read.";

            return RedirectToPage();
        }
    }
}

