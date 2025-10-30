using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for customer-related business logic operations
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves all customers
        /// </summary>
        List<Customer> GetAll();
        /// <summary>
        /// Retrieves a customer by their ID
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
        /// Registers a new customer
        /// </summary>
        /// <param name="customer">Customer entity to register</param>
        /// <param name="password">Plain text password</param>
        /// <returns>Registration result with success status and any error messages</returns>
        Task<RegistrationResult> RegisterCustomerAsync(Customer customer, string password);
        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">Customer entity with updated information</param>
        void Update(Customer customer);
        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">Customer ID to delete</param>
        void Delete(int id);
        /// <summary>
        /// Validates customer credentials for login
        /// </summary>
        /// <param name="email">Customer email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        bool ValidateCredentials(string email, string password);
    }
    /// <summary>
    /// Represents the result of a registration operation (customer or employee)
    /// </summary>
    public class RegistrationResult
    {
        /// <summary>
        /// Whether registration was successful
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Error message if registration failed
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// The registered entity (Customer or Employee) or null if failed
        /// </summary>
        public object? Entity { get; set; }
        /// <summary>
        /// Creates a RegistrationResult with IsSuccess true and supplied entity
        /// </summary>
        /// <param name="entity">The entity registered</param>
        /// <returns>Successful RegistrationResult</returns>
        public static RegistrationResult Success(object entity)
        {
            return new RegistrationResult { IsSuccess = true, Entity = entity };
        }
        /// <summary>
        /// Creates a RegistrationResult with IsSuccess false and error message
        /// </summary>
        /// <param name="errorMessage">Reason for failure</param>
        /// <returns>Failed RegistrationResult</returns>
        public static RegistrationResult Failure(string errorMessage)
        {
            return new RegistrationResult { IsSuccess = false, ErrorMessage = errorMessage };
        }
    }
}
