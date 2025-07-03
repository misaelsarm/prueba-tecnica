using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
public class EmailService
{
  private readonly SmtpOptions _options;

  public EmailService(IOptions<SmtpOptions> options)
  {
    _options = options.Value;
  }

  public async Task SendEmailAsync(string to, string subject, string body)
  {
    using var smtpClient = new SmtpClient(_options.Host, _options.Port)
    {
      Credentials = new NetworkCredential(_options.Username, _options.Password),
      EnableSsl = _options.EnableSsl
    };

    var mailMessage = new MailMessage
    {
      From = new MailAddress(_options.From),
      Subject = subject,
      Body = body,
      IsBodyHtml = true
    };

    mailMessage.To.Add(to);

    await smtpClient.SendMailAsync(mailMessage);
  }

}