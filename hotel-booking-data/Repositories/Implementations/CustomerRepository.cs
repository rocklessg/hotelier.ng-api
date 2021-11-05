using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using hotel_booking_data.Repositories.Abstractions;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Linq;
using hotel_booking_utilities.Pagination;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.commons;

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

        public IQueryable<Customer> GetAllUsers()
        {
           return _customers.Include(x => x.AppUser);
        }

        public IQueryable<WishList> GetCustomerHotelsAsync(string customerId)
        {
            var customerHotels = _context.WishLists.Where(x => x.CustomerId == customerId)
                .Include(x => x.Hotel);
            return customerHotels;
        }
    }
}

