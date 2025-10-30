using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Data;
using InterportCargo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.DataAccess.Repositories
{
    /// <summary>
    /// Entity Framework implementation of the RateSchedule repository
    /// </summary>
    public class EFRateScheduleRepository : IRateScheduleRepository
    {
        private readonly InterportCargoDbContext _context;

        /// <summary>
        /// Initialises a new instance of the EFRateScheduleRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public EFRateScheduleRepository(InterportCargoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all rate schedule items
        /// </summary>
        /// <returns>List of all rate schedule items</returns>
        public List<RateSchedule> GetAll()
        {
            return _context.RateSchedules.ToList();
        }

        /// <summary>
        /// Get active rate schedule items only
        /// </summary>
        /// <returns>List of active rate schedule items</returns>
        public List<RateSchedule> GetActiveRates()
        {
            return _context.RateSchedules
                .Where(r => r.IsActive)
                .ToList();
        }

        /// <summary>
        /// Get rate schedule item by ID
        /// </summary>
        /// <param name="id">Rate schedule item ID</param>
        /// <returns>Rate schedule item or null if not found</returns>
        public RateSchedule? GetById(int id)
        {
            return _context.RateSchedules.Find(id);
        }

        /// <summary>
        /// Get rate for a specific service type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Rate schedule item or null if not found</returns>
        public RateSchedule? GetByServiceType(string serviceType)
        {
            return _context.RateSchedules
                .FirstOrDefault(r => r.ServiceType == serviceType && r.IsActive);
        }

        /// <summary>
        /// Add a new rate schedule item
        /// </summary>
        /// <param name="rateSchedule">Rate schedule item to add</param>
        public void Add(RateSchedule rateSchedule)
        {
            _context.RateSchedules.Add(rateSchedule);
            _context.SaveChanges();
        }

        /// <summary>
        /// Update an existing rate schedule item
        /// </summary>
        /// <param name="rateSchedule">Rate schedule item to update</param>
        public void Update(RateSchedule rateSchedule)
        {
            _context.Entry(rateSchedule).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete a rate schedule item by ID
        /// </summary>
        /// <param name="id">Rate schedule item ID</param>
        public void Delete(int id)
        {
            var rateSchedule = _context.RateSchedules.Find(id);
            if (rateSchedule != null)
            {
                _context.RateSchedules.Remove(rateSchedule);
                _context.SaveChanges();
            }
        }
    }
}
