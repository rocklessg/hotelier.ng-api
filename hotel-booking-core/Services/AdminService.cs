using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

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
        public async Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetManagerTransactionsAsync(string managerId, TransactionFilter filter)
        {
            var manager = await _unitOfWork.Managers.GetManagerByHotelsAsync(managerId);
            var response = new Response<PageResult<IEnumerable<TransactionResponseDto>>>();
            IQueryable<Booking> managerBookings;

            if (manager != null)
            {
                if (manager.Hotels != null)
                {
                    if (filter.Month != null && filter.SearchQuery == null)
                    {
                        managerBookings = _unitOfWork.Booking.GetManagerBookingsFilterByDate(managerId, filter);                        
                    }
                    else if (filter.Month == null && filter.SearchQuery == null)
                    {
                        managerBookings = _unitOfWork.Booking.GetManagerBookingsFilterByDate(managerId, filter.Year);
                    }
                    else if (filter.Month == null && filter.SearchQuery != null)
                    {
                        managerBookings = _unitOfWork.Booking.GetManagerBookingsSearchByHotel(managerId, filter);
                    }                   
                    else if (filter.Month != null && filter.SearchQuery != null)
                    {
                        managerBookings = _unitOfWork.Booking.GetManagerBookingsByHotelAndMonth(managerId, filter);
                    }
                    else
                    {
                        managerBookings = _unitOfWork.Booking.GetManagerBookings(managerId);
                    };

                    var transactionList = await managerBookings.PaginationAsync<Booking, TransactionResponseDto>(filter.PageSize, filter.PageNumber, _mapper);
                    var message = "";
                    if (transactionList.PageItems.Any() == false)
                    {
                        message = $"No transaction found.";
                    }
                    else
                    {
                        message = $"Above are the transaction found.";
                    }
                    response.Message = message;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Succeeded = true;
                    response.Data = transactionList;                    
                    return response;

                }
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = $"Manager with id {managerId} has no transactions!";
                response.Succeeded = true;
                response.Data = default;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Succeeded = false;
            response.Data = default;
            response.Message = $"Manager with Id = {managerId} not found";
            return response;
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
