using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ManagerRequestRepository : GenericRepository<ManagerRequest>, IManagerRequestRepository
    {
        private readonly DbSet<ManagerRequest> _dbSet;
        private readonly HbaDbContext _context;
        public ManagerRequestRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<ManagerRequest>();
        }
    }
}
