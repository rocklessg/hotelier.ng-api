using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IAppUserRepository
    {
        Task<Customer> FindAsync(string customerId);
<<<<<<< HEAD:hotel-booking-data/Repositories/Abstractions/ICustomerRepository.cs
        void Update(Customer customer);
        bool UpdateUserPhotoById(Customer customer);
=======
        bool UpdateUserPhotoById(AppUser customer);
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10:hotel-booking-data/Repositories/Abstractions/IAppUserRepository.cs
    }
}