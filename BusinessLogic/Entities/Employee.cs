using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents an employee of InterportCargo, including authentication, contact, and role details.
    /// </summary>
    public class Employee
    {
        /// <summary>Internal database identifier for the employee.</summary>
        public int Id { get; set; }

        /// <summary>Employee's given (first) name. Required, max length 50.</summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Employee's family (last) name. Required, max length 50.</summary>
        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>Employee's unique email address. Required, email-validated, max length 100.</summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>Employee's phone contact. Required, max length 20.</summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>Employee's type/role. Required, max length 50.</summary>
        [Required]
        [StringLength(50)]
        public string EmployeeType { get; set; } = string.Empty;

        /// <summary>Mailing or street address. Required, max length 500.</summary>
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>Securely stored hash of the login password for the employee.</summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>UTC timestamp for when this employee profile was created.</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>Whether this employee account is active and allowed to log in/use services.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Convenience: the employee's full display name (FirstName + FamilyName).</summary>
        public string FullName => $"{FirstName} {FamilyName}";
    }
}
