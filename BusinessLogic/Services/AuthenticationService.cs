using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;
using InterportCargo.DataAccess.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace InterportCargo.BusinessLogic.Services
{
    /// <summary>
    /// Service implementation for unified authentication operations
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Initialises a new instance of the AuthenticationService
        /// </summary>
        /// <param name="customerRepository">Customer repository</param>
        /// <param name="employeeRepository">Employee repository</param>
        public AuthenticationService(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Authenticates a user (customer or employee) and returns user information
        /// </summary>
        /// <param name="email">User email address</param>
        /// <param name="password">Plain text password</param>
        /// <returns>User authentication result with user information</returns>
        public AuthenticationResult AuthenticateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return AuthenticationResult.Failure("Email and password are required.");
            }

            // Try to authenticate as customer first
            var customer = _customerRepository.GetByEmail(email);
            if (customer != null && customer.IsActive && VerifyPassword(password, customer.PasswordHash))
            {
                return AuthenticationResult.Success(
                    customer.Email,
                    customer.FullName,
                    "Customer",
                    customer.Id
                );
            }

            // Try to authenticate as employee
            var employee = _employeeRepository.GetByEmail(email);
            if (employee != null && employee.IsActive && VerifyPassword(password, employee.PasswordHash))
            {
                return AuthenticationResult.Success(
                    employee.Email,
                    employee.FullName,
                    employee.EmployeeType,
                    employee.Id
                );
            }

            return AuthenticationResult.Failure("Invalid email or password. Please try again.");
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
    }
}
