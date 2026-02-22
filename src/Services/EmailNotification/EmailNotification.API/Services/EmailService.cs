namespace EmailNotification.API.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Simulate email sending
        _logger.LogInformation($"Sending email to {to}: {subject}");
        await Task.Delay(100);
    }
}
