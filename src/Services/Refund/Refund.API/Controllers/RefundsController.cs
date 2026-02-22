using Microsoft.AspNetCore.Mvc;
using Dapr.Client;

namespace Refund.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefundsController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public RefundsController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessRefund([FromBody] dynamic request)
    {
        var refund = new
        {
            Id = Guid.NewGuid(),
            BookingId = Guid.Parse(request.bookingId.ToString()),
            Amount = (decimal)request.amount,
            Status = "Processed",
            ProcessedAt = DateTime.UtcNow
        };

        await _daprClient.SaveStateAsync("statestore", $"refund-{refund.Id}", refund);
        await _daprClient.PublishEventAsync("hotelbooking-pubsub", "refund-processed", refund);

        return Ok(refund);
    }
}
