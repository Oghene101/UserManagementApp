namespace UserManagementApp.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string? userEmail, string subject, string body);
}