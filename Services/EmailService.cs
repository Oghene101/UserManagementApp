using UserManagementApp.Abstractions;

namespace UserManagementApp.Services;

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string? userEmail, string subject, string body)
    {
        throw new NotImplementedException();
    }
}