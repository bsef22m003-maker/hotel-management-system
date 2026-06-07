using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Data.Repository;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestRepository _guestRepository;

        public GuestController(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        // GET: Guest/Index
        public async Task<IActionResult> Index()
        {
            var guests = await _guestRepository.GetAllAsync();
            return View(guests);
        }

        // GET: Guest/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var guest = await _guestRepository.GetGuestWithBookingsAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // GET: Guest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,Address,City,Country")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                guest.CreatedDate = DateTime.Now;
                await _guestRepository.AddAsync(guest);
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guest/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestID,FirstName,LastName,Email,Phone,Address,City,Country,CreatedDate")] Guest guest)
        {
            if (id != guest.GuestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _guestRepository.UpdateAsync(guest);
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guest/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guest/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _guestRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
