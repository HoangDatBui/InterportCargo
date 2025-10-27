using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents an employee entity in the InterportCargo system
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Unique identifier for the employee
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's first name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Employee's family name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>
        /// Employee's email address (used as username)
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Employee's phone number
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Employee's type (Admin, QuotationOfficer, BookingOfficer, WarehouseOfficer, Manager, CIO)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EmployeeType { get; set; } = string.Empty;

        /// <summary>
        /// Employee's address
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Employee's hashed password
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Date when the employee was registered
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Whether the employee account is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Full name property combining first and family name
        /// </summary>
        public string FullName => $"{FirstName} {FamilyName}";
    }
}
