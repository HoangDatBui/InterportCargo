using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for customer data access operations
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves all customers from the data store
        /// </summary>
        /// <returns>List of all customers</returns>
        List<Customer> GetAll();

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer entity or null if not found</returns>
        Customer? GetById(int id);

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">Customer email address</param>
        /// <returns>Customer entity or null if not found</returns>
        Customer? GetByEmail(string email);

        /// <summary>
        /// Adds a new customer to the data store
        /// </summary>
        /// <param name="customer">Customer entity to add</param>
        void Add(Customer customer);

        /// <summary>
        /// Updates an existing customer in the data store
        /// </summary>
        /// <param name="customer">Customer entity with updated information</param>
        void Update(Customer customer);

        /// <summary>
        /// Deletes a customer from the data store
        /// </summary>
        /// <param name="id">Customer ID to delete</param>
        void Delete(int id);

        /// <summary>
        /// Checks if a customer with the given email already exists
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <returns>True if customer exists, false otherwise</returns>
        bool ExistsByEmail(string email);
    }
}
