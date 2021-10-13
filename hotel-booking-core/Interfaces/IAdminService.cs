using hotel_booking_dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAdminService
    {
        Task<Response<IEnumerable<TransactionResponseDto>>> GetAllTransactions();
    }
}
