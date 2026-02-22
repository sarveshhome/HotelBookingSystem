namespace FraudDetection.API.Services;

public interface IFraudDetectionService
{
    Task<bool> CheckTransactionAsync(Guid bookingId, decimal amount);
}

public class FraudDetectionService : IFraudDetectionService
{
    private readonly ILogger<FraudDetectionService> _logger;

    public FraudDetectionService(ILogger<FraudDetectionService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> CheckTransactionAsync(Guid bookingId, decimal amount)
    {
        // Simple fraud check - flag if amount > 10000
        await Task.Delay(50);
        var isFraud = amount > 10000;
        _logger.LogInformation($"Fraud check for booking {bookingId}: {(isFraud ? "FLAGGED" : "PASSED")}");
        return !isFraud;
    }
}
