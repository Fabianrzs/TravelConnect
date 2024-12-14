using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using TravelConnect.Domain.Ports.Notifications;

namespace TravelConnect.Infrastructure.Adapters.Notifications;

public class NotificationService(IConfiguration configuration) : INotificationService
{
    private readonly string smtpHost = configuration["EmailSettings:SmtpHost"]!;
    private readonly int smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]!);
    private readonly string smtpUser = configuration["TravelConnect.EmailSettings-SmtpUser"]!;
    private readonly string smtpPassword = configuration["TravelConnect.EmailSettings-SmtpPassword"]!;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }

    public Task SendSmsAsync(string phoneNumber, string message)
    {
        throw new NotImplementedException();
    }
}
