using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;

namespace InterportCargo.BusinessLogic.Services
{
    /// <summary>
    /// Service for calculating discounts based on business rules
    /// </summary>
    public class DiscountService : IDiscountService
    {
        /// <summary>
        /// Calculate discount percentage based on quotation request criteria
        /// </summary>
        /// <param name="quotationRequest">The quotation request</param>
        /// <returns>Discount percentage (0-10%)</returns>
        public decimal CalculateDiscount(QuotationRequest quotationRequest)
        {
            // Rule 1: 2.5% discount if containers > 5 AND (Quarantine OR Fumigation)
            // Rule 2: 5% discount if containers > 5 AND (Quarantine AND Fumigation)
            // Rule 3: 10% discount if containers > 10 AND (Quarantine AND Fumigation)

            var numberOfContainers = quotationRequest.NumberOfContainers;
            var hasQuarantine = quotationRequest.IsQuarantineRequired;
            var hasFumigation = quotationRequest.IsFumigationRequired;

            // Check Rule 3: 10% discount
            if (numberOfContainers > 10 && hasQuarantine && hasFumigation)
            {
                return 10.0m;
            }

            // Check Rule 2: 5% discount
            if (numberOfContainers > 5 && hasQuarantine && hasFumigation)
            {
                return 5.0m;
            }

            // Check Rule 1: 2.5% discount
            if (numberOfContainers > 5 && (hasQuarantine || hasFumigation))
            {
                return 2.5m;
            }

            // No discount
            return 0m;
        }

        /// <summary>
        /// Calculate discount amount based on subtotal and percentage
        /// </summary>
        /// <param name="subtotal">Subtotal before discount</param>
        /// <param name="discountPercentage">Discount percentage</param>
        /// <returns>Discount amount</returns>
        public decimal CalculateDiscountAmount(decimal subtotal, decimal discountPercentage)
        {
            return subtotal * (discountPercentage / 100);
        }
    }
}

