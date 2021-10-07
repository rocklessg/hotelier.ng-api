using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class CustomerRepository : GenericRepository<AppUser>, IAppUserRepository
    {

        private readonly HbaDbContext _context;
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _customers = _context.Set<Customer>();
        }


     //


        public async Task<Customer> FindAsync(string customerId)
        {
            Customer customer = await _customers.FindAsync(customerId);
            return customer;
        }

    }
}

