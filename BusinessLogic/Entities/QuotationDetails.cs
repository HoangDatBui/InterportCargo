using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    public class QuotationDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string QuotationNumber { get; set; } = string.Empty;

        [Required]
        public int QuotationRequestId { get; set; }

        public QuotationRequest? QuotationRequest { get; set; }

        [Required]
        public int OfficerId { get; set; }

        [Required]
        [StringLength(100)]
        public string OfficerName { get; set; } = string.Empty;

        [Required]
        public DateTime DateIssued { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string ContainerType { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Scope { get; set; } = string.Empty;

        [Required]
        public decimal Subtotal { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        [Required]
        public decimal AmountAfterDiscount { get; set; }

        [Required]
        public decimal GST { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [StringLength(2000)]
        public string? ItemizedCharges { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
