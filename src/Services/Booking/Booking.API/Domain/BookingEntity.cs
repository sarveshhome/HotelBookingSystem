namespace Booking.API.Domain;

public class BookingEntity
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string GuestName { get; set; }
    public string GuestEmail { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Guests { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}
