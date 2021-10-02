using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_models;

namespace hotel_booking_data.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<Amenity> _amenities;
        private IGenericRepository<Booking> _bookings;
        private IGenericRepository<Customer> _customers;
        private IGenericRepository<Hotel> _hotels;
        private IGenericRepository<Manager> _managers;
        private IGenericRepository<Payment> _payments;
        private IGenericRepository<Rating> _ratings;
        private IGenericRepository<Review> _reviews;
        private IGenericRepository<Room> _rooms;
        private IGenericRepository<RoomType> _roomTypes;
        private IGenericRepository<WishList> _wishLists;
        private readonly HbaDbContext _context;

        public UnitOfWork(HbaDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<Amenity> Amenities => _amenities ??= new GenericRepository<Amenity>(_context);

        public IGenericRepository<Booking> Bookings => _bookings ??= new GenericRepository<Booking>(_context);

        public IGenericRepository<Customer> Customers => _customers ??= new GenericRepository<Customer>(_context);

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);

        public IGenericRepository<Manager> Managers => _managers ??= new GenericRepository<Manager>(_context);

        public IGenericRepository<Payment> Payments => _payments ??= new GenericRepository<Payment>(_context);

        public IGenericRepository<Rating> Ratings => _ratings ??= new GenericRepository<Rating>(_context);

        public IGenericRepository<Review> Reviews => _reviews ??= new GenericRepository<Review>(_context);

        public IGenericRepository<Room> Rooms => _rooms ??= new GenericRepository<Room>(_context);

        public IGenericRepository<RoomType> RoomTypes => _roomTypes ??= new GenericRepository<RoomType>(_context);

        public IGenericRepository<WishList> WishLists => _wishLists ??= new GenericRepository<WishList>(_context);
    }
}
