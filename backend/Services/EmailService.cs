using MailKit.Net.Smtp;
using MimeKit;

namespace Proyecto_Web_Q2.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(
            new MailboxAddress(
                _configuration["Email:FromName"],
                _configuration["Email:FromAddress"]!
            )
        );
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _configuration["Email:SmtpHost"]!,
            int.Parse(_configuration["Email:SmtpPort"]!),
            MailKit.Security.SecureSocketOptions.StartTls
        );
        await client.AuthenticateAsync(
            _configuration["Email:SmtpUser"]!,
            _configuration["Email:SmtpPass"]!
        );
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
