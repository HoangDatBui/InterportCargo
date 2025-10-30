using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Data;
using InterportCargo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.DataAccess.Repositories
{
    /// <summary>
    /// Entity Framework implementation of the QuotationDetails repository
    /// </summary>
    public class EFQuotationDetailsRepository : IQuotationDetailsRepository
    {
        private readonly InterportCargoDbContext _context;

        /// <summary>
        /// Initialises a new instance of the EFQuotationDetailsRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public EFQuotationDetailsRepository(InterportCargoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all quotation details
        /// </summary>
        /// <returns>List of all quotation details</returns>
        public List<QuotationDetails> GetAll()
        {
            return _context.QuotationDetails
                .Include(qd => qd.QuotationRequest)
                .ToList();
        }

        /// <summary>
        /// Get quotation details by ID
        /// </summary>
        /// <param name="id">Quotation details ID</param>
        /// <returns>Quotation details or null if not found</returns>
        public QuotationDetails? GetById(int id)
        {
            return _context.QuotationDetails
                .Include(qd => qd.QuotationRequest)
                .FirstOrDefault(qd => qd.Id == id);
        }

        /// <summary>
        /// Get quotation details by quotation request ID
        /// </summary>
        /// <param name="quotationRequestId">Quotation request ID</param>
        /// <returns>Quotation details or null if not found</returns>
        public QuotationDetails? GetByQuotationRequestId(int quotationRequestId)
        {
            return _context.QuotationDetails
                .Include(qd => qd.QuotationRequest)
                .FirstOrDefault(qd => qd.QuotationRequestId == quotationRequestId);
        }

        /// <summary>
        /// Get quotation details by customer ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of quotation details for the customer</returns>
        public List<QuotationDetails> GetByCustomerId(int customerId)
        {
            return _context.QuotationDetails
                .Include(qd => qd.QuotationRequest)
                .Where(qd => qd.QuotationRequest != null && qd.QuotationRequest.CustomerId == customerId)
                .ToList();
        }

        /// <summary>
        /// Get quotation details by quotation number
        /// </summary>
        /// <param name="quotationNumber">Quotation number</param>
        /// <returns>Quotation details or null if not found</returns>
        public QuotationDetails? GetByQuotationNumber(string quotationNumber)
        {
            return _context.QuotationDetails
                .Include(qd => qd.QuotationRequest)
                .FirstOrDefault(qd => qd.QuotationNumber == quotationNumber);
        }

        /// <summary>
        /// Add a new quotation details
        /// </summary>
        /// <param name="quotationDetails">Quotation details to add</param>
        public void Add(QuotationDetails quotationDetails)
        {
            _context.QuotationDetails.Add(quotationDetails);
            _context.SaveChanges();
        }

        /// <summary>
        /// Update an existing quotation details
        /// </summary>
        /// <param name="quotationDetails">Quotation details to update</param>
        public void Update(QuotationDetails quotationDetails)
        {
            _context.Entry(quotationDetails).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete a quotation details by ID
        /// </summary>
        /// <param name="id">Quotation details ID</param>
        public void Delete(int id)
        {
            var quotationDetails = _context.QuotationDetails.Find(id);
            if (quotationDetails != null)
            {
                _context.QuotationDetails.Remove(quotationDetails);
                _context.SaveChanges();
            }
        }
    }
}
