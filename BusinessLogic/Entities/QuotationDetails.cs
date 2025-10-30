using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents the finalised details of a quotation, including pricing, officers, discounts, and itemised charges.
    /// </summary>
    public class QuotationDetails
    {
        /// <summary>Database identifier for the quotation details.</summary>
        public int Id { get; set; }

        /// <summary>Unique quotation number for this quotation. Required, max 50.</summary>
        [Required]
        [StringLength(50)]
        public string QuotationNumber { get; set; } = string.Empty;

        /// <summary>The related quotation request ID. Required.</summary>
        [Required]
        public int QuotationRequestId { get; set; }

        /// <summary>The related quotation request entity (navigation property, optional).</summary>
        public QuotationRequest? QuotationRequest { get; set; }

        /// <summary>ID of the officer who issued the quotation. Required.</summary>
        [Required]
        public int OfficerId { get; set; }

        /// <summary>Name of the officer who issued the quotation. Required, max 100.</summary>
        [Required]
        [StringLength(100)]
        public string OfficerName { get; set; } = string.Empty;

        /// <summary>The date the quotation was issued. Required.</summary>
        [Required]
        public DateTime DateIssued { get; set; } = DateTime.UtcNow;

        /// <summary>Type of container (e.g., 20ft/40ft/etc). Required, max 20.</summary>
        [Required]
        [StringLength(20)]
        public string ContainerType { get; set; } = string.Empty;

        /// <summary>Scope/description of the quotation services. Required, max 1000.</summary>
        [Required]
        [StringLength(1000)]
        public string Scope { get; set; } = string.Empty;

        /// <summary>Subtotal price before discounts and GST. Required.</summary>
        [Required]
        public decimal Subtotal { get; set; }

        /// <summary>The discount percentage applied (if any).</summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>The discount amount applied (if any).</summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>The amount after applying discounts, before GST. Required.</summary>
        [Required]
        public decimal AmountAfterDiscount { get; set; }

        /// <summary>GST amount charged. Required.</summary>
        [Required]
        public decimal GST { get; set; }

        /// <summary>The total final amount charged (after all discounts and taxes). Required.</summary>
        [Required]
        public decimal TotalAmount { get; set; }

        /// <summary>List or JSON/string blob of itemised charges for the quotation (optional, max 2000).</summary>
        [StringLength(2000)]
        public string? ItemizedCharges { get; set; }

        /// <summary>Status string for this quotation (e.g. 'Pending', 'Issued'). Required, max 20.</summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        /// <summary>Timestamp indicating when the quotation was created (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
