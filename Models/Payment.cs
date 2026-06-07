using System;

namespace HotelManagement.Models
{
    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        OnlinePayment
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public class Payment
    {
        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionID { get; set; }
        public string? Notes { get; set; }
        
        // Foreign key
        public Booking Booking { get; set; }
    }
}
