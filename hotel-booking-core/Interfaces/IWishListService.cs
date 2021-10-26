using hotel_booking_dto;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IWishListService
    {
        Task<Response<string>> AddWishList(string hotelId, string customerId);
        Task<Response<string>> RemoveWishListItem(string hotelId, string customerId);
        Task<Response<string>> ClearWishList(string customerId);
    }
}
