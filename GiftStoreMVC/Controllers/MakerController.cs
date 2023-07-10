using GiftStoreMVC.Models;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Linq;

namespace GiftStoreMVC.Controllers;

public class MakerController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEmail _email;

    public MakerController(ModelContext context, IWebHostEnvironment webHostEnvironment,IEmail email)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _email = email;
    }

    public IActionResult Index()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        var allThisRequestsNumber = _context.GiftstoreSenderrequests
            .Where(obj => obj.Makerid == id)
            .Select(obj => obj.Requestid)
            .ToList();
        var allThisUserOrdersNumber = _context.GiftstoreOrders
            .Where(obj => allThisRequestsNumber.Contains((decimal)obj.Requestid))
            .ToList();
        var thisUserProfitsBeforDiscount = allThisUserOrdersNumber
            .Where(obj => obj.Orderstatus
            .Equals("Arrived"))
            .ToList()
            .Sum(obj => obj.Finalprice);

        var totalThisUserProfits = thisUserProfitsBeforDiscount - (thisUserProfitsBeforDiscount * (decimal)0.05);

        currentUser.Profits = totalThisUserProfits;
        _context.Update(currentUser);
        _context.SaveChanges();

        ViewData["UserRequestsNumber"] = allThisRequestsNumber.Count();
        ViewData["TotalProfits"] = currentUser.Profits;

        return View(currentUser);
    }

    public IActionResult MyGifts()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        var category = _context.GiftstoreCategories.Where(obj => obj.Categoryid == currentUser.Categoryid).ToList();


        return View(category);
    }

    public IActionResult SpecificCategoryGifts(decimal? categoryId)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        var gifts = _context.GiftstoreGifts.Where(obj => obj.Categoryid == categoryId).ToList();

        return View(gifts);
    }

    public IActionResult Gifts(decimal? categoryId)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        GiftstoreUser? user = _context.GiftstoreUsers
            .SingleOrDefault(obj => obj.Userid == id && obj.Categoryid == currentUser.Categoryid);

        var gifts = _context.GiftstoreGifts.Where(obj => obj.Userid == id).ToList();
            
        return View(gifts);
    }
 
    public IActionResult Notification(decimal? Categoryid)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        var users = _context.GiftstoreUsers.Where(obj => obj.Roleid == 3).ToList();
        var notifications = _context.GiftstoreNotifications.ToList();

        IEnumerable<UsersNotifications>? model = from user in users
            join notification in notifications
                on user.Email equals notification.Email
            select new UsersNotifications
            {
                GiftstoreUser = user,
                GiftstoreNotification = notification
            };
        return View(model);
    }

    public IActionResult SenderRequest()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;


        var request = _context.GiftstoreSenderrequests.Where(obj=> obj.Makerid == id && obj.Requeststatus.Equals("Pending")).ToList();
        
        return View(request);
    }

    public async void DeleteRequest(decimal id)
    {
        GiftstoreSenderrequest? requests = await _context.GiftstoreSenderrequests.FindAsync(id);
        if (requests != null)
        {
            _context.GiftstoreSenderrequests.Remove(requests);
        }
        await _context.SaveChangesAsync();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SenderRequest(decimal? Senderid, decimal requestId, string? action)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

        GiftstoreSenderrequest? request = _context.GiftstoreSenderrequests.Where(obj => obj.Requestid == requestId).SingleOrDefault();

        GiftstoreGift? userGift = _context.GiftstoreGifts.Where(obj=> obj.Giftname.Equals(request.Giftname)).SingleOrDefault();

        if (action.Equals("Accepted"))
        {
            request.Requeststatus = "WaitToPay";
            _context.Update(request);
            _context.SaveChangesAsync();
            
            GiftstoreUser sender = _context.GiftstoreUsers.Where(x => x.Userid == Senderid).SingleOrDefault();
            _email.SendPaymentEmailToSender(sender.Userid,sender.Email,sender.Username, userGift.Giftid,request.Recipientaddress, requestId);
        }
        else
        {
            request.Requeststatus = "Reject";
            _context.Update(request);
            _context.SaveChangesAsync();
        }

        //DeleteRequest(requestId);

        var makerRequest = _context.GiftstoreSenderrequests.Where(obj => obj.Makerid == id && obj.Requeststatus.Equals("Pending")).ToList();
        return View(makerRequest);
    }



    public IActionResult Profits(decimal? Categoryid)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
        ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count(); 
        ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();

        var allThisRequestsNumber = _context.GiftstoreSenderrequests
            .Where(obj => obj.Makerid == id)
            .Select(obj => obj.Requestid)
            .ToList();
        var allThisUserOrdersNumber = _context.GiftstoreOrders
            .Where(obj => allThisRequestsNumber.Contains((decimal)obj.Requestid))
            .ToList();
        var thisUserProfitsBeforDiscount = allThisUserOrdersNumber
            .Where(obj => obj.Orderstatus
            .Equals("Arrived"))
            .ToList()
            .Sum(obj => obj.Finalprice);

        var totalThisUserProfits = thisUserProfitsBeforDiscount - (thisUserProfitsBeforDiscount * (decimal)0.05);

        currentUser.Profits = totalThisUserProfits;
        _context.Update(currentUser);
        _context.SaveChanges();

        ViewData["UserRequestsNumber"] = allThisRequestsNumber.Count();
        ViewData["UserPaidRequestsNumber"] = _context.GiftstoreSenderrequests.Count(obj => obj.Makerid == id && obj.Requeststatus.Equals("Paid"));
        ViewData["UserArrivedRequestsNumber"] = allThisUserOrdersNumber.Count(obj => obj.Orderstatus.Equals("Arrived"));

        ViewData["TotalProfits"] = currentUser.Profits;

        return View();
    }



    private bool GiftstoreGiftExists(decimal id) => (_context.GiftstoreGifts?.Any(e => e.Giftid == id)).GetValueOrDefault();
}