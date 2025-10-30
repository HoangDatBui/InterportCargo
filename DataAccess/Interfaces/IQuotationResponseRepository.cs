using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for quotation response data access operations
    /// </summary>
    public interface IQuotationResponseRepository
    {
        /// <summary>
        /// Retrieves all quotation responses from the data store
        /// </summary>
        /// <returns>List of all quotation responses</returns>
        List<QuotationResponse> GetAll();

        /// <summary>
        /// Retrieves a quotation response by its unique identifier
        /// </summary>
        /// <param name="id">Quotation response ID</param>
        /// <returns>Quotation response entity or null if not found</returns>
        QuotationResponse? GetById(int id);

        /// <summary>
        /// Retrieves all responses for a specific quotation request
        /// </summary>
        /// <param name="quotationRequestId">Quotation request ID</param>
        /// <returns>List of quotation responses</returns>
        QuotationResponse? GetByQuotationRequestId(int quotationRequestId);

        /// <summary>
        /// Retrieves all unread notifications for a specific customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of unread quotation responses</returns>
        List<QuotationResponse> GetUnreadNotificationsByCustomerId(int customerId);

        /// <summary>
        /// Adds a new quotation response to the data store
        /// </summary>
        /// <param name="quotationResponse">Quotation response entity to add</param>
        void Add(QuotationResponse quotationResponse);

        /// <summary>
        /// Updates an existing quotation response in the data store
        /// </summary>
        /// <param name="quotationResponse">Quotation response entity with updated information</param>
        void Update(QuotationResponse quotationResponse);

        /// <summary>
        /// Marks a notification as read
        /// </summary>
        /// <param name="id">Quotation response ID</param>
        void MarkAsRead(int id);

        /// <summary>
        /// Retrieves all unread notifications for officers (customer responses)
        /// </summary>
        /// <param name="officerId">Optional officer ID to filter by. If null, returns all officer notifications</param>
        /// <returns>List of unread customer responses</returns>
        List<QuotationResponse> GetUnreadNotificationsForOfficers(int? officerId = null);

        /// <summary>
        /// Retrieves all customer responses for a specific quotation request
        /// </summary>
        /// <param name="quotationRequestId">Quotation request ID</param>
        /// <returns>List of customer responses</returns>
        List<QuotationResponse> GetCustomerResponsesByQuotationRequestId(int quotationRequestId);
    }
}
