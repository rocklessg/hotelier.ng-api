using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ManagerStatistics : IManagerStatistics
    {
        private readonly IMapper _mapper;
        private readonly IManagerRepository _managerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ManagerStatistics(IMapper mapper, IManagerRepository managerRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _managerRepository = managerRepository;
            _unitOfWork = unitOfWork;
           
        }

        public async Task<ManagersStatisticsDto> GetManagerStatistics(string managersId) 
        {
            var manager = await _unitOfWork.Managers.GetManagerStatistics(managersId);
            //var manager = await db.Managers.Where(x => x.AppUserId == managersId).FirstOrDefaultAsync();

            if(manager != null) 
            {
                var managerStat = _mapper.Map<ManagersStatisticsDto>(manager);
                return managerStat;
            }

            throw new ArgumentException("This manager doesn't exist");
        }

        
    }
}
