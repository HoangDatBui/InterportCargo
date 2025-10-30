using System.ComponentModel.DataAnnotations;

namespace InterportCargo.BusinessLogic.Entities
{
    /// <summary>
    /// Represents a response to a quotation request, including status, officer/customer info, and related metadata.
    /// </summary>
    public class QuotationResponse
    {
        /// <summary>Database identifier for the quotation response.</summary>
        public int Id { get; set; }

        /// <summary>The related quotation request ID which this response addresses. Required.</summary>
        [Required]
        public int QuotationRequestId { get; set; }

        /// <summary>The ID of the officer handling the response (optional).</summary>
        public int? OfficerId { get; set; }

        /// <summary>The name of the officer handling the response (optional, max 100).</summary>
        [StringLength(100)]
        public string? OfficerName { get; set; }

        /// <summary>The ID of the customer (optional).</summary>
        public int? CustomerId { get; set; }

        /// <summary>The name of the customer (optional, max 100).</summary>
        [StringLength(100)]
        public string? CustomerName { get; set; }

        /// <summary>The type of response: e.g. 'Officer' or other. Required, max 20.</summary>
        [Required]
        [StringLength(20)]
        public string ResponseType { get; set; } = "Officer";

        /// <summary>Status string for this response: e.g. 'Sent', 'Viewed'. Required, max 20.</summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        /// <summary>Unique quotation number allocated for this response (optional, max 50).</summary>
        [StringLength(50)]
        public string? QuotationNumber { get; set; }

        /// <summary>Rich message or note sent with the response (optional, max 1000).</summary>
        [StringLength(1000)]
        public string? Message { get; set; }

        /// <summary>UTC timestamp indicating when the response was created.</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>Indicates whether the response has been read by its recipient(s).</summary>
        public bool IsRead { get; set; } = false;

        /// <summary>The related QuotationRequest entity (navigation property, optional).</summary>
        public QuotationRequest? QuotationRequest { get; set; }
    }
}
