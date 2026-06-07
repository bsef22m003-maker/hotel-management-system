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
        private readonly IRepository<RoomType> _roomTypeRepository;

        public RoomController(IRoomRepository roomRepository, IRepository<RoomType> roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
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
        public async Task<IActionResult> Create()
        {
            ViewBag.RoomTypes = await _roomTypeRepository.GetAllAsync();
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomNumber,RoomTypeID,Capacity,Price,Status,Description,ImageUrl")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.CreatedDate = DateTime.Now;
                await _roomRepository.AddAsync(room);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RoomTypes = await _roomTypeRepository.GetAllAsync();
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
            ViewBag.RoomTypes = await _roomTypeRepository.GetAllAsync();
            return View(room);
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomID,RoomNumber,RoomTypeID,Capacity,Price,Status,Description,ImageUrl,CreatedDate")] Room room)
        {
            if (id != room.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _roomRepository.UpdateAsync(room);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RoomTypes = await _roomTypeRepository.GetAllAsync();
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
