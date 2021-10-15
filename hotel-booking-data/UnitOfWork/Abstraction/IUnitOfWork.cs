using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_models;
using System;
using System.Threading.Tasks;

namespace hotel_booking_data.UnitOfWork.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        IAmenityRepository Amenities { get; }
        ICustomerRepository Customers { get; }
        IHotelRepository Hotels { get; }
        IManagerRepository Managers { get; }
        IPaymentRepository Payments { get; }
        IRoomRepository Rooms { get; }
        IWishListRepository WishLists { get; }
        IRoomTypeRepository RoomType { get; }
        
        Task Save();
    }
}
