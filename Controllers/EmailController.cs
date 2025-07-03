using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
  private readonly EmailService _emailService;

  public EmailController(EmailService emailService)
  {
    _emailService = emailService;
  }

  [HttpPost("send")]
  public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
  {

    await _emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);

    return Ok(new { message = "Se envió correctamente el correo electrónico" });
  }
}