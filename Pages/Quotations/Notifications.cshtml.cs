using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.DataAccess.Interfaces;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.Pages.Quotations
{
    public class NotificationsModel : PageModel
    {
        private readonly IQuotationResponseRepository _quotationResponseRepository;
        private readonly IQuotationRequestRepository _quotationRequestRepository;

        public NotificationsModel(IQuotationResponseRepository quotationResponseRepository, IQuotationRequestRepository quotationRequestRepository)
        {
            _quotationResponseRepository = quotationResponseRepository;
            _quotationRequestRepository = quotationRequestRepository;
        }

        public List<NotificationViewModel> Notifications { get; set; } = new();
        public int UnreadCount { get; set; }

        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Get customer ID
            var customerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (customerId == 0)
            {
                return RedirectToPage("/Quotations/Index");
            }

            // Get officer notifications for the customer only (exclude customer-authored responses)
            var allResponses = _quotationResponseRepository.GetAll()
                .Where(r => r.ResponseType == "Officer")
                .OrderByDescending(r => r.CreatedDate)
                .ToList();

            // Get customer's quotation request IDs
            var customerQuotationIds = _quotationRequestRepository.GetByCustomerId(customerId)
                .Select(q => q.Id)
                .ToList();

            // Filter to customer's notifications and create view models
            Notifications = allResponses
                .Where(r => customerQuotationIds.Contains(r.QuotationRequestId))
                .Select(r => new NotificationViewModel
                {
                    Id = r.Id,
                    Status = r.Status,
                    Message = r.Message ?? "",
                    OfficerName = r.OfficerName,
                    CreatedDate = r.CreatedDate,
                    IsRead = r.IsRead,
                    QuotationRequestId = r.QuotationRequestId
                })
                .ToList();

            UnreadCount = Notifications.Count(n => !n.IsRead);

            return Page();
        }

        public IActionResult OnPost(int? notificationId)
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!notificationId.HasValue)
            {
                return RedirectToPage("/Quotations/Notifications");
            }

            // Mark notification as read
            _quotationResponseRepository.MarkAsRead(notificationId.Value);

            TempData["SuccessMessage"] = "Notification marked as read.";

            return RedirectToPage("/Quotations/Notifications");
        }

        public string GetQuotationRequestId(int quotationRequestId)
        {
            var quotationRequest = _quotationRequestRepository.GetById(quotationRequestId);
            return quotationRequest?.RequestId ?? "Unknown";
        }
    }

    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string OfficerName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public int QuotationRequestId { get; set; }
    }
}
