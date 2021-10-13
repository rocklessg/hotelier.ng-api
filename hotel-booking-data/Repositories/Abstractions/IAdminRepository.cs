using hotel_booking_models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IAdminRepository : IGenericRepository<AppUser>
    {
        Task<IEnumerable<Booking>> GetAllTransactions();
    }
}
