using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;

namespace InterportCargo.Application.Interfaces
{
    /// <summary>
    /// Application service interface for employee operations
    /// </summary>
    public interface IEmployeeAppService
    {
        /// <summary>
        /// Retrieves all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        List<Employee> GetAllEmployees();

        /// <summary>
        /// Retrieves an employee by their unique identifier
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee entity or null if not found</returns>
        Employee? GetEmployee(int id);

        /// <summary>
        /// Retrieves an employee by their email address
        /// </summary>
        /// <param name="email">Employee email address</param>
        /// <returns>Employee entity or null if not found</returns>
        Employee? GetEmployeeByEmail(string email);

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
        Task<RegistrationResult> RegisterEmployeeAsync(string firstName, string familyName, string email, 
            string phoneNumber, string employeeType, string address, string password);

        /// <summary>
        /// Updates an existing employee
        /// </summary>
        /// <param name="employee">Employee entity with updated information</param>
        void UpdateEmployee(Employee employee);

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        void DeleteEmployee(int id);

        /// <summary>
        /// Validates employee credentials for login
        /// </summary>
        /// <param name="email">Employee email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        bool ValidateEmployeeCredentials(string email, string password);
    }
}
