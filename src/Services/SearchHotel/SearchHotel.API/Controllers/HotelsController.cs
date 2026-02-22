using Microsoft.AspNetCore.Mvc;
using SearchHotel.API.Application;
using Asp.Versioning;

namespace SearchHotel.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly SearchHotelsHandler _handler;

    public HotelsController(SearchHotelsHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string city, [FromQuery] DateTime checkIn, 
                                            [FromQuery] DateTime checkOut, [FromQuery] int guests = 1)
    {
        var query = new SearchHotelsQuery(city, checkIn, checkOut, guests);
        var result = await _handler.Handle(query);
        return Ok(result);
    }
}
