namespace SearchHotel.API.Domain;

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string City { get; set; }
    public decimal Rating { get; set; }
    public List<string> Amenities { get; set; }
    public int AvailableRooms { get; set; }
}
