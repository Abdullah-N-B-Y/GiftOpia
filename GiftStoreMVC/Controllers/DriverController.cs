using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace GiftStoreMVC.Controllers;

public class DriverController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IEmail _email;

    //GiftstoreNotification notification;
    public DriverController(ModelContext context, IWebHostEnvironment webHostEnvironment, IEmail email)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _email = email;
    }
    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user.Username;
        ViewData["Name"] = user.Name;
        ViewData["Password"] = user.Password;
        ViewData["Email"] = user.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user.Roleid;
        ViewData["ImagePath"] = user.Imagepath;
        ViewData["PhoneNumber"] = user.Phonenumber;

        ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
        ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count();
        ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();

        var allOrders = _context.GiftstoreOrders.ToList();
        var allArrivedOrders = allOrders.Where(obj => obj.Orderstatus.Equals("Arrived")).ToList();

        
        ViewData["AllOrders"] = allOrders.Count();
        ViewData["AllArrivedOrders"] = allArrivedOrders.Count();

        return View(user);
    }

    public IActionResult Orders()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user.Username;
        ViewData["Name"] = user.Name;
        ViewData["Password"] = user.Password;
        ViewData["Email"] = user.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user.Roleid;
        ViewData["ImagePath"] = user.Imagepath;
        ViewData["PhoneNumber"] = user.Phonenumber;


        var orders = _context.GiftstoreOrders.Where(obj => obj.Orderstatus.Equals("Shipping")).ToList();
        return View(orders);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Orders(decimal? orderId, string? arrivalStatus, string? reason)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user.Username;
        ViewData["Name"] = user.Name;
        ViewData["Password"] = user.Password;
        ViewData["Email"] = user.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user.Roleid;
        ViewData["ImagePath"] = user.Imagepath;
        ViewData["PhoneNumber"] = user.Phonenumber;

        var order = _context.GiftstoreOrders.Where(obj => obj.Orderid == orderId).SingleOrDefault();
        var requestid = order.Requestid;
        var request = _context.GiftstoreSenderrequests.Where(obj => obj.Requestid == requestid).SingleOrDefault();
        var senderId = request.Senderid;
        var sender = _context.GiftstoreUsers.Where(obj => obj.Userid == senderId).SingleOrDefault();

        {
            order.Orderstatus = "Arrived";
            order.Arrivaldate = DateTime.UtcNow;
            _context.Update(order);
            _context.SaveChanges();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));
            email.To.Add(MailboxAddress.Parse(sender.Email));

            email.Subject = "Arrivel User gift";
            email.Body = new TextPart(TextFormat.Html) { Text = $"Hi {sender.Name} your gift arrived successfully\n Thank you for dealing with us." };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        var orders = _context.GiftstoreOrders.Where(obj => obj.Orderstatus.Equals("Shipping")).ToList();
        return View(orders);
    }
}


   