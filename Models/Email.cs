
public class SmtpOptions
{
  public string Host { get; set; }
  public int Port { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }
  public bool EnableSsl { get; set; }
  public string From { get; set; }
}

public class EmailRequest
{
  public string To { get; set; }
  public string Subject { get; set; }
  public string Body { get; set; }
}