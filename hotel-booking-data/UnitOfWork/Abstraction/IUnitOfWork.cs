using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;

namespace hotel_booking_data.UnitOfWork.Abstraction
{
    public interface IUnitOfWork
    {
        IGenericRepository<Amenity> Amenities { get; }
        IGenericRepository<Booking> Bookings { get; }
        IGenericRepository<Customer> Customers { get; }
        IGenericRepository<Hotel> Hotels { get; }
        IGenericRepository<Manager> Managers { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Rating> Ratings { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<Room> Rooms { get; }
        IGenericRepository<RoomType> RoomTypes { get; }
        IGenericRepository<WishList> WishLists { get; }
    }
}
