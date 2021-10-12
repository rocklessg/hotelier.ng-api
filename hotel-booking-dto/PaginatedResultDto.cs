using hotel_booking_dto.commons;
using System.Collections.Generic;

namespace hotel_booking_dto
{
    public class PaginatedResultDto<T>
    {
        public Paging PageMetaData { get; set; }

        public IEnumerable<T> ResponseData { get; set; } = new List<T>();
    }
}
