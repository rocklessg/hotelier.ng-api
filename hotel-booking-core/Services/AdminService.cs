using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<Response<IEnumerable<TransactionResponseDto>>> GetAllTransactions()
        {
            var transactions = await _unitOfWork.Booking.GetAllTransactions();
            var response = new Response<IEnumerable<TransactionResponseDto>>();
            var transactionList = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);
            

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Succeeded = true;
            response.Data = transactionList;
            response.Message = "All transactions retrieved successfully";
            return response;
        }
    }
}
