using hotel_booking_data.UnitOfWork.Abstraction;
<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
=======
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
>>>>>>> d919bd7932422fe350c8a4536f4b6ce5e33423fb

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RoomController(ILogger<RoomController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}
