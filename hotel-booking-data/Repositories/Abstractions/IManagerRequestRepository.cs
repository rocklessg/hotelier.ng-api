using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IManagerRequestRepository : IGenericRepository<ManagerRequest>
    {
        Task<ManagerRequest> GetHotelManager(string email);
    }
}
