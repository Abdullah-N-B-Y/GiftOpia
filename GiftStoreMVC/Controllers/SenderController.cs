using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;

namespace GiftStoreMVC.Controllers;

public class SenderController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEmail _email;

    public SenderController(ModelContext context, IWebHostEnvironment webHostEnvironment,IEmail email)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _email = email;
    }

    public IActionResult Index()
    {
        try
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            GiftstoreUser? user = _context.GiftstoreUsers.Where(obj=> obj.Userid == userId).SingleOrDefault();

            ViewData["Username"] = user.Username;
            ViewData["Name"] = user.Name;
            ViewData["Password"] = user.Password;
            ViewData["Email"] = user.Email;
            ViewData["UserId"] = userId;
            ViewData["RoleId"] = user.Roleid;
            ViewData["ImagePath"] = user.Imagepath;


                var testimonials = _context.GiftstoreTestimonials.ToList();
                return View(testimonials);

        }
        catch (Exception ex)
        {
            return View();
        }
    }

    public IActionResult About()
    {
        try
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


            var categories = _context.GiftstoreCategories.ToList();

                return View(categories);

        }
        catch (Exception ex)
        {
            return View();
        }
    }

    public IActionResult Categories()
    {
        try
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

            var categoris = _context.GiftstoreCategories.ToList();
            var gifts = _context.GiftstoreGifts.ToList();

            CategoryGift categoryGift = new() 
            {
                Category = categoris,
                Gift = gifts
            };
            //var model = Tuple.Create<IEnumerable<GiftstoreCategory>,>
            return View(categoryGift);
        }
        catch (Exception ex)
        {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Categories(string? GiftName)//Search
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

        var model = new CategoryGift();

        var categories = _context.GiftstoreCategories.Where(obj => obj.Categoryname.ToLower().Equals(GiftName.ToLower())).ToList();
        if (categories == null || categories.Count == 0)
        {
            var gifts = _context.GiftstoreGifts.Where(obj => obj.Giftname.ToLower().Equals(GiftName.ToLower())).ToList();
            model.Gift = gifts;
        }
        else
        {
            model.Category = categories;
        }

        return View(model);
    }


    public IActionResult GetGiftsByCategoryId(decimal? categoryId)
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

        var gifts = _context.GiftstoreGifts.Where(obj => obj.Categoryid == categoryId).ToList();
        return View(gifts);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult GetGiftsByCategoryId(string? GiftName)
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

        GiftstoreGift? gift = _context.GiftstoreGifts.Where(obj => obj.Giftname.Equals(GiftName)).SingleOrDefault();

        return RedirectToAction("GiftDetails", new { giftId = gift.Giftid });

    }

    public IActionResult GiftDetails(decimal? giftId)
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

        GiftstoreGift? gift = _context.GiftstoreGifts.Where(obj => obj.Giftid == giftId).SingleOrDefault();
        return View(gift);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GiftDetails(string? GiftName, string? recipientName, string? recipientAddress)
    {
        GiftstoreGift? gift = _context.GiftstoreGifts.Where(obj => obj.Giftname.Equals(GiftName)).SingleOrDefault();
        int? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user.Username;
        ViewData["Name"] = user.Name;
        ViewData["Password"] = user.Password;
        ViewData["Email"] = user.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user.Roleid;
        ViewData["ImagePath"] = user.Imagepath;

        GiftstoreUser? makerUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == gift.Userid).FirstOrDefault();
        GiftstoreUser? senderUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == userId).FirstOrDefault();
        var categories = _context.GiftstoreCategories.ToList();
        
        GiftstoreSenderrequest senderrequest = new()
        {
            Recipientname = recipientName,
            Recipientaddress = recipientAddress,
            Requeststatus = "Pending",
            Requestdate = DateTime.Now,
            Senderid = userId,
            Makerid = makerUser.Userid,
            Sendername = senderUser.Name,
            Giftname = gift.Giftname,
            Giftprice = gift.Giftprice,
        };

        _context.Add(senderrequest);
        await _context.SaveChangesAsync();

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));
        email.To.Add(MailboxAddress.Parse(user.Email));

        email.Subject = "Selecting Gift";
        email.Body = new TextPart(TextFormat.Html) { Text = "Hi" + " " + user.Name + " Your chosen gift has been added successfully, you have to wait for gift maker to accept this request." };


        using var smtp = new SmtpClient();
        smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
        smtp.Send(email);
        smtp.Disconnect(true);

        return RedirectToAction("Index");
    }


    public IActionResult Payment(decimal? giftId, string? address ,decimal userId, decimal requestId)
    {
        HttpContext.Session.SetInt32("giftId", (int)giftId);
        HttpContext.Session.SetString("address", address);
        HttpContext.Session.SetInt32("senderId",(int)userId);
        HttpContext.Session.SetInt32("requestId", (int)requestId);
        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user.Username;
        ViewData["Name"] = user.Name;
        ViewData["Password"] = user.Password;
        ViewData["Email"] = user.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user.Roleid;
        ViewData["ImagePath"] = user.Imagepath;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Payment([Bind("Cardholdername,Cardnumber,Expirationdate,Cvv")] GiftstoreBankcard bankcard)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? user2 = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user2.Username;
        ViewData["Name"] = user2.Name;
        ViewData["Password"] = user2.Password;
        ViewData["Email"] = user2.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user2.Roleid;
        ViewData["ImagePath"] = user2.Imagepath;

        decimal giftId = (decimal)HttpContext.Session.GetInt32("giftId");
        string? address = HttpContext.Session.GetString("address");
        decimal? requestId = (decimal)HttpContext.Session.GetInt32("requestId");


        GiftstoreBankcard? card = _context.GiftstoreBankcards.Where(obj => obj.Cardholdername.Equals(bankcard.Cardholdername) &&
                                                                          obj.Cardnumber.Equals(bankcard.Cardnumber) &&
                                                                          obj.Expirationdate.Equals(bankcard.Expirationdate) &&
                                                                          obj.Cvv.Equals(bankcard.Cvv)
        ).SingleOrDefault();
        if (card is not null)
        {

            decimal? senderId = (decimal) HttpContext.Session.GetInt32("senderId");
            GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == senderId).SingleOrDefault();
            GiftstoreGift? gift = _context.GiftstoreGifts.Where(obj => obj.Giftid == giftId).SingleOrDefault();

            if (card.Totalamount >= gift.Giftprice)
            {

                GiftstoreSenderrequest? senderRequest = _context.GiftstoreSenderrequests
                    .Where(obj => obj.Requestid == requestId)
                    .SingleOrDefault();

                senderRequest.Requeststatus = "Paid";
                _context.Update(senderRequest);
                _context.SaveChangesAsync();
                // add to order
                GiftstoreOrder order = new()
                {
                    Orderdate = DateTime.UtcNow,
                    Orderstatus = "Shipping",
                    Recipientaddress = address,
                    Finalprice = gift.Giftprice,
                    Requestid = requestId
                };
                card.Totalamount -= gift.Giftprice;
                _context.Add(order);
                _context.SaveChangesAsync();
                
                
                //sendPaymentBill
                _email.SendPaymentBill(user,gift);

            }
        }
        return View();
    }
}