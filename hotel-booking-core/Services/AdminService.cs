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
            var transactions = _unitOfWork.Booking.GetAllTransactions();
            var response = new Response<PageResult<IEnumerable<TransactionResponseDto>>>();

            if (filter.Month != null && filter.SearchQuery == null)
            {
                transactions = _unitOfWork.Booking.GetTransactionsFilterByDate(filter);         
            }
            else if (filter.Month == null && filter.SearchQuery == null)
            {
                transactions = _unitOfWork.Booking.GetTransactionsFilterByDate(filter.Year);
            }
            else if (filter.Month == null && filter.SearchQuery != null)
            {
                transactions = _unitOfWork.Booking.GetTransactionsByQuery(filter);
            }
            else if (filter.Month != null && filter.SearchQuery != null)
            {
                transactions = _unitOfWork.Booking.GetTransactionsByHotelAndMonth(filter);
            }
            else
            {
                transactions = _unitOfWork.Booking.GetAllTransactions();
            };
            var item = await transactions.PaginationAsync<Booking, TransactionResponseDto>(filter.PageSize, filter.PageNumber, _mapper);


            response.StatusCode = (int)HttpStatusCode.OK;
            response.Succeeded = true;
            response.Data = item;
            response.Message = "All transactions retrieved successfully";
            return response;
        }
    }
}
