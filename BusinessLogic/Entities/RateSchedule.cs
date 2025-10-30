using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents the rate schedule for different service types, including rates for 20ft and 40ft containers.
    /// </summary>
    public class RateSchedule
    {
        /// <summary>Database identifier for the rate schedule entry.</summary>
        public int Id { get; set; }

        /// <summary>The service type (e.g., shipping, warehousing). Required, max length 100.</summary>
        [Required]
        [StringLength(100)]
        public string ServiceType { get; set; } = string.Empty;

        /// <summary>Rate for 20-foot containers. Required.</summary>
        [Required]
        public decimal Rate20Feet { get; set; }

        /// <summary>Rate for 40-foot containers. Required.</summary>
        [Required]
        public decimal Rate40Feet { get; set; }

        /// <summary>Optional description for this rate schedule.</summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>Timestamp for when this rate schedule was created (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>Whether this rate schedule is currently active.</summary>
        public bool IsActive { get; set; } = true;
    }
}
