using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for employee data access operations
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Retrieves all employees from the data store
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
        /// Adds a new employee to the data store
        /// </summary>
        /// <param name="employee">Employee entity to add</param>
        void Add(Employee employee);

        /// <summary>
        /// Updates an existing employee in the data store
        /// </summary>
        /// <param name="employee">Employee entity with updated information</param>
        void Update(Employee employee);

        /// <summary>
        /// Deletes an employee from the data store
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        void Delete(int id);

        /// <summary>
        /// Checks if an employee with the given email already exists
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <returns>True if employee exists, false otherwise</returns>
        bool ExistsByEmail(string email);
    }
}
