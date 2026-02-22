using Payment.API.Application;
using Payment.API.Domain;
using Dapr.Client;
using Moq;
using FluentAssertions;
using Xunit;

namespace Payment.Tests;

public class ProcessPaymentHandlerTests
{
    private readonly Mock<DaprClient> _daprClientMock;
    private readonly ProcessPaymentHandler _handler;

    public ProcessPaymentHandlerTests()
    {
        _daprClientMock = new Mock<DaprClient>();
        _handler = new ProcessPaymentHandler(_daprClientMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ProcessesPayment()
    {
        var command = new ProcessPaymentCommand(Guid.NewGuid(), 500m, "CreditCard");

        var result = await _handler.Handle(command);

        result.Should().NotBeNull();
        result.BookingId.Should().Be(command.BookingId);
        result.Amount.Should().Be(command.Amount);
        result.Status.Should().Be(PaymentStatus.Completed);
    }

    [Fact]
    public async Task Handle_ValidCommand_SavesPaymentState()
    {
        var command = new ProcessPaymentCommand(Guid.NewGuid(), 300m, "DebitCard");

        await _handler.Handle(command);

        _daprClientMock.Verify(x => x.SaveStateAsync(
            "statestore", It.IsAny<string>(), It.IsAny<PaymentEntity>(),
            It.IsAny<StateOptions>(), It.IsAny<Dictionary<string, string>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_PublishesPaymentCompletedEvent()
    {
        var command = new ProcessPaymentCommand(Guid.NewGuid(), 750m, "CreditCard");

        await _handler.Handle(command);

        _daprClientMock.Verify(x => x.PublishEventAsync(
            "hotelbooking-pubsub", "payment-completed", It.IsAny<object>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("CreditCard")]
    [InlineData("DebitCard")]
    [InlineData("PayPal")]
    public async Task Handle_DifferentPaymentMethods_ProcessesSuccessfully(string paymentMethod)
    {
        var command = new ProcessPaymentCommand(Guid.NewGuid(), 400m, paymentMethod);

        var result = await _handler.Handle(command);

        result.PaymentMethod.Should().Be(paymentMethod);
    }
}
