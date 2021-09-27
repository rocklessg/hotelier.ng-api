using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(User user);
    }
}