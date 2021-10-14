using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ManagerRepository : GenericRepository<Manager>, IManagerRepository
    {
        private readonly HbaDbContext _context;

        public ManagerRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Manager> GetManagerStatistics(string managerId) 
        {
            var manager = await _context.Managers.Where(x => x.AppUserId == managerId).FirstOrDefaultAsync();
            return manager;
        }

        public async Task<Manager> GetManagerAsync(string managerId)
        {
           return await _context.Managers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUserId == managerId);
        }
    }
}
