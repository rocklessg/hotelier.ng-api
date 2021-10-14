using AutoMapper;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ManagerRepository : GenericRepository<Manager>, IManagerRepository
    {
        private readonly HbaDbContext _context;
       
        public ManagerRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            
        }

        public async Task<Manager> GetManagerStatistics(string managerId) 
        {
            var manager = await _context.Managers.Where(x => x.AppUserId == managerId).FirstOrDefaultAsync();
            return manager;
        }

        public async Task<bool> AddManagerAsync(ManagerDto entity)
        {
                var manager = await _context.Managers.Where(x => x.AppUserId == entity.AppUserId)
                    .FirstOrDefaultAsync();
            return true; 
        }
    }
    
}
