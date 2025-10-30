using InterportCargo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using InterportCargo.DataAccess.Data;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Repositories
{
    /// <summary>
    /// Entity Framework Core implementation of quotation request repository
    /// </summary>
    public class EFQuotationRequestRepository : IQuotationRequestRepository
    {
        private readonly InterportCargoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the EFQuotationRequestRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public EFQuotationRequestRepository(InterportCargoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all quotation requests from the database
        /// </summary>
        /// <returns>List of all quotation requests</returns>
        public List<QuotationRequest> GetAll()
        {
            return _context.QuotationRequests.ToList();
        }

        /// <summary>
        /// Retrieves a quotation request by its unique identifier
        /// </summary>
        /// <param name="id">Quotation request ID</param>
        /// <returns>Quotation request entity or null if not found</returns>
        public QuotationRequest? GetById(int id)
        {
            return _context.QuotationRequests.Find(id);
        }

        /// <summary>
        /// Retrieves all quotation requests for a specific customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of quotation requests for the customer</returns>
        public List<QuotationRequest> GetByCustomerId(int customerId)
        {
            return _context.QuotationRequests
                .Where(q => q.CustomerId == customerId)
                .OrderByDescending(q => q.CreatedDate)
                .ToList();
        }

        /// <summary>
        /// Adds a new quotation request to the database
        /// </summary>
        /// <param name="quotationRequest">Quotation request entity to add</param>
        public void Add(QuotationRequest quotationRequest)
        {
            quotationRequest.CreatedDate = DateTime.UtcNow;
            _context.QuotationRequests.Add(quotationRequest);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing quotation request in the database
        /// </summary>
        /// <param name="quotationRequest">Quotation request entity with updated information</param>
        public void Update(QuotationRequest quotationRequest)
        {
            _context.Entry(quotationRequest).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a quotation request from the database
        /// </summary>
        /// <param name="id">Quotation request ID to delete</param>
        public void Delete(int id)
        {
            var quotationRequest = _context.QuotationRequests.Find(id);
            if (quotationRequest != null)
            {
                _context.QuotationRequests.Remove(quotationRequest);
                _context.SaveChanges();
            }
        }
    }
}
