using Booking.API.Domain;
using Booking.API.Events;
using Dapr.Client;

namespace Booking.API.Application;

public record CreateBookingCommand(Guid HotelId, string GuestName, string GuestEmail, 
                                   DateTime CheckIn, DateTime CheckOut, int Guests, decimal TotalAmount);

public class CreateBookingHandler
{
    private readonly DaprClient _daprClient;
    private const string StateStore = "statestore";
    private const string PubSub = "hotelbooking-pubsub";

    public CreateBookingHandler(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<BookingEntity> Handle(CreateBookingCommand command)
    {
        var booking = new BookingEntity
        {
            Id = Guid.NewGuid(),
            HotelId = command.HotelId,
            GuestName = command.GuestName,
            GuestEmail = command.GuestEmail,
            CheckIn = command.CheckIn,
            CheckOut = command.CheckOut,
            Guests = command.Guests,
            TotalAmount = command.TotalAmount,
            Status = BookingStatus.Pending
        };

        await _daprClient.SaveStateAsync(StateStore, $"booking-{booking.Id}", booking);

        var bookingEvent = new BookingCreatedEvent(booking.Id, booking.HotelId, 
                                                    booking.GuestEmail, booking.TotalAmount, booking.CreatedAt);
        await _daprClient.PublishEventAsync(PubSub, "booking-created", bookingEvent);

        return booking;
    }
}
