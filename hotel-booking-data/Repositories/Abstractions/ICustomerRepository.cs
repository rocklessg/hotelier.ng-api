using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer GetCustomer(string id);

        Task<Customer> GetCustomerAsync(string id);
      
    }
}