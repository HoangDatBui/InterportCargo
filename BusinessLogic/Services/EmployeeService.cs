using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;
using InterportCargo.DataAccess.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace InterportCargo.BusinessLogic.Services
{
    /// <summary>
    /// Service implementation for employee business logic operations
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Initializes a new instance of the EmployeeService
        /// </summary>
        /// <param name="employeeRepository">Employee repository dependency</param>
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Retrieves all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        public List<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        /// <summary>
        /// Retrieves an employee by their unique identifier
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee entity or null if not found</returns>
        public Employee? GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves an employee by their email address
        /// </summary>
        /// <param name="email">Employee email address</param>
        /// <returns>Employee entity or null if not found</returns>
        public Employee? GetByEmail(string email)
        {
            return _employeeRepository.GetByEmail(email);
        }

        /// <summary>
        /// Registers a new employee with validation and password hashing
        /// </summary>
        /// <param name="employee">Employee entity to register</param>
        /// <param name="password">Plain text password to hash</param>
        /// <returns>Registration result with success status and any error messages</returns>
        public async Task<RegistrationResult> RegisterEmployeeAsync(Employee employee, string password)
        {
            // Validate input
            if (employee == null)
                return RegistrationResult.Failure("Employee information is required.");

            if (string.IsNullOrWhiteSpace(password))
                return RegistrationResult.Failure("Password is required.");

            if (string.IsNullOrWhiteSpace(employee.Email))
                return RegistrationResult.Failure("Email address is required.");

            // Validate employee type
            if (string.IsNullOrWhiteSpace(employee.EmployeeType))
                return RegistrationResult.Failure("Employee type is required.");

            var validEmployeeTypes = new[] { "Admin", "QuotationOfficer", "BookingOfficer", "WarehouseOfficer", "Manager", "CIO" };
            if (!validEmployeeTypes.Contains(employee.EmployeeType))
                return RegistrationResult.Failure("Invalid employee type. Must be one of: Admin, QuotationOfficer, BookingOfficer, WarehouseOfficer, Manager, CIO");

            // Check if employee already exists
            if (_employeeRepository.ExistsByEmail(employee.Email))
                return RegistrationResult.Failure("An employee with this email address already exists.");

            // Hash the password
            employee.PasswordHash = HashPassword(password);
            employee.CreatedDate = DateTime.UtcNow;
            employee.IsActive = true;

            // Add employee to repository
            _employeeRepository.Add(employee);

            // Simulate async operation
            await Task.Delay(1);

            return RegistrationResult.Success(employee);
        }

        /// <summary>
        /// Updates an existing employee
        /// </summary>
        /// <param name="employee">Employee entity with updated information</param>
        public void Update(Employee employee)
        {
            _employeeRepository.Update(employee);
        }

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        public void Delete(int id)
        {
            _employeeRepository.Delete(id);
        }

        /// <summary>
        /// Validates employee credentials for login
        /// </summary>
        /// <param name="email">Employee email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public bool ValidateCredentials(string email, string password)
        {
            var employee = _employeeRepository.GetByEmail(email);
            if (employee == null || !employee.IsActive)
                return false;

            return VerifyPassword(password, employee.PasswordHash);
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
