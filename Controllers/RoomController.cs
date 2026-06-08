using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Data.Repository;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;

        // ✂ Removed: IRepository<RoomType> _roomTypeRepository
        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // GET: Room/Index
        public async Task<IActionResult> Index()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return View(rooms);
        }

        // GET: Room/AvailableRooms
        public async Task<IActionResult> AvailableRooms()
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync();
            return View(rooms);
        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomRepository.GetRoomWithDetailsAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // GET: Room/Create
        public IActionResult Create()
        {
            // ✂ Removed: ViewBag.RoomTypes (no longer needed)
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomNumber,RoomType,Capacity,Price,Status,Description,ImageUrl")] Room room)
        {
            // ✅ RoomTypeID → RoomType (string field now)
            if (ModelState.IsValid)
            {
                await _roomRepository.AddRoomAsync(room); // ✅ Uses AddRoomAsync (handles CreatedDate & default Status)
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            // ✂ Removed: ViewBag.RoomTypes
            return View(room);
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomID,RoomNumber,RoomType,Capacity,Price,Status,Description,ImageUrl,CreatedDate")] Room room)
        {
            // ✅ RoomTypeID → RoomType (string field now)
            if (id != room.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _roomRepository.UpdateAsync(room);
                return RedirectToAction(nameof(Index));
            }
            // ✂ Removed: ViewBag.RoomTypes
            return View(room);
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roomRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Room/SearchByDateRange
        [HttpPost]
        public async Task<IActionResult> SearchByDateRange(DateTime checkInDate, DateTime checkOutDate)
        {
            if (checkOutDate <= checkInDate)
            {
                ModelState.AddModelError("", "Check-out date must be after check-in date.");
                return View("AvailableRooms", await _roomRepository.GetAvailableRoomsAsync());
            }

            var availableRooms = await _roomRepository.GetAvailableRoomsByDateRangeAsync(checkInDate, checkOutDate);
            ViewBag.CheckInDate = checkInDate;
            ViewBag.CheckOutDate = checkOutDate;
            return View("AvailableRooms", availableRooms);
        }
    }
}