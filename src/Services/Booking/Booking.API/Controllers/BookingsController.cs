using Microsoft.AspNetCore.Mvc;
using Booking.API.Application;
using Dapr.Client;
using Asp.Versioning;

namespace Booking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly CreateBookingHandler _createHandler;
    private readonly DaprClient _daprClient;

    public BookingsController(CreateBookingHandler createHandler, DaprClient daprClient)
    {
        _createHandler = createHandler;
        _daprClient = daprClient;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var booking = await _createHandler.Handle(command);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var booking = await _daprClient.GetStateAsync<Domain.BookingEntity>("statestore", $"booking-{id}");
        return booking != null ? Ok(booking) : NotFound();
    }
}
