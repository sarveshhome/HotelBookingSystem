using Microsoft.AspNetCore.Mvc;
using EmailNotification.API.Services;
using Dapr;

namespace EmailNotification.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IEmailService _emailService;

    public NotificationsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [Topic("hotelbooking-pubsub", "booking-created")]
    [HttpPost("booking-created")]
    public async Task<IActionResult> OnBookingCreated([FromBody] dynamic bookingEvent)
    {
        await _emailService.SendEmailAsync(
            bookingEvent.GuestEmail.ToString(),
            "Booking Confirmation",
            $"Your booking {bookingEvent.BookingId} has been created.");
        return Ok();
    }

    [Topic("hotelbooking-pubsub", "payment-completed")]
    [HttpPost("payment-completed")]
    public async Task<IActionResult> OnPaymentCompleted([FromBody] dynamic paymentEvent)
    {
        await _emailService.SendEmailAsync(
            "guest@example.com",
            "Payment Confirmation",
            $"Payment completed for booking {paymentEvent.BookingId}");
        return Ok();
    }
}
