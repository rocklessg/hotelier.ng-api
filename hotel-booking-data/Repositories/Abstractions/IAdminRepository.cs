using hotel_booking_dto;
using hotel_booking_models;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IAdminRepository : IGenericRepository<AppUser>
    {
        IQueryable<Booking> GetAllTransactions(TransactionFilter filter);
        //IQueryable<Booking> GetTransactionsByQuery(TransactionFilter filter);
        //IQueryable<Booking> GetTransactionsFilterByDate(TransactionFilter filter);
        //IQueryable<Booking> GetTransactionsFilterByDate(string year);
        //IQueryable<Booking> GetTransactionsByHotelAndMonth(TransactionFilter filter);
       Task<Manager> GetManagerById(string id);
    }
}
