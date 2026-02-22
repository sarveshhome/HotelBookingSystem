using SearchHotel.API.Application;
using FluentAssertions;
using Xunit;

namespace SearchHotel.Tests;

public class SearchHotelsHandlerTests
{
    private readonly SearchHotelsHandler _handler;

    public SearchHotelsHandlerTests()
    {
        _handler = new SearchHotelsHandler();
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsHotels()
    {
        var query = new SearchHotelsQuery("Mumbai", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5), 2);

        var result = await _handler.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsHotelsWithCorrectCity()
    {
        var query = new SearchHotelsQuery("Delhi", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3), 1);

        var result = await _handler.Handle(query);

        result.Should().AllSatisfy(h => h.City.Should().Be(query.City));
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsHotelsWithAmenities()
    {
        var query = new SearchHotelsQuery("Bangalore", DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(4), 3);

        var result = await _handler.Handle(query);

        result.Should().AllSatisfy(h => h.Amenities.Should().NotBeEmpty());
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsHotelsWithAvailableRooms()
    {
        var query = new SearchHotelsQuery("Chennai", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 2);

        var result = await _handler.Handle(query);

        result.Should().AllSatisfy(h => h.AvailableRooms.Should().BeGreaterThan(0));
    }
}
