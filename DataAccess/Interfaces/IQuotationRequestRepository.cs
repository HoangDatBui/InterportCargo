using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for quotation request data access operations
    /// </summary>
    public interface IQuotationRequestRepository
    {
        /// <summary>
        /// Retrieves all quotation requests from the data store
        /// </summary>
        /// <returns>List of all quotation requests</returns>
        List<QuotationRequest> GetAll();

        /// <summary>
        /// Retrieves a quotation request by its unique identifier
        /// </summary>
        /// <param name="id">Quotation request ID</param>
        /// <returns>Quotation request entity or null if not found</returns>
        QuotationRequest? GetById(int id);

        /// <summary>
        /// Retrieves all quotation requests for a specific customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of quotation requests for the customer</returns>
        List<QuotationRequest> GetByCustomerId(int customerId);

        /// <summary>
        /// Adds a new quotation request to the data store
        /// </summary>
        /// <param name="quotationRequest">Quotation request entity to add</param>
        void Add(QuotationRequest quotationRequest);

        /// <summary>
        /// Updates an existing quotation request in the data store
        /// </summary>
        /// <param name="quotationRequest">Quotation request entity with updated information</param>
        void Update(QuotationRequest quotationRequest);

        /// <summary>
        /// Deletes a quotation request from the data store
        /// </summary>
        /// <param name="id">Quotation request ID to delete</param>
        void Delete(int id);
    }
}
