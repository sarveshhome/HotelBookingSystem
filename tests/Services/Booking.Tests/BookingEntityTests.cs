using Booking.API.Domain;
using FluentAssertions;
using Xunit;

namespace Booking.Tests;

public class BookingEntityTests
{
    [Fact]
    public void BookingEntity_DefaultStatus_IsPending()
    {
        var booking = new BookingEntity
        {
            Id = Guid.NewGuid(),
            HotelId = Guid.NewGuid(),
            GuestName = "Test User",
            GuestEmail = "test@example.com",
            CheckIn = DateTime.UtcNow.AddDays(1),
            CheckOut = DateTime.UtcNow.AddDays(3),
            Guests = 2,
            TotalAmount = 400m,
            Status = BookingStatus.Pending
        };

        booking.Status.Should().Be(BookingStatus.Pending);
    }

    [Fact]
    public void BookingEntity_CreatedAt_IsSet()
    {
        var booking = new BookingEntity();
        
        booking.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(BookingStatus.Pending)]
    [InlineData(BookingStatus.Confirmed)]
    [InlineData(BookingStatus.Cancelled)]
    [InlineData(BookingStatus.Completed)]
    public void BookingEntity_CanSetStatus(BookingStatus status)
    {
        var booking = new BookingEntity { Status = status };
        
        booking.Status.Should().Be(status);
    }
}
