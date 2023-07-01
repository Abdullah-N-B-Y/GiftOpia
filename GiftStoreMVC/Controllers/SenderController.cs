using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace GiftStoreMVC.Controllers
{
    public class SenderController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        GiftstoreNotification notification;
        public SenderController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {//decimal? Giftid, string? Giftname, decimal? Giftprice, int? Giftavailability, string? Giftdescription, decimal? Categoryid
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var user = _context.GiftstoreUsers.Where(obj=> obj.Userid == userId).SingleOrDefault();
                if (userId != null)
                {
                    //ViewData["GiftObj"] = gift;
                    return View(user);
                }
                else
                {
                    return View();
                }

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
                var userId = HttpContext.Session.GetInt32("UserId");
                var user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
                if (userId != null)
                {
                    //ViewData["GiftObj"] = gift;
                    return View(user);
                }
                else
                {
                    return View();
                }

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
                var userId = HttpContext.Session.GetInt32("UserId");

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
            var gifts = _context.GiftstoreGifts.Where(obj => obj.Categoryid == categoryId).ToList();
            return View(gifts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetGiftsByCategoryId(string? GiftName)
        {
            var gift = _context.GiftstoreGifts.Where(obj => obj.Giftname.Equals(GiftName)).SingleOrDefault();

            return RedirectToAction("GiftDetails", new { giftId = gift.Giftid });

        }

        public IActionResult Payment(decimal? giftId, string? address)
        {
            ViewData["giftId"] = giftId;
            ViewData["address"] = address;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Payment([Bind("Cardholdername,Cardnumber,Expirationdate,Cvv")] GiftstoreBankcard bankcard)
        {
            var card = _context.GiftstoreBankcards.Where(obj=> obj.Cardholdername.Equals(bankcard.Cardholdername) &&
                                                               obj.Cardnumber.Equals(bankcard.Cardnumber) &&
                                                               obj.Expirationdate.Equals(bankcard.Expirationdate) &&
                                                               obj.Cvv.Equals(bankcard.Cvv) 
                                                               ).SingleOrDefault();
            if (card != null) 
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
                var giftId = (decimal) ViewData["giftId"];
                string? address = (string) ViewData["address"];
                var gift = _context.GiftstoreGifts.Where(obj => obj.Giftid == giftId).SingleOrDefault();

                if (card.Totalamount >= gift.Giftprice)
                {
                    GiftstoreSenderrequest senderrequest = new()
                    {
                        Recipientname = bankcard.Cardholdername,
                        Recipientaddress = address,
                        Requeststatus = "Pending",
                        Requestdate = DateTime.Now,
                        Senderid = userId,
                        Giftid = giftId
                    };

                    _context.Add(senderrequest);
                    _context.SaveChangesAsync();

                    GiftstoreNotification notification = new()
                    {
                        Notificationcontent = "Accept my gift request",
                        Notificationdate = DateTime.Now,
                        Email = user.Email,
                        Isread = false,
                        Userid = userId
                    };
                    _context.Add(notification);
                    _context.SaveChangesAsync();
                }
            }
            return View();
        }


        public IActionResult GiftDetails(decimal? giftId)
        {
            var gift = _context.GiftstoreGifts.Where(obj => obj.Giftid == giftId).SingleOrDefault();
            return View(gift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiftDetails(string? GiftName, string? recipientName, string? recipientAddress)
        {
            var gift = _context.GiftstoreGifts.Where(obj => obj.Giftname.Equals(GiftName)).SingleOrDefault();
            var userId = HttpContext.Session.GetInt32("UserId");
            var makerUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == gift.Userid).FirstOrDefault();
            var senderUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == userId).FirstOrDefault();
            var categories = _context.GiftstoreCategories.ToList();
            
            GiftstoreSenderrequest senderrequest = new()
            {
                Recipientname = recipientName,
                Recipientaddress = recipientAddress,
                Requeststatus = "Pending",
                Requestdate = DateTime.Now,
                Senderid = userId,
                Giftid = gift.Giftid,
                Makerid = makerUser.Userid,
                Sendername = senderUser.Name,
                Giftname = gift.Giftname,
                Giftprice = gift.Giftprice,
            };

            _context.Add(senderrequest);
            await _context.SaveChangesAsync();

            //Email from sender to maker to tell him that I request a gift

            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(senderUser.Email.ToString()));
            //email.To.Add(MailboxAddress.Parse(makerUser.Email));


            //email.Subject = "Buy a gift";
            //email.Body = new TextPart(TextFormat.Html) { Text = "Hi" + " " + makerUser.Name + " I wanna order a gift" };


            //using var smtp = new SmtpClient();
            //smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //smtp.Authenticate("abdullahnby@outlook.com", "Abdullah2000$");
            //smtp.Send(email);
            //smtp.Disconnect(true); 
            ////senderemailmessage@gmail.com
            ////A1234!@#$

            return RedirectToAction("Index");
        }
    }
}
