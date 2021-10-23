using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using System.Collections.Generic;
using hotel_booking_utilities.Pagination;
using System.Net;
using hotel_booking_models;
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



        public async Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetAllTransactions(TransactionFilter filter)
        {
            var transactions = _unitOfWork.Transactions.GetAllTransactions(filter);
            var item = await transactions.PaginationAsync<Booking, TransactionResponseDto>(filter.PageSize, filter.PageNumber, _mapper);
            return new Response<PageResult<IEnumerable<TransactionResponseDto>>>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Succeeded = true,
                Data = item,
                Message = "All transactions retrieved successfully"
            };
        }

        
    }
}
