using System.Collections.Generic;

namespace HotelManagement.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        
        // Navigation property
        public ICollection<Room> Rooms { get; set; }
    }
}
