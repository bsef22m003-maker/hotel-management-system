using System;
using System.Collections.Generic;

namespace HotelManagement.Models
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        CheckedIn,
        CheckedOut,
        Cancelled
    }

    public class Booking
    {
        public int BookingID { get; set; }
        public int GuestID { get; set; }
        public int RoomID { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string? SpecialRequests { get; set; }
        
        // Foreign keys
        public Guest Guest { get; set; }
        public Room Room { get; set; }
        
        // Navigation property
        public ICollection<Payment> Payments { get; set; }
    }
}
