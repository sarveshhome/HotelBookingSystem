using Microsoft.AspNetCore.Mvc;
using Payment.API.Application;
using Dapr;

namespace Payment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ProcessPaymentHandler _handler;

    public PaymentsController(ProcessPaymentHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
    {
        var payment = await _handler.Handle(command);
        return Ok(payment);
    }

    [Topic("hotelbooking-pubsub", "booking-created")]
    [HttpPost("booking-created")]
    public async Task<IActionResult> OnBookingCreated([FromBody] dynamic bookingEvent)
    {
        // Auto-process payment when booking is created
        return Ok();
    }
}
