using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for customer business logic operations
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves all customers
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
        /// Registers a new customer with validation and password hashing
        /// </summary>
        /// <param name="customer">Customer entity to register</param>
        /// <param name="password">Plain text password to hash</param>
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
    /// Result of customer registration operation
    /// </summary>
    public class RegistrationResult
    {
        /// <summary>
        /// Indicates whether the registration was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if registration failed
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// The registered customer entity if successful
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// Creates a successful registration result
        /// </summary>
        /// <param name="customer">Registered customer</param>
        /// <returns>Successful registration result</returns>
        public static RegistrationResult Success(Customer customer)
        {
            return new RegistrationResult
            {
                IsSuccess = true,
                Customer = customer
            };
        }

        /// <summary>
        /// Creates a failed registration result
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <returns>Failed registration result</returns>
        public static RegistrationResult Failure(string errorMessage)
        {
            return new RegistrationResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
