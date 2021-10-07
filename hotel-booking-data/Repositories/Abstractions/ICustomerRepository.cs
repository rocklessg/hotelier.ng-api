using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ICustomerRepository
    {
        Task<Customer> FindAsync(string customerId);
        void Update(Customer customer);
        bool UpdateUserPhotoById(Customer customer);
    }
}