using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class RoomTypeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Thumbnail { get; set; }
    }

    public class ReviewDTO
    {
        public string Text { get; set; }
        public string CustomerImage { get; set; }
        public string Date { get; set; }
    }
}
