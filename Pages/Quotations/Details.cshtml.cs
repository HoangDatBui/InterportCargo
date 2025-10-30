using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;

namespace InterportCargo.Pages.Quotations
{
    public class DetailsModel : PageModel
    {
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private readonly IQuotationResponseRepository _quotationResponseRepository;
        private readonly IQuotationDetailsRepository _quotationDetailsRepository;

        public DetailsModel(
            IQuotationRequestRepository quotationRequestRepository, 
            IQuotationResponseRepository quotationResponseRepository,
            IQuotationDetailsRepository quotationDetailsRepository)
        {
            _quotationRequestRepository = quotationRequestRepository;
            _quotationResponseRepository = quotationResponseRepository;
            _quotationDetailsRepository = quotationDetailsRepository;
        }

        public QuotationRequest? QuotationRequest { get; set; }
        public bool IsEmployee { get; set; }
        public bool HasQuotationDetails { get; set; }
        public List<QuotationResponse> CustomerResponses { get; set; } = new();

        public IActionResult OnGet(int? id)
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!id.HasValue)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Get quotation request
            QuotationRequest = _quotationRequestRepository.GetById(id.Value);

            if (QuotationRequest == null)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Check if user is employee
            var userType = HttpContext.Session.GetString("UserType") ?? string.Empty;
            IsEmployee = !string.IsNullOrEmpty(userType) && userType != "Customer";

            // If user is customer, only allow viewing their own quotations
            if (!IsEmployee)
            {
                var customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
                if (QuotationRequest.CustomerId != customerId)
                {
                    return RedirectToPage("/Quotations/Index");
                }
            }

            // Check if quotation details already exist
            HasQuotationDetails = _quotationDetailsRepository.GetByQuotationRequestId(QuotationRequest.Id) != null;

            // Get customer responses for this quotation request
            if (IsEmployee && HasQuotationDetails)
            {
                CustomerResponses = _quotationResponseRepository.GetCustomerResponsesByQuotationRequestId(QuotationRequest.Id);
            }

            return Page();
        }

        public IActionResult OnPost(string action, int? id, string? rejectMessage)
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!id.HasValue)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Check if user is employee
            var userType = HttpContext.Session.GetString("UserType") ?? string.Empty;
            var isEmployee = !string.IsNullOrEmpty(userType) && userType != "Customer";

            if (!isEmployee)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Get quotation request
            var quotationRequest = _quotationRequestRepository.GetById(id.Value);
            if (quotationRequest == null)
            {
                return RedirectToPage("/Quotations/Index");
            }

            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var userName = HttpContext.Session.GetString("UserName") ?? "Officer";

            if (action == "accept")
            {
                // Update quotation request status
                quotationRequest.Status = "Accepted";
                _quotationRequestRepository.Update(quotationRequest);

                // Create response
                var response = new QuotationResponse
                {
                    QuotationRequestId = quotationRequest.Id,
                    OfficerId = userId,
                    OfficerName = userName,
                    ResponseType = "Officer",
                    Status = "Accepted",
                    Message = "Your quotation request has been accepted. We will contact you shortly with the quotation details.",
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                };
                _quotationResponseRepository.Add(response);
            }
            else if (action == "reject")
            {
                if (string.IsNullOrWhiteSpace(rejectMessage))
                {
                    ModelState.AddModelError("", "Rejection message is required.");
                    // Reload the page with error
                    QuotationRequest = quotationRequest;
                    IsEmployee = true;
                    return Page();
                }

                // Update quotation request status
                quotationRequest.Status = "Rejected";
                _quotationRequestRepository.Update(quotationRequest);

                // Create response
                var response = new QuotationResponse
                {
                    QuotationRequestId = quotationRequest.Id,
                    OfficerId = userId,
                    OfficerName = userName,
                    ResponseType = "Officer",
                    Status = "Rejected",
                    Message = rejectMessage,
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                };
                _quotationResponseRepository.Add(response);
            }

            TempData["SuccessMessage"] = action == "accept" 
                ? "Quotation request has been accepted successfully!" 
                : "Quotation request has been rejected.";
            
            return RedirectToPage("/Quotations/Index");
        }
    }
}
