using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer GetCustomer(string id);
        IEnumerable<Customer> GetAllUsers(Expression<Func<Customer, bool>> expression = null, List<string> includes = null, Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderby = null);
    }
}