using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Models;
using HotelManagement.Data.Repository;

namespace HotelManagement.Data.Repository
{
    public interface IGuestRepository : IRepository<Guest>
    {
        Task<Guest> GetGuestByEmailAsync(string email);
        Task<IEnumerable<Guest>> GetGuestsByCityAsync(string city);
        Task<Guest> GetGuestWithBookingsAsync(int guestId);
    }

    public class GuestRepository : Repository<Guest>, IGuestRepository
    {
        private readonly HotelDbContext _context;

        public GuestRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Guest> GetGuestByEmailAsync(string email)
        {
            return await _context.Guests
                .FirstOrDefaultAsync(g => g.Email == email);
        }

        public async Task<IEnumerable<Guest>> GetGuestsByCityAsync(string city)
        {
            return await _context.Guests
                .Where(g => g.City == city)
                .ToListAsync();
        }

        public async Task<Guest> GetGuestWithBookingsAsync(int guestId)
        {
            return await _context.Guests
                .Include(g => g.Bookings)
                .FirstOrDefaultAsync(g => g.GuestID == guestId);
        }
    }
}
