using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for discount calculation
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Calculate discount percentage based on quotation request criteria
        /// </summary>
        /// <param name="quotationRequest">The quotation request</param>
        /// <returns>Discount percentage (0-10%)</returns>
        decimal CalculateDiscount(QuotationRequest quotationRequest);

        /// <summary>
        /// Calculate discount amount based on subtotal and percentage
        /// </summary>
        /// <param name="subtotal">Subtotal before discount</param>
        /// <param name="discountPercentage">Discount percentage</param>
        /// <returns>Discount amount</returns>
        decimal CalculateDiscountAmount(decimal subtotal, decimal discountPercentage);
    }
}

