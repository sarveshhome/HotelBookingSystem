using Booking.API.Application;
using Booking.API.Domain;
using Dapr.Client;
using Moq;
using FluentAssertions;
using Xunit;

namespace Booking.Tests;

public class CreateBookingHandlerTests
{
    private readonly Mock<DaprClient> _daprClientMock;
    private readonly CreateBookingHandler _handler;

    public CreateBookingHandlerTests()
    {
        _daprClientMock = new Mock<DaprClient>();
        _handler = new CreateBookingHandler(_daprClientMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesBooking()
    {
        var command = new CreateBookingCommand(
            Guid.NewGuid(), "John Doe", "john@example.com",
            DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5), 2, 500m);

        var result = await _handler.Handle(command);

        result.Should().NotBeNull();
        result.HotelId.Should().Be(command.HotelId);
        result.GuestName.Should().Be(command.GuestName);
        result.Status.Should().Be(BookingStatus.Pending);
    }

    [Fact]
    public async Task Handle_ValidCommand_SavesStateToStore()
    {
        var command = new CreateBookingCommand(
            Guid.NewGuid(), "Jane Doe", "jane@example.com",
            DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3), 1, 300m);

        await _handler.Handle(command);

        _daprClientMock.Verify(x => x.SaveStateAsync(
            "statestore", It.IsAny<string>(), It.IsAny<BookingEntity>(),
            It.IsAny<StateOptions>(), It.IsAny<Dictionary<string, string>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_PublishesEvent()
    {
        var command = new CreateBookingCommand(
            Guid.NewGuid(), "Bob Smith", "bob@example.com",
            DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(4), 3, 600m);

        await _handler.Handle(command);

        _daprClientMock.Verify(x => x.PublishEventAsync(
            "hotelbooking-pubsub", "booking-created", It.IsAny<object>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
