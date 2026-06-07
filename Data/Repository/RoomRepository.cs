using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Models;
using HotelManagement.Data.Repository;

namespace HotelManagement.Data.Repository
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
        Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomTypeId);
        Task<IEnumerable<Room>> GetAvailableRoomsByDateRangeAsync(DateTime checkInDate, DateTime checkOutDate);
        Task<IEnumerable<Room>> GetRoomsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<Room> GetRoomWithDetailsAsync(int roomId);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
    }

    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return await _context.Rooms
                .Where(r => r.Status == RoomStatus.Available)
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomTypeId)
        {
            return await _context.Rooms
                .Where(r => r.RoomTypeID == roomTypeId)
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsByDateRangeAsync(DateTime checkInDate, DateTime checkOutDate)
        {
            return await _context.Rooms
                .Where(r => r.Status == RoomStatus.Available &&
                    !r.Bookings.Any(b => 
                        b.Status != BookingStatus.Cancelled &&
                        b.CheckinDate < checkOutDate &&
                        b.CheckoutDate > checkInDate))
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Rooms
                .Where(r => r.Price >= minPrice && r.Price <= maxPrice)
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<Room> GetRoomWithDetailsAsync(int roomId)
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.RoomID == roomId);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.RoomID == roomId);

            if (room == null || room.Status != RoomStatus.Available)
                return false;

            var hasConflict = room.Bookings.Any(b =>
                b.Status != BookingStatus.Cancelled &&
                b.CheckinDate < checkOutDate &&
                b.CheckoutDate > checkInDate);

            return !hasConflict;
        }
    }
}
