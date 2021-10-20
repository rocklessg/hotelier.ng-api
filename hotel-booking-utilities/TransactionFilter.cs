using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public class TransactionFilter
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
