﻿using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        IQueryable<Hotel> GetAllHotels();
        Task<Hotel> GetHotelEntitiesById(string id);
        Task<List<Rating>> HotelRatings(string hotelId);
        Hotel GetHotelByIdForAddAmenity(string id);
        Task<Hotel> GetHotelById(string hotelId);
        IQueryable<Hotel> GetHotelsByRating();
        IQueryable<Hotel> GetTopDeals();
        IQueryable<Review> GetAllReviewsByHotelAsync(string HotelId);
    }
}
