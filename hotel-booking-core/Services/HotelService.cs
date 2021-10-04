using hotel_booking_data.UnitOfWork.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HotelService
    {
        private readonly ILogger<HotelService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HotelService(ILogger<HotelService> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}
