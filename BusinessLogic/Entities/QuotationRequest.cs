using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents a request for a shipping or freight quotation by a customer, including details and requirements.
    /// </summary>
    public class QuotationRequest
    {
        /// <summary>Database identifier for the quotation request.</summary>
        public int Id { get; set; }

        /// <summary>Internal string ID for the request, max length 50.</summary>
        [Required]
        [StringLength(50)]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>The ID of the customer making this request.</summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>Customer's full name when making the request.</summary>
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>Customer's email address for request-related notifications.</summary>
        [Required]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>Origin location for the shipment.</summary>
        [Required]
        [StringLength(100)]
        public string Source { get; set; } = string.Empty;

        /// <summary>Destination location for the shipment.</summary>
        [Required]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        /// <summary>Number of containers included in this quotation request.</summary>
        [Required]
        public int NumberOfContainers { get; set; }

        /// <summary>Description of the type of package for the request.</summary>
        [Required]
        [StringLength(100)]
        public string NatureOfPackage { get; set; } = string.Empty;

        /// <summary>Width of the package (in cm or specified unit).</summary>
        [Required]
        public decimal PackageWidth { get; set; }

        /// <summary>Height of the package (in cm or specified unit).</summary>
        [Required]
        public decimal PackageHeight { get; set; }

        /// <summary>Optional: Depth of the package (in cm or specified unit).</summary>
        public decimal? PackageDepth { get; set; }

        /// <summary>Indicates if this shipment is an import or export. Required.</summary>
        [Required]
        [StringLength(20)]
        public string ImportOrExport { get; set; } = string.Empty;

        /// <summary>Indicates packing or unpacking service. Required.</summary>
        [Required]
        [StringLength(20)]
        public string PackingOrUnpacking { get; set; } = string.Empty;

        /// <summary>Specifies whether quarantine services are required.</summary>
        public bool IsQuarantineRequired { get; set; }

        /// <summary>Details about the requested quarantine service (optional).</summary>
        [StringLength(500)]
        public string? QuarantineDetails { get; set; }

        /// <summary>Specifies whether fumigation services are required.</summary>
        public bool IsFumigationRequired { get; set; }

        /// <summary>Details about the requested fumigation service (optional).</summary>
        [StringLength(500)]
        public string? FumigationDetails { get; set; }

        /// <summary>Additional notes or requirements for the quotation request.</summary>
        [StringLength(1000)]
        public string? AdditionalRequirements { get; set; }

        /// <summary>Date/time the quotation request was created (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>Current status of the quotation request (e.g. Pending, Approved, Rejected).</summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
    }
}
