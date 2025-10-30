using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    public class QuotationResponse
    {
        public int Id { get; set; }

        [Required]
        public int QuotationRequestId { get; set; }

        public int? OfficerId { get; set; }

        [StringLength(100)]
        public string? OfficerName { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(100)]
        public string? CustomerName { get; set; }

        [Required]
        [StringLength(20)]
        public string ResponseType { get; set; } = "Officer";

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [StringLength(50)]
        public string? QuotationNumber { get; set; }

        [StringLength(1000)]
        public string? Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public QuotationRequest? QuotationRequest { get; set; }
    }
}
