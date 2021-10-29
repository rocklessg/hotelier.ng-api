using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {

        private readonly HbaDbContext _context;
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _customers = _context.Set<Customer>();
        }

        public Customer GetCustomer(string id)
        {
            Customer customer = _customers.Find(id);
            return customer;
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            return await _customers
                .Include(x => x.AppUser)
                .Include(x => x.Bookings)
                .FirstOrDefaultAsync(x => x.AppUserId == id);
        }

        public IQueryable<Customer> GetAllUsers()
        {
            return _customers.Include(x => x.AppUser);
        }

        public async Task<IEnumerable<Customer>> GetTopCustomerForManagerAsync(string managerId)
        {
            var query = _customers.AsNoTracking()
                            .Include(c => c.AppUser)
                            .Include(c => c.Bookings).ThenInclude(bk => bk.Hotel)
                            .Include(c => c.Bookings).ThenInclude(bk => bk.Payment)
                            .Where(c => c.Bookings.Where(bk => bk.Hotel.ManagerId == managerId).Count() > 0);
            if (query.Count() == 0)
            {
                return null;
            }
            var topMoneySpenders = await query.OrderByDescending(c => c.Bookings.Where(x => x.Hotel.ManagerId == managerId && x.PaymentStatus == true)
                                              .Sum(bk => bk.Payment.Amount)).Take(3).ToListAsync();
            var topFrequentUsers = await query.OrderByDescending(c => c.Bookings.Where(x => x.Hotel.ManagerId == managerId && x.PaymentStatus == true).Count()).ToListAsync();
            var list = new List<Customer>(topMoneySpenders);
            for(int i = 0; i < topFrequentUsers.Count; i++)
            {
                if(list.Count( x => x.AppUserId == topFrequentUsers[i].AppUserId) == 0)
                {
                    list.Add(topFrequentUsers[i]);
                }
            }
            return list.OrderByDescending(c => c.Bookings.Sum(x => x.Payment.Amount));
        }
        public async Task<Customer> GetCustomerDetails(string id)
        {
            return await _customers.Where(c => c.AppUserId == id)
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync();
        }
    }
}

