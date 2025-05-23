

using System.Net.Mail;
using System.Net;
using TaskManager.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure.Notification;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = _configuration.GetValue<string>("EmailSettings:Email");
        var password = _configuration.GetValue<string>("EmailSettings:Password");
        var host = _configuration.GetValue<string>("EmailSettings:Host");
        var port = _configuration.GetValue<int>("EmailSettings:Port");

        var smtpClient = new SmtpClient(host,port);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(email, password);

        var mailMessage = new MailMessage(email!, toEmail, subject, body)
        {
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}
