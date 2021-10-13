using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using hotel_booking_data.Repositories.Abstractions;

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
    }
}

