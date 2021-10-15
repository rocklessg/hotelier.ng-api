using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<ManagerRequest> GetHotelManagerByEmail(string email)
        {
            var check = await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
            return check;
        }

        public async Task<ManagerRequest> GetHotelManagerByEmailToken(string email, string token)
        {
            var check = await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
            var checkToken = check.Token == token;
            return checkToken ? check : null;
        }
    }
}
