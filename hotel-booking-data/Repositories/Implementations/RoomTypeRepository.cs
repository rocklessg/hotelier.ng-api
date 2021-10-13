﻿using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<RoomType> _dbSet;

        public RoomTypeRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<RoomType>();
        }


        public IQueryable<RoomType> GetRoomByPrice(decimal minPrice, decimal maxPrice)
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(rt => rt.Hotel);
            query = query.Where(rt => (!(maxPrice > minPrice) ? rt.Price >= minPrice
                                        : (rt.Price >= minPrice) && (rt.Price <= maxPrice)));
            query = query.OrderBy(rt => rt.Price);
            return query;
        }
        public async Task<IEnumerable<RoomType>> GetTopDealsAsync()
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(rt => rt.Hotel);
            query = query.OrderByDescending(rt => rt.Discount / rt.Price);
            query = query.Take(5);
            return await query.ToListAsync();
        }
        public async Task<List<RoomType>> GetRoomTypesInEachHotel(string hotelId)
        {
            var rooms = await _context.RoomTypes.Where(x => x.HotelId == hotelId).ToListAsync();

            return rooms;
        }



    }
}
