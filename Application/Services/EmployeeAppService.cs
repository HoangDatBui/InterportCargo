using InterportCargo.Application.Interfaces;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;

namespace InterportCargo.Application.Services
{
    /// <summary>
    /// Application service implementation for employee operations
    /// </summary>
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Initialises a new instance of the EmployeeAppService
        /// </summary>
        /// <param name="employeeService">Employee service dependency</param>
        public EmployeeAppService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Retrieves all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        public List<Employee> GetAllEmployees()
        {
            return _employeeService.GetAll();
        }

        /// <summary>
        /// Retrieves an employee by their unique identifier
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee entity or null if not found</returns>
        public Employee? GetEmployee(int id)
        {
            return _employeeService.GetById(id);
        }

        /// <summary>
        /// Retrieves an employee by their email address
        /// </summary>
        /// <param name="email">Employee email address</param>
        /// <returns>Employee entity or null if not found</returns>
        public Employee? GetEmployeeByEmail(string email)
        {
            return _employeeService.GetByEmail(email);
        }

        /// <summary>
        /// Registers a new employee
        /// </summary>
        /// <param name="firstName">Employee's first name</param>
        /// <param name="familyName">Employee's family name</param>
        /// <param name="email">Employee's email address</param>
        /// <param name="phoneNumber">Employee's phone number</param>
        /// <param name="employeeType">Employee's type</param>
        /// <param name="address">Employee's address</param>
        /// <param name="password">Employee's password</param>
        /// <returns>Registration result with success status and any error messages</returns>
        public async Task<RegistrationResult> RegisterEmployeeAsync(string firstName, string familyName, string email, 
            string phoneNumber, string employeeType, string address, string password)
        {
            // Create employee entity
            var employee = new Employee
            {
                FirstName = firstName?.Trim() ?? string.Empty,
                FamilyName = familyName?.Trim() ?? string.Empty,
                Email = email?.Trim() ?? string.Empty,
                PhoneNumber = phoneNumber?.Trim() ?? string.Empty,
                EmployeeType = employeeType?.Trim() ?? string.Empty,
                Address = address?.Trim() ?? string.Empty
            };

            // Validate required fields
            if (string.IsNullOrWhiteSpace(employee.FirstName))
                return RegistrationResult.Failure("First name is required.");

            if (string.IsNullOrWhiteSpace(employee.FamilyName))
                return RegistrationResult.Failure("Family name is required.");

            if (string.IsNullOrWhiteSpace(employee.Email))
                return RegistrationResult.Failure("Email address is required.");

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
                return RegistrationResult.Failure("Phone number is required.");

            if (string.IsNullOrWhiteSpace(employee.EmployeeType))
                return RegistrationResult.Failure("Employee type is required.");

            if (string.IsNullOrWhiteSpace(employee.Address))
                return RegistrationResult.Failure("Address is required.");

            if (string.IsNullOrWhiteSpace(password))
                return RegistrationResult.Failure("Password is required.");

            // Validate email format
            if (!IsValidEmail(employee.Email))
                return RegistrationResult.Failure("Invalid email format.");

            // Validate password strength
            if (password.Length < 6)
                return RegistrationResult.Failure("Password must be at least 6 characters long.");

            // Register employee through business service
            return await _employeeService.RegisterEmployeeAsync(employee, password);
        }

        /// <summary>
        /// Updates an existing employee
        /// </summary>
        /// <param name="employee">Employee entity with updated information</param>
        public void UpdateEmployee(Employee employee)
        {
            _employeeService.Update(employee);
        }

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        public void DeleteEmployee(int id)
        {
            _employeeService.Delete(id);
        }

        /// <summary>
        /// Validates employee credentials for login
        /// </summary>
        /// <param name="email">Employee email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public bool ValidateEmployeeCredentials(string email, string password)
        {
            return _employeeService.ValidateCredentials(email, password);
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if email format is valid, false otherwise</returns>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
