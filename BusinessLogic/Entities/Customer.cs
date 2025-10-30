using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents a customer of InterportCargo, including authentication, contact, and profile details.
    /// </summary>
    public class Customer
    {
        /// <summary>Internal database identifier for the customer.</summary>
        public int Id { get; set; }

        /// <summary>Customer's given (first) name. Required, max length 50.</summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Customer's family (last) name. Required, max length 50.</summary>
        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>Customer's unique email address. Required, email-validated, max length 100.</summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>Customer's phone contact. Required, max length 20.</summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>Optional company the customer belongs to. Max length 100.</summary>
        [StringLength(100)]
        public string? CompanyName { get; set; }

        /// <summary>Mailing or street address. Required, max length 500.</summary>
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Securely stored hash of the login password (never store plain text). Populated via registration/updates.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>UTC timestamp for when this account/profile was created.</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>Whether this customer account is active and allowed to log in/use services.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Convenience: the customer's full display name (FirstName + FamilyName).</summary>
        public string FullName => $"{FirstName} {FamilyName}";
    }
}
