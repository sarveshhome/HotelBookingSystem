using Microsoft.AspNetCore.Mvc;
using Dapr;
using Dapr.Client;

namespace Loyalty.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoyaltyController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public LoyaltyController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpGet("{email}/points")]
    public async Task<IActionResult> GetPoints(string email)
    {
        var points = await _daprClient.GetStateAsync<int>("statestore", $"loyalty-{email}") ?? 0;
        return Ok(new { Email = email, Points = points });
    }

    [Topic("hotelbooking-pubsub", "booking-created")]
    [HttpPost("booking-created")]
    public async Task<IActionResult> OnBookingCreated([FromBody] dynamic bookingEvent)
    {
        var email = bookingEvent.GuestEmail.ToString();
        var points = await _daprClient.GetStateAsync<int>("statestore", $"loyalty-{email}") ?? 0;
        points += (int)((decimal)bookingEvent.Amount / 10);
        await _daprClient.SaveStateAsync("statestore", $"loyalty-{email}", points);
        return Ok();
    }
}
