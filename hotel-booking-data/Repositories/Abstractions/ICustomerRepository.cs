using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public interface ICustomerRepository
    {
        Task<Customer> FindAsync(string customerId);
    }
}