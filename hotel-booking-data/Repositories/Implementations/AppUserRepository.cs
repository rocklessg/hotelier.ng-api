using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using hotel_booking_data.Repositories.Abstractions;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {

        private readonly HbaDbContext _context;
        private readonly DbSet<Customer> _customers;

        public AppUserRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _customers = _context.Set<Customer>();
        }


<<<<<<< HEAD:hotel-booking-data/Repositories/Implementations/CustomerRepository.cs
        public bool UpdateUserPhotoById(Customer customer)
        {

            if (customer is not null)
            {
                Update(customer);
                return true;
            }
            return false;
        }
=======
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10:hotel-booking-data/Repositories/Implementations/AppUserRepository.cs


        public async Task<Customer> FindAsync(string customerId)
        {
            Customer customer = await _customers.FindAsync(customerId);
            return customer;
        }

        public void Update(Customer customer)
        {
            _customers.Update(customer);
        }

    }
}

