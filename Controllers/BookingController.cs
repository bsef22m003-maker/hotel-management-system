using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Data.Repository;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRepository<Guest> _guestRepository;

        public BookingController(IBookingRepository bookingRepository, IRoomRepository roomRepository, IRepository<Guest> guestRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
        }

        // GET: Booking/Index
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingRepository.GetBookingWithDetailsAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create(int? roomId = null)
        {
            ViewBag.Guests = await _guestRepository.GetAllAsync();
            ViewBag.Rooms = await _roomRepository.GetAvailableRoomsAsync();
            ViewBag.SelectedRoomId = roomId;
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestID,RoomID,CheckinDate,CheckoutDate,SpecialRequests")] Booking booking)
        {
            if (booking.CheckoutDate <= booking.CheckinDate)
            {
                ModelState.AddModelError("", "Check-out date must be after check-in date.");
            }

            if (ModelState.IsValid)
            {
                bool isAvailable = await _roomRepository.IsRoomAvailableAsync(booking.RoomID, booking.CheckinDate, booking.CheckoutDate);
                if (!isAvailable)
                {
                    ModelState.AddModelError("", "Selected room is not available for the specified dates.");
                    ViewBag.Guests = await _guestRepository.GetAllAsync();
                    ViewBag.Rooms = await _roomRepository.GetAvailableRoomsAsync();
                    return View(booking);
                }

                var room = await _roomRepository.GetByIdAsync(booking.RoomID);
                int nights = (int)(booking.CheckoutDate - booking.CheckinDate).TotalDays;
                booking.TotalPrice = room.Price * nights;
                booking.BookingDate = DateTime.Now;
                booking.Status = BookingStatus.Confirmed;

                await _bookingRepository.AddAsync(booking);
                return RedirectToAction("Details", new { id = booking.BookingID });
            }

            ViewBag.Guests = await _guestRepository.GetAllAsync();
            ViewBag.Rooms = await _roomRepository.GetAvailableRoomsAsync();
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingID,GuestID,RoomID,CheckinDate,CheckoutDate,BookingDate,TotalPrice,Status,SpecialRequests")] Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bookingRepository.UpdateAsync(booking);
                return RedirectToAction(nameof(Details), new { id = booking.BookingID });
            }
            return View(booking);
        }

        // GET: Booking/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Booking/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, BookingStatus status)
        {
            var success = await _bookingRepository.UpdateBookingStatusAsync(id, status);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Booking/Cancel/5
        public async Task<IActionResult> Cancel(int id)
        {
            var success = await _bookingRepository.UpdateBookingStatusAsync(id, BookingStatus.Cancelled);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/MyBookings/5
        public async Task<IActionResult> MyBookings(int guestId)
        {
            var bookings = await _bookingRepository.GetBookingsByGuestAsync(guestId);
            return View(bookings);
        }
    }
}
