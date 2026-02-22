using Microsoft.AspNetCore.Mvc;

namespace RoomRate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatesController : ControllerBase
{
    [HttpGet("{hotelId}")]
    public IActionResult GetRates(Guid hotelId, [FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        var rates = new[]
        {
            new { RoomType = "Standard", PricePerNight = 100m, TotalPrice = 100m * nights },
            new { RoomType = "Deluxe", PricePerNight = 150m, TotalPrice = 150m * nights },
            new { RoomType = "Suite", PricePerNight = 250m, TotalPrice = 250m * nights }
        };
        return Ok(rates);
    }
}
