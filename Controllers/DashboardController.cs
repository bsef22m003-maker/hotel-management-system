using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Data.Repository;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRepository<Guest> _guestRepository;

        public DashboardController(IBookingRepository bookingRepository, IRoomRepository roomRepository, 
            IPaymentRepository paymentRepository, IRepository<Guest> guestRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _paymentRepository = paymentRepository;
            _guestRepository = guestRepository;
        }

        // GET: Dashboard/Index
        public async Task<IActionResult> Index()
        {
            var allRooms = await _roomRepository.GetAllAsync();
            var allGuests = await _guestRepository.GetAllAsync();
            var allBookings = await _bookingRepository.GetAllAsync();
            var totalRevenue = await _paymentRepository.GetTotalRevenueAsync();

            var stats = new DashboardStats
            {
                TotalRooms = ((List<Room>)allRooms).Count,
                TotalGuests = ((List<Guest>)allGuests).Count,
                TotalBookings = ((List<Booking>)allBookings).Count,
                TotalRevenue = totalRevenue,
                AvailableRooms = ((List<Room>)await _roomRepository.GetAvailableRoomsAsync()).Count,
                PendingBookings = ((List<Booking>)await _bookingRepository.GetBookingsByStatusAsync(BookingStatus.Pending)).Count
            };

            return View(stats);
        }
    }

    public class DashboardStats
    {
        public int TotalRooms { get; set; }
        public int TotalGuests { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int AvailableRooms { get; set; }
        public int PendingBookings { get; set; }
    }
}
