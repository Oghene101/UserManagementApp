using MailKit.Net.Smtp;
using MimeKit;
using UserManagementApp.Abstractions;

namespace UserManagementApp.Services;

public class EmailService(
    IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string? recipientEmail, string subject, string body)
    {
        var senderEmail = configuration["EmailSettings:Sender"];
        var port = Convert.ToInt32(configuration["EmailSettings:Port"]);
        var host = configuration["EmailSettings:Host"];
        var appPassword = configuration["EmailSettings:AppPassword"];

        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(senderEmail);
        email.To.Add(MailboxAddress.Parse(recipientEmail));
        email.Subject = subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = body;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.CheckCertificateRevocation = true;
        await smtp.ConnectAsync(host, port, true);
        await smtp.AuthenticateAsync(senderEmail, appPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}