using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Models;
using HotelManagement.Data.Repository;

namespace HotelManagement.Data.Repository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByBookingAsync(int bookingId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Payment> GetPaymentWithDetailsAsync(int paymentId);
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetMonthlyRevenueAsync(int month, int year);
    }

    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly HotelDbContext _context;

        public PaymentRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingAsync(int bookingId)
        {
            return await _context.Payments
                .Where(p => p.BookingID == bookingId)
                .Include(p => p.Booking)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .Include(p => p.Booking)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .Include(p => p.Booking)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }
        public async Task<decimal> GetTotalRevenueAsync()
        {
            var total = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .Select(p => (double)p.Amount)
                .SumAsync();
            return (decimal)total;
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int month, int year)
        {
            var total = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed &&
                    p.PaymentDate.Month == month &&
                    p.PaymentDate.Year == year)
                .Select(p => (double)p.Amount)
                .SumAsync();
            return (decimal)total;
        }

        public async Task<Payment> GetPaymentWithDetailsAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Room)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Guest)
                .FirstOrDefaultAsync(p => p.PaymentID == paymentId);
        }
    }
}
