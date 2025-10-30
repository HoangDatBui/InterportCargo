using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;
using InterportCargo.DataAccess.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace InterportCargo.BusinessLogic.Services
{
    /// <summary>
    /// Service implementation for customer-related operations.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        /// <summary>
        /// Retrieves all customers from the repository.
        /// </summary>
        public List<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }
        /// <summary>
        /// Retrieves a customer by unique identifier.
        /// </summary>
        public Customer? GetById(int id)
        {
            return _customerRepository.GetById(id);
        }
        /// <summary>
        /// Retrieves a customer by their email address.
        /// </summary>
        public Customer? GetByEmail(string email)
        {
            return _customerRepository.GetByEmail(email);
        }
        /// <summary>
        /// Registers a new customer with validation and password hashing.
        /// </summary>
        /// <param name="customer">Customer entity to register.</param>
        /// <param name="password">Plain text password to hash.</param>
        /// <returns>Registration result with success status and error messages.</returns>
        public async Task<RegistrationResult> RegisterCustomerAsync(Customer customer, string password)
        {
            if (customer == null) return RegistrationResult.Failure("Customer information is required.");
            if (string.IsNullOrWhiteSpace(password)) return RegistrationResult.Failure("Password is required.");
            if (string.IsNullOrWhiteSpace(customer.Email)) return RegistrationResult.Failure("Email address is required.");
            if (_customerRepository.ExistsByEmail(customer.Email)) return RegistrationResult.Failure("A customer with this email address already exists.");
            customer.PasswordHash = HashPassword(password);
            customer.CreatedDate = DateTime.UtcNow;
            customer.IsActive = true;
            _customerRepository.Add(customer);
            await Task.Delay(1); // Simulate async
            return RegistrationResult.Success(customer);
        }
        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="customer">Customer entity with updated information.</param>
        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
        }
        /// <summary>
        /// Deletes a customer by unique identifier.
        /// </summary>
        /// <param name="id">Customer ID to delete.</param>
        public void Delete(int id)
        {
            _customerRepository.Delete(id);
        }
        /// <summary>
        /// Validates provided credentials against stored hash.
        /// </summary>
        /// <param name="email">Customer email.</param>
        /// <param name="password">Plain text password.</param>
        /// <returns>True if credentials are valid; otherwise, false.</returns>
        public bool ValidateCredentials(string email, string password)
        {
            var customer = _customerRepository.GetByEmail(email);
            if (customer == null || !customer.IsActive) return false;
            return VerifyPassword(password, customer.PasswordHash);
        }
        /// <summary>
        /// Hashes a password using SHA256.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <returns>Base64 hash.</returns>
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
        /// <summary>
        /// Verifies a password against its stored hash.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <param name="hash">Stored hash to verify.</param>
        /// <returns>True if match; otherwise, false.</returns>
        private static bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword.Equals(hash, StringComparison.Ordinal);
        }
    }
}
