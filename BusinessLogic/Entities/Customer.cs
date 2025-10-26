using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents a customer entity in the InterportCargo system
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unique identifier for the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Customer's first name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Customer's family name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>
        /// Customer's email address (used as username)
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Customer's phone number
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Customer's company name (optional)
        /// </summary>
        [StringLength(100)]
        public string? CompanyName { get; set; }

        /// <summary>
        /// Customer's address
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Customer's hashed password
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Date when the customer was registered
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Whether the customer account is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Full name property combining first and family name
        /// </summary>
        public string FullName => $"{FirstName} {FamilyName}";
    }
}
