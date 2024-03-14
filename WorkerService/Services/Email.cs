using System;
using System.Net;
using System.Net.Mail;

public class Email
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _username;
    private readonly string _password;

    public Email()
    {
        _smtpServer = "";
        _smtpPort = 587;
        _username = "";
        _password = "";
    }

    public void SendEmail(string to, string subject, string body)
    {
        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_username, _password);
            client.EnableSsl = false;

            var message = new MailMessage(_username, to, subject, body);

            client.Send(message);
        }
    }
}