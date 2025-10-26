using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;
using InterportCargo.DataAccess.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace InterportCargo.BusinessLogic.Services
{
    /// <summary>
    /// Service implementation for customer business logic operations
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// Initializes a new instance of the CustomerService
        /// </summary>
        /// <param name="customerRepository">Customer repository dependency</param>
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        public List<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer entity or null if not found</returns>
        public Customer? GetById(int id)
        {
            return _customerRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">Customer email address</param>
        /// <returns>Customer entity or null if not found</returns>
        public Customer? GetByEmail(string email)
        {
            return _customerRepository.GetByEmail(email);
        }

        /// <summary>
        /// Registers a new customer with validation and password hashing
        /// </summary>
        /// <param name="customer">Customer entity to register</param>
        /// <param name="password">Plain text password to hash</param>
        /// <returns>Registration result with success status and any error messages</returns>
        public async Task<RegistrationResult> RegisterCustomerAsync(Customer customer, string password)
        {
            // Validate input
            if (customer == null)
                return RegistrationResult.Failure("Customer information is required.");

            if (string.IsNullOrWhiteSpace(password))
                return RegistrationResult.Failure("Password is required.");

            if (string.IsNullOrWhiteSpace(customer.Email))
                return RegistrationResult.Failure("Email address is required.");

            // Check if customer already exists
            if (_customerRepository.ExistsByEmail(customer.Email))
                return RegistrationResult.Failure("A customer with this email address already exists.");

            // Hash the password
            customer.PasswordHash = HashPassword(password);
            customer.CreatedDate = DateTime.UtcNow;
            customer.IsActive = true;

            // Add customer to repository
            _customerRepository.Add(customer);

            // Simulate async operation
            await Task.Delay(1);

            return RegistrationResult.Success(customer);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">Customer entity with updated information</param>
        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">Customer ID to delete</param>
        public void Delete(int id)
        {
            _customerRepository.Delete(id);
        }

        /// <summary>
        /// Validates customer credentials for login
        /// </summary>
        /// <param name="email">Customer email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public bool ValidateCredentials(string email, string password)
        {
            var customer = _customerRepository.GetByEmail(email);
            if (customer == null || !customer.IsActive)
                return false;

            return VerifyPassword(password, customer.PasswordHash);
        }

        /// <summary>
        /// Hashes a password using SHA256
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password</returns>
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// Verifies a password against its hash
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="hash">Hashed password</param>
        /// <returns>True if password matches hash, false otherwise</returns>
        private static bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword.Equals(hash, StringComparison.Ordinal);
        }
    }
}
