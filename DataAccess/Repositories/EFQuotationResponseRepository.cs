using InterportCargo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using InterportCargo.DataAccess.Data;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Repositories
{
    /// <summary>
    /// Entity Framework Core implementation of quotation response repository
    /// </summary>
    public class EFQuotationResponseRepository : IQuotationResponseRepository
    {
        private readonly InterportCargoDbContext _context;

        /// <summary>
        /// Initialises a new instance of the EFQuotationResponseRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public EFQuotationResponseRepository(InterportCargoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all quotation responses from the database
        /// </summary>
        /// <returns>List of all quotation responses</returns>
        public List<QuotationResponse> GetAll()
        {
            return _context.QuotationResponses.ToList();
        }

        /// <summary>
        /// Retrieves a quotation response by its unique identifier
        /// </summary>
        /// <param name="id">Quotation response ID</param>
        /// <returns>Quotation response entity or null if not found</returns>
        public QuotationResponse? GetById(int id)
        {
            return _context.QuotationResponses.Find(id);
        }

        /// <summary>
        /// Retrieves response for a specific quotation request
        /// </summary>
        /// <param name="quotationRequestId">Quotation request ID</param>
        /// <returns>Quotation response entity or null if not found</returns>
        public QuotationResponse? GetByQuotationRequestId(int quotationRequestId)
        {
            return _context.QuotationResponses
                .FirstOrDefault(qr => qr.QuotationRequestId == quotationRequestId);
        }

        /// <summary>
        /// Retrieves all unread notifications for a specific customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of unread quotation responses</returns>
        public List<QuotationResponse> GetUnreadNotificationsByCustomerId(int customerId)
        {
            var quotationRequestIds = _context.QuotationRequests
                .Where(q => q.CustomerId == customerId)
                .Select(q => q.Id)
                .ToList();

            return _context.QuotationResponses
                .Where(qr => quotationRequestIds.Contains(qr.QuotationRequestId) && !qr.IsRead)
                .OrderByDescending(qr => qr.CreatedDate)
                .ToList();
        }

        /// <summary>
        /// Adds a new quotation response to the database
        /// </summary>
        /// <param name="quotationResponse">Quotation response entity to add</param>
        public void Add(QuotationResponse quotationResponse)
        {
            quotationResponse.CreatedDate = DateTime.UtcNow;
            _context.QuotationResponses.Add(quotationResponse);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing quotation response in the database
        /// </summary>
        /// <param name="quotationResponse">Quotation response entity with updated information</param>
        public void Update(QuotationResponse quotationResponse)
        {
            _context.Entry(quotationResponse).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Marks a notification as read
        /// </summary>
        /// <param name="id">Quotation response ID</param>
        public void MarkAsRead(int id)
        {
            var quotationResponse = _context.QuotationResponses.Find(id);
            if (quotationResponse != null)
            {
                quotationResponse.IsRead = true;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves all unread notifications for officers (customer responses)
        /// </summary>
        /// <param name="officerId">Optional officer ID to filter by. If null, returns all officer notifications</param>
        /// <returns>List of unread customer responses</returns>
        public List<QuotationResponse> GetUnreadNotificationsForOfficers(int? officerId = null)
        {
            var query = _context.QuotationResponses
                .Where(qr => qr.ResponseType == "Customer" && !qr.IsRead)
                .AsQueryable();

            // If officerId is provided, filter by quotations prepared by that officer
            if (officerId.HasValue)
            {
                var quotationRequestIds = _context.QuotationDetails
                    .Where(qd => qd.OfficerId == officerId.Value)
                    .Select(qd => qd.QuotationRequestId)
                    .ToList();

                query = query.Where(qr => quotationRequestIds.Contains(qr.QuotationRequestId));
            }

            return query
                .OrderByDescending(qr => qr.CreatedDate)
                .ToList();
        }

        /// <summary>
        /// Retrieves all customer responses for a specific quotation request
        /// </summary>
        /// <param name="quotationRequestId">Quotation request ID</param>
        /// <returns>List of customer responses</returns>
        public List<QuotationResponse> GetCustomerResponsesByQuotationRequestId(int quotationRequestId)
        {
            return _context.QuotationResponses
                .Where(qr => qr.QuotationRequestId == quotationRequestId && qr.ResponseType == "Customer")
                .OrderByDescending(qr => qr.CreatedDate)
                .ToList();
        }
    }
}
