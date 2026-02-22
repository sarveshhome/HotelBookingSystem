namespace Payment.API.Domain;

public class PaymentEntity
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}
