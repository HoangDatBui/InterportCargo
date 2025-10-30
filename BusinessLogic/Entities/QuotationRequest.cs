using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    public class QuotationRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RequestId { get; set; } = string.Empty;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Source { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public int NumberOfContainers { get; set; }

        [Required]
        [StringLength(100)]
        public string NatureOfPackage { get; set; } = string.Empty;

        [Required]
        public decimal PackageWidth { get; set; }

        [Required]
        public decimal PackageHeight { get; set; }

        public decimal? PackageDepth { get; set; }

        [Required]
        [StringLength(20)]
        public string ImportOrExport { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PackingOrUnpacking { get; set; } = string.Empty;

        public bool IsQuarantineRequired { get; set; }

        [StringLength(500)]
        public string? QuarantineDetails { get; set; }

        public bool IsFumigationRequired { get; set; }

        [StringLength(500)]
        public string? FumigationDetails { get; set; }

        [StringLength(1000)]
        public string? AdditionalRequirements { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
    }
}
