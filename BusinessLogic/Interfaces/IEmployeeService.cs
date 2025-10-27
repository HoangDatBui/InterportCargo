using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for employee business logic operations
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        List<Employee> GetAll();

        /// <summary>
        /// Retrieves an employee by their unique identifier
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee entity or null if not found</returns>
        Employee? GetById(int id);

        /// <summary>
        /// Retrieves an employee by their email address
        /// </summary>
        /// <param name="email">Employee email address</param>
        /// <returns>Employee entity or null if not found</returns>
        Employee? GetByEmail(string email);

        /// <summary>
        /// Registers a new employee with validation and password hashing
        /// </summary>
        /// <param name="employee">Employee entity to register</param>
        /// <param name="password">Plain text password to hash</param>
        /// <returns>Registration result with success status and any error messages</returns>
        Task<RegistrationResult> RegisterEmployeeAsync(Employee employee, string password);

        /// <summary>
        /// Updates an existing employee
        /// </summary>
        /// <param name="employee">Employee entity with updated information</param>
        void Update(Employee employee);

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        void Delete(int id);

        /// <summary>
        /// Validates employee credentials for login
        /// </summary>
        /// <param name="email">Employee email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        bool ValidateCredentials(string email, string password);
    }
}
