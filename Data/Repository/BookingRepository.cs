using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Models;
using HotelManagement.Data.Repository;

namespace HotelManagement.Data.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByGuestAsync(int guestId);
        Task<IEnumerable<Booking>> GetBookingsByRoomAsync(int roomId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<Booking> GetBookingWithDetailsAsync(int bookingId);
        Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(DateTime fromDate);
        Task<IEnumerable<Booking>> GetOverdueBookingsAsync();
        Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);
    }

    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByGuestAsync(int guestId)
        {
            return await _context.Bookings
                .Where(b => b.GuestID == guestId)
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRoomAsync(int roomId)
        {
            return await _context.Bookings
                .Where(b => b.RoomID == roomId)
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderBy(b => b.CheckinDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _context.Bookings
                .Where(b => b.Status == status)
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Include(b => b.Room.RoomType)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.BookingID == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(DateTime fromDate)
        {
            return await _context.Bookings
                .Where(b => b.CheckinDate >= fromDate && b.Status != BookingStatus.Cancelled)
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderBy(b => b.CheckinDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetOverdueBookingsAsync()
        {
            return await _context.Bookings
                .Where(b => b.CheckoutDate < DateTime.Now && b.Status == BookingStatus.CheckedIn)
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .ToListAsync();
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
                return false;

            booking.Status = newStatus;

            if (newStatus == BookingStatus.CheckedOut)
            {
                var room = await _context.Rooms.FindAsync(booking.RoomID);
                if (room != null)
                {
                    room.Status = RoomStatus.Available;
                    _context.Rooms.Update(room);
                }
            }
            else if (newStatus == BookingStatus.CheckedIn)
            {
                var room = await _context.Rooms.FindAsync(booking.RoomID);
                if (room != null)
                {
                    room.Status = RoomStatus.Occupied;
                    _context.Rooms.Update(room);
                }
            }

            _context.Bookings.Update(booking);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
