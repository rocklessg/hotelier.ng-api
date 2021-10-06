using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto
{
    /// <summary>
    /// DTO model for an Hotel update request.
    /// </summary>
    public class UpdateHotelDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
    }
}
