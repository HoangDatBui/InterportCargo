using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;

namespace InterportCargo.Pages.Quotations
{
    /// <summary>
    /// Page model for viewing quotation details (prepared quotation)
    /// </summary>
    public class QuotationDetailsModel : PageModel
    {
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private readonly IQuotationDetailsRepository _quotationDetailsRepository;
        private readonly IQuotationResponseRepository _quotationResponseRepository;

        /// <summary>
        /// Initializes a new instance of the QuotationDetailsModel
        /// </summary>
        public QuotationDetailsModel(
            IQuotationRequestRepository quotationRequestRepository,
            IQuotationDetailsRepository quotationDetailsRepository,
            IQuotationResponseRepository quotationResponseRepository)
        {
            _quotationRequestRepository = quotationRequestRepository;
            _quotationDetailsRepository = quotationDetailsRepository;
            _quotationResponseRepository = quotationResponseRepository;
        }

        public QuotationDetails? QuotationDetails { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public bool IsEmployee { get; set; }
        public int QuotationRequestId { get; set; }

        /// <summary>
        /// Handles GET requests to display quotation details
        /// </summary>
        public IActionResult OnGet(int id)
        {
            // Check authentication
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            QuotationRequestId = id;

            // Get quotation request
            var quotationRequest = _quotationRequestRepository.GetById(id);
            if (quotationRequest == null)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Get quotation details
            QuotationDetails = _quotationDetailsRepository.GetByQuotationRequestId(id);
            if (QuotationDetails == null)
            {
                return RedirectToPage("/Quotations/Details", new { id = id });
            }

            // Set customer info
            CustomerName = quotationRequest.CustomerName;
            CustomerEmail = quotationRequest.CustomerEmail;
            RequestId = quotationRequest.RequestId;

            // Check if user is employee
            var userType = HttpContext.Session.GetString("UserType") ?? string.Empty;
            IsEmployee = !string.IsNullOrEmpty(userType) && userType != "Customer";

            // If user is customer, only allow viewing their own quotations
            if (!IsEmployee)
            {
                var customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
                if (quotationRequest.CustomerId != customerId)
                {
                    return RedirectToPage("/Quotations/Index");
                }
            }

            return Page();
        }

        /// <summary>
        /// Handles POST requests to accept or reject quotation
        /// </summary>
        public IActionResult OnPost(string action, string? rejectReason)
        {
            // Check authentication
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Check if user is customer
            var userType = HttpContext.Session.GetString("UserType") ?? string.Empty;
            var isCustomer = userType == "Customer";

            if (!isCustomer)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Get quotation request ID from route
            var id = RouteData.Values["id"]?.ToString();
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out var quotationRequestId))
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Get quotation details
            var quotationDetails = _quotationDetailsRepository.GetByQuotationRequestId(quotationRequestId);
            if (quotationDetails == null)
            {
                return RedirectToPage("/Quotations/Index");
            }

            if (action == "accept")
            {
                quotationDetails.Status = "Accepted";
                _quotationDetailsRepository.Update(quotationDetails);

                // Update quotation request status
                var requestAccepted = _quotationRequestRepository.GetById(quotationRequestId);
                if (requestAccepted != null)
                {
                    requestAccepted.Status = "Quoted - Accepted";
                    _quotationRequestRepository.Update(requestAccepted);

                    // Create customer response notification for officer
                    var customerResponse = new QuotationResponse
                    {
                        QuotationRequestId = quotationRequestId,
                        CustomerId = requestAccepted.CustomerId,
                        CustomerName = requestAccepted.CustomerName,
                        ResponseType = "Customer",
                        Status = "Accepted",
                        Message = $"Customer has accepted quotation {quotationDetails.QuotationNumber}.",
                        CreatedDate = DateTime.UtcNow,
                        IsRead = false
                    };
                    
                    _quotationResponseRepository.Add(customerResponse);
                }

                TempData["SuccessMessage"] = "Quotation accepted! We will contact you shortly.";
            }
            else if (action == "reject")
            {
                if (string.IsNullOrWhiteSpace(rejectReason))
                {
                    ModelState.AddModelError("", "Rejection reason is required.");
                    // Reload data
                    QuotationDetails = quotationDetails;
                    var quotationRequest = _quotationRequestRepository.GetById(quotationRequestId);
                    if (quotationRequest != null)
                    {
                        CustomerName = quotationRequest.CustomerName;
                        CustomerEmail = quotationRequest.CustomerEmail;
                        RequestId = quotationRequest.RequestId;
                        IsEmployee = false;
                        QuotationRequestId = quotationRequestId;
                    }
                    return Page();
                }

                quotationDetails.Status = "Rejected";
                _quotationDetailsRepository.Update(quotationDetails);

                // Update quotation request status
                var requestRejected = _quotationRequestRepository.GetById(quotationRequestId);
                if (requestRejected != null)
                {
                    requestRejected.Status = "Quoted - Rejected";
                    _quotationRequestRepository.Update(requestRejected);

                    // Create customer response notification for officer
                    var customerResponse = new QuotationResponse
                    {
                        QuotationRequestId = quotationRequestId,
                        CustomerId = requestRejected.CustomerId,
                        CustomerName = requestRejected.CustomerName,
                        ResponseType = "Customer",
                        Status = "Rejected",
                        Message = $"Customer has rejected quotation {quotationDetails.QuotationNumber}. Reason: {rejectReason}",
                        CreatedDate = DateTime.UtcNow,
                        IsRead = false
                    };
                    
                    _quotationResponseRepository.Add(customerResponse);
                }

                TempData["SuccessMessage"] = "Quotation has been rejected.";
            }

            return RedirectToPage("/Quotations/Index");
        }
    }
}
