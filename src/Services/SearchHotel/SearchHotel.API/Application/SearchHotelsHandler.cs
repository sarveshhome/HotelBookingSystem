using SearchHotel.API.Domain;

namespace SearchHotel.API.Application;

public record SearchHotelsQuery(string City, DateTime CheckIn, DateTime CheckOut, int Guests);

public class SearchHotelsHandler
{
    public async Task<List<Hotel>> Handle(SearchHotelsQuery query)
    {
        // Mock data - replace with actual repository
        return await Task.FromResult(new List<Hotel>
        {
            new() { Id = Guid.NewGuid(), Name = "Grand Hotel", Location = "Downtown", City = query.City, 
                    Rating = 4.5m, Amenities = new() { "WiFi", "Pool", "Gym" }, AvailableRooms = 10 },
            new() { Id = Guid.NewGuid(), Name = "Luxury Inn", Location = "Beach", City = query.City, 
                    Rating = 4.8m, Amenities = new() { "WiFi", "Spa", "Restaurant" }, AvailableRooms = 5 }
        });
    }
}
