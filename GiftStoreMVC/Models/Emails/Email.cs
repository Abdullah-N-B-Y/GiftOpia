using Azure;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace GiftStoreMVC.Models;

public class Email : IEmail
{
    private readonly IConfiguration _configuration;

    public Email(IConfiguration configuration) => _configuration = configuration;

    public  string SendPaymentEmailToSender(decimal userId, string  userEmailTo,string  userNameTo,decimal? giftId,string address)
    {
        var email = new MimeMessage();
        
        email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings")["EmailUsername"].ToString()));
        email.To.Add(MailboxAddress.Parse(userEmailTo));
        
        email.Subject = "Gift request from GiftOpia";

        string paymentUrlLink =$"<a href='{_configuration.GetSection("EmailSettings")["PaymentURl"]}?giftId={giftId}&address={address}&userId={userId}'>Pay</a>";
        
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


public string SendPaymentBill(GiftstoreUser user, GiftstoreGift gift)
{
    var email = new MimeMessage();

    email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings")["EmailUsername"].ToString()));
    email.To.Add(MailboxAddress.Parse(user.Email));
    email.Subject = "Payment Bill";
    email.Body = GeneratePdf(user.Username, gift);
    using var smtp = new SmtpClient();
    smtp.Connect(_configuration.GetSection("EmailSettings")["EmailHost"], 587, MailKit.Security.SecureSocketOptions.StartTls);
    smtp.Authenticate(_configuration.GetSection("EmailSettings")["EmailUsername"], _configuration.GetSection("EmailSettings")["EmailPassword"]);
    smtp.Send(email);
    smtp.Disconnect(true);

    return "message is sent";
}

private MimePart GeneratePdf(string userName, GiftstoreGift gift)
{
    var document = new Document();
    var stream = new MemoryStream();
    var writer = PdfWriter.GetInstance(document, stream);

    document.Open();
    document.Add(new Paragraph($"User Name: {userName}"));
    document.Add(new Paragraph($"Gift Name: {gift.Giftname}"));
    document.Add(new Paragraph($"Gift Description: {gift.Giftdescription}"));
    document.Add(new Paragraph($"Gift Price: {gift.Giftprice}"));
    document.Add(new Paragraph("Thanks for trusting"));

    writer.CloseStream = false;
    document.Close();
    writer.Close();

    stream.Position = 0;

    var multipart = new Multipart("mixed");

    var body = new TextPart("plain")
    {
        Text = "Please find the Payment Bill PDF document."
    };

    multipart.Add(body);

    var attachment = new MimePart("application", "pdf")
    {
        Content = new MimeContent(stream),
        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
        ContentTransferEncoding = ContentEncoding.Base64,
        FileName = "Bill.pdf"
    };

    multipart.Add(attachment);

    return attachment;
}

}

public interface IEmail
{
    public string SendPaymentEmailToSender(decimal userId,string userEmailTo, string userNameTo, decimal? giftId, string address);
    public string SendEmailToUser(string userEmailTo, string userNameTo, string massage);
    public string SendPaymentBill(GiftstoreUser user,GiftstoreGift gift);
}

 
