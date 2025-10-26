using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;

namespace InterportCargo.Application.Interfaces
{
    /// <summary>
    /// Application service interface for customer operations
    /// </summary>
    public interface ICustomerAppService
    {
        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        List<Customer> GetAllCustomers();

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer entity or null if not found</returns>
        Customer? GetCustomer(int id);

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">Customer email address</param>
        /// <returns>Customer entity or null if not found</returns>
        Customer? GetCustomerByEmail(string email);

        /// <summary>
        /// Registers a new customer
        /// </summary>
        /// <param name="firstName">Customer's first name</param>
        /// <param name="familyName">Customer's family name</param>
        /// <param name="email">Customer's email address</param>
        /// <param name="phoneNumber">Customer's phone number</param>
        /// <param name="companyName">Customer's company name (optional)</param>
        /// <param name="address">Customer's address</param>
        /// <param name="password">Customer's password</param>
        /// <returns>Registration result with success status and any error messages</returns>
        Task<RegistrationResult> RegisterCustomerAsync(string firstName, string familyName, string email, 
            string phoneNumber, string? companyName, string address, string password);

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">Customer entity with updated information</param>
        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">Customer ID to delete</param>
        void DeleteCustomer(int id);

        /// <summary>
        /// Validates customer credentials for login
        /// </summary>
        /// <param name="email">Customer email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        bool ValidateCustomerCredentials(string email, string password);
    }
}
