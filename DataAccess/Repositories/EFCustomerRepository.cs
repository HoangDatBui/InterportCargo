using InterportCargo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using InterportCargo.DataAccess.Data;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Repositories
{
    /// <summary>
    /// Entity Framework Core implementation of customer repository
    /// </summary>
    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly InterportCargoDbContext _context;

        /// <summary>
        /// Initialises a new instance of the EFCustomerRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public EFCustomerRepository(InterportCargoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all customers from the database
        /// </summary>
        /// <returns>List of all customers</returns>
        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer entity or null if not found</returns>
        public Customer? GetById(int id)
        {
            return _context.Customers.Find(id);
        }

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">Customer email address</param>
        /// <returns>Customer entity or null if not found</returns>
        public Customer? GetByEmail(string email)
        {
            return _context.Customers
                .FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Adds a new customer to the database
        /// </summary>
        /// <param name="customer">Customer entity to add</param>
        public void Add(Customer customer)
        {
            customer.CreatedDate = DateTime.UtcNow;
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing customer in the database
        /// </summary>
        /// <param name="customer">Customer entity with updated information</param>
        public void Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a customer from the database
        /// </summary>
        /// <param name="id">Customer ID to delete</param>
        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Checks if a customer with the given email already exists
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <returns>True if customer exists, false otherwise</returns>
        public bool ExistsByEmail(string email)
        {
            return _context.Customers
                .Any(c => c.Email.ToLower() == email.ToLower());
        }
    }
}
