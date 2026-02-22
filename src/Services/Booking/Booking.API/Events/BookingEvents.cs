namespace Booking.API.Events;

public record BookingCreatedEvent(Guid BookingId, Guid HotelId, string GuestEmail, decimal Amount, DateTime CreatedAt);

public record BookingConfirmedEvent(Guid BookingId, string GuestEmail);

public record BookingCancelledEvent(Guid BookingId, string GuestEmail, decimal RefundAmount);
