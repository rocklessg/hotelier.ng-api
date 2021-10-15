using hotel_booking_dto.commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace hotel_booking_dto
{
    public class TransactionFilter : PagingDto
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string SearchQuery { get; set; }
        public TransactionFilter()
        {
            Year = DateTime.Now.Year.ToString();
        }
    }
}