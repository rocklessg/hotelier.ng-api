using System.Collections.Generic;

namespace hotel_booking_models
{
    public class Manager
    {
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string State { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public User User { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
    }
}