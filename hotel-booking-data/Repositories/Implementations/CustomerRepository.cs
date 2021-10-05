using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
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


        public async Task<Customer> FindAsync(string customerId)
        {
            Customer customer = await _customers.FindAsync(customerId);
            return customer;
        }

    }
}
