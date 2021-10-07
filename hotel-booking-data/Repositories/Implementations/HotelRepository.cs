using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HbaDbContext _context;

        public HotelRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public Hotel GetHotelByIdForAddAmenity(string id)
        {
           return  _context.Hotels.FirstOrDefault(x => x.Id == id);
            
        }
    }
}
