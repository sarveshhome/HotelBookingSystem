using Microsoft.AspNetCore.Mvc;
using FraudDetection.API.Services;
using Dapr;

namespace FraudDetection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FraudController : ControllerBase
{
    private readonly IFraudDetectionService _fraudService;

    public FraudController(IFraudDetectionService fraudService)
    {
        _fraudService = fraudService;
    }

    [HttpPost("check")]
    public async Task<IActionResult> CheckTransaction([FromBody] dynamic request)
    {
        var isValid = await _fraudService.CheckTransactionAsync(
            Guid.Parse(request.bookingId.ToString()), 
            (decimal)request.amount);
        return Ok(new { IsValid = isValid });
    }

    [Topic("hotelbooking-pubsub", "booking-created")]
    [HttpPost("booking-created")]
    public async Task<IActionResult> OnBookingCreated([FromBody] dynamic bookingEvent)
    {
        await _fraudService.CheckTransactionAsync(
            Guid.Parse(bookingEvent.BookingId.ToString()), 
            (decimal)bookingEvent.Amount);
        return Ok();
    }
}
