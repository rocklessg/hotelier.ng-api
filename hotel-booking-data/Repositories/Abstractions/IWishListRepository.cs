using hotel_booking_models;
using System.Linq;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IWishListRepository : IGenericRepository<WishList>
    {
        IQueryable<WishList> GetCustomerWishList(string customerId);
        WishList CheckWishList(string customerId, string hotelId);
    }
}
