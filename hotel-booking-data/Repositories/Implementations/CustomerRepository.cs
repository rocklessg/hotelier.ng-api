using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly  HbaDbContext _context;
        private readonly DbSet<Customer> _customer;
       

        public CustomerRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _customer = _context.Set<Customer>();
        }


        public async Task<bool> UpdateUserPhotoById(string customerId, string url)
        {
            var customer = await _customer.FindAsync(customerId);

            if (customer is not null)
            {
                customer.AppUser.Avatar= url;
                UpdateAsync(customer);
            }
            throw new ArithmeticException("Resource not found");
        }
    }
}

