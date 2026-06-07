using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Data.Repository;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;

        public PaymentController(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        // GET: Payment/Index
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return View(payments);
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentRepository.GetPaymentWithDetailsAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // GET: Payment/Create
        public async Task<IActionResult> Create(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            var payment = new Payment
            {
                BookingID = bookingId,
                Amount = booking.TotalPrice,
                PaymentDate = DateTime.Now,
                Status = PaymentStatus.Pending
            };

            return View(payment);
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingID,Amount,PaymentMethod,TransactionID,Notes")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.PaymentDate = DateTime.Now;
                payment.Status = PaymentStatus.Completed;

                await _paymentRepository.AddAsync(payment);

                var booking = await _bookingRepository.GetByIdAsync(payment.BookingID);
                if (booking != null && booking.Status == BookingStatus.Pending)
                {
                    booking.Status = BookingStatus.Confirmed;
                    await _bookingRepository.UpdateAsync(booking);
                }

                return RedirectToAction("Details", new { id = payment.PaymentID });
            }
            return View(payment);
        }

        // GET: Payment/ByBooking/5
        public async Task<IActionResult> ByBooking(int bookingId)
        {
            var payments = await _paymentRepository.GetPaymentsByBookingAsync(bookingId);
            ViewBag.BookingId = bookingId;
            return View(payments);
        }
    }
}
