using InterportCargo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using InterportCargo.DataAccess.Data;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Repositories
{
    /// <summary>
    /// Entity Framework Core implementation of employee repository
    /// </summary>
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private readonly InterportCargoDbContext _context;

        /// <summary>
        /// Initialises a new instance of the EFEmployeeRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public EFEmployeeRepository(InterportCargoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all employees from the database
        /// </summary>
        /// <returns>List of all employees</returns>
        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        /// <summary>
        /// Retrieves an employee by their unique identifier
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee entity or null if not found</returns>
        public Employee? GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        /// <summary>
        /// Retrieves an employee by their email address
        /// </summary>
        /// <param name="email">Employee email address</param>
        /// <returns>Employee entity or null if not found</returns>
        public Employee? GetByEmail(string email)
        {
            return _context.Employees
                .FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Adds a new employee to the database
        /// </summary>
        /// <param name="employee">Employee entity to add</param>
        public void Add(Employee employee)
        {
            employee.CreatedDate = DateTime.UtcNow;
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing employee in the database
        /// </summary>
        /// <param name="employee">Employee entity with updated information</param>
        public void Update(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes an employee from the database
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        public void Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Checks if an employee with the given email already exists
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <returns>True if employee exists, false otherwise</returns>
        public bool ExistsByEmail(string email)
        {
            return _context.Employees
                .Any(e => e.Email.ToLower() == email.ToLower());
        }
    }
}
