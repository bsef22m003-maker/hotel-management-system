using System;
using System.Collections.Generic;

namespace HotelManagement.Models
{
    public enum RoomStatus
    {
        Available,
        Occupied,
        Maintenance,
        Reserved
    }

    public class Room
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; } = "";
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public RoomStatus Status { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public ICollection<Booking> Bookings { get; set; }
    }
}
