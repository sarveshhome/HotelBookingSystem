using Payment.API.Domain;
using Dapr.Client;

namespace Payment.API.Application;

public record ProcessPaymentCommand(Guid BookingId, decimal Amount, string PaymentMethod);

public class ProcessPaymentHandler
{
    private readonly DaprClient _daprClient;

    public ProcessPaymentHandler(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<PaymentEntity> Handle(ProcessPaymentCommand command)
    {
        // Simulate payment processing
        var payment = new PaymentEntity
        {
            Id = Guid.NewGuid(),
            BookingId = command.BookingId,
            Amount = command.Amount,
            PaymentMethod = command.PaymentMethod,
            Status = PaymentStatus.Completed
        };

        await _daprClient.SaveStateAsync("statestore", $"payment-{payment.Id}", payment);
        await _daprClient.PublishEventAsync("hotelbooking-pubsub", "payment-completed", 
                                            new { payment.Id, payment.BookingId, payment.Amount });

        return payment;
    }
}
