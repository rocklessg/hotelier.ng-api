using hotel_booking_models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IManagerRepository : IGenericRepository<Manager>
    {
        Task<Manager> GetManagerStatistics(string managerId);
        Task<Manager> GetManagerAsync(string managerId);
        Task<Manager> GetManagerByHotelsAsync(string managerId);
        Task<IEnumerable<Hotel>> GetAllHotelsForManagerAsync(string managerId);
    }
}
