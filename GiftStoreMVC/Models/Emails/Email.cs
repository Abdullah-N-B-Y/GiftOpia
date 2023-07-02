using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace GiftStoreMVC.Models;

public class Email : IEmail
{
    private readonly IConfiguration _configuration;

    public Email(IConfiguration configuration) => _configuration = configuration;

    public  string SendPaymentEmailToSender(string  userEmailTo,string  userNameTo,decimal? giftId,string address)
    {
        var email = new MimeMessage();
        
        email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings")["EmailUsername"].ToString()));
        email.To.Add(MailboxAddress.Parse(userEmailTo));
        
        email.Subject = "Gift request from GiftOpia";

        string paymentUrlLink =$"<a href='{_configuration.GetSection("EmailSettings")["PaymentURl"]}?giftId={giftId}&address={address}'>Pay</a>";
        
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = $"Hi {userNameTo}, has been accepted with {giftId} ID,\nPlease pay using this link: {paymentUrlLink}"
        };


        using var smtp = new SmtpClient();
        smtp.Connect(_configuration.GetSection("EmailSettings")["EmailHost"], 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate(_configuration.GetSection("EmailSettings")["EmailUsername"], _configuration.GetSection("EmailSettings")["EmailPassword"]);
        smtp.Send(email);
        smtp.Disconnect(true);
        return "massage is sent";
    }


    public string SendEmailToUser(string userEmailTo,string  userNameTo,string massage)
    {
        var email = new MimeMessage();
        
        email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings")["EmailUsername"].ToString()));
        email.To.Add(MailboxAddress.Parse(userEmailTo));


        email.Subject = "Sign in";
        
        email.Body = new TextPart(TextFormat.Html) { Text = $"Hi {userNameTo}  Your are {massage}" };


        using var smtp = new SmtpClient();
        smtp.Connect(_configuration.GetSection("EmailSettings")["EmailHost"], 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate(_configuration.GetSection("EmailSettings")["EmailUsername"], _configuration.GetSection("EmailSettings")["EmailPassword"]);
        smtp.Send(email);
        smtp.Disconnect(true);
        
        return "massage is sent";
        
    }
}

public interface IEmail
{
    public string SendPaymentEmailToSender(string userEmailTo, string userNameTo, decimal? giftId, string address);
    public string SendEmailToUser(string userEmailTo, string userNameTo, string massage);
}

 
