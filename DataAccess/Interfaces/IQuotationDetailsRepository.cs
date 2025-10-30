using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for QuotationDetails data access operations
    /// </summary>
    public interface IQuotationDetailsRepository
    {
        /// <summary>
        /// Get all quotation details
        /// </summary>
        /// <returns>List of all quotation details</returns>
        List<QuotationDetails> GetAll();

        /// <summary>
        /// Get quotation details by ID
        /// </summary>
        /// <param name="id">Quotation details ID</param>
        /// <returns>Quotation details or null if not found</returns>
        QuotationDetails? GetById(int id);

        /// <summary>
        /// Get quotation details by quotation request ID
        /// </summary>
        /// <param name="quotationRequestId">Quotation request ID</param>
        /// <returns>Quotation details or null if not found</returns>
        QuotationDetails? GetByQuotationRequestId(int quotationRequestId);

        /// <summary>
        /// Get quotation details by customer ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of quotation details for the customer</returns>
        List<QuotationDetails> GetByCustomerId(int customerId);

        /// <summary>
        /// Get quotation details by quotation number
        /// </summary>
        /// <param name="quotationNumber">Quotation number</param>
        /// <returns>Quotation details or null if not found</returns>
        QuotationDetails? GetByQuotationNumber(string quotationNumber);

        /// <summary>
        /// Add a new quotation details
        /// </summary>
        /// <param name="quotationDetails">Quotation details to add</param>
        void Add(QuotationDetails quotationDetails);

        /// <summary>
        /// Update an existing quotation details
        /// </summary>
        /// <param name="quotationDetails">Quotation details to update</param>
        void Update(QuotationDetails quotationDetails);

        /// <summary>
        /// Delete a quotation details by ID
        /// </summary>
        /// <param name="id">Quotation details ID</param>
        void Delete(int id);
    }
}
