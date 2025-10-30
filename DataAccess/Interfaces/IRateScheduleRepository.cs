using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for RateSchedule data access operations
    /// </summary>
    public interface IRateScheduleRepository
    {
        /// <summary>
        /// Get all rate schedule items
        /// </summary>
        /// <returns>List of all rate schedule items</returns>
        List<RateSchedule> GetAll();

        /// <summary>
        /// Get active rate schedule items only
        /// </summary>
        /// <returns>List of active rate schedule items</returns>
        List<RateSchedule> GetActiveRates();

        /// <summary>
        /// Get rate schedule item by ID
        /// </summary>
        /// <param name="id">Rate schedule item ID</param>
        /// <returns>Rate schedule item or null if not found</returns>
        RateSchedule? GetById(int id);

        /// <summary>
        /// Get rate for a specific service type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Rate schedule item or null if not found</returns>
        RateSchedule? GetByServiceType(string serviceType);

        /// <summary>
        /// Add a new rate schedule item
        /// </summary>
        /// <param name="rateSchedule">Rate schedule item to add</param>
        void Add(RateSchedule rateSchedule);

        /// <summary>
        /// Update an existing rate schedule item
        /// </summary>
        /// <param name="rateSchedule">Rate schedule item to update</param>
        void Update(RateSchedule rateSchedule);

        /// <summary>
        /// Delete a rate schedule item by ID
        /// </summary>
        /// <param name="id">Rate schedule item ID</param>
        void Delete(int id);
    }
}
