using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

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
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;
        return View(currentUser);
    }

    public IActionResult MyGifts()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;
        ViewData["CategoryId"] = currentUser?.Categoryid;
        
        var category = _context.GiftstoreCategories.Where(obj => obj.Categoryid == currentUser.Categoryid).ToList();


        return View(category);
    }

    public IActionResult SpecificCategoryGifts(decimal? categoryId)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;
        ViewData["CategoryId"] = currentUser?.Categoryid;

        var gifts = _context.GiftstoreGifts.Where(obj => obj.Categoryid == categoryId).ToList();

        return View(gifts);
    }

    public IActionResult Gifts(decimal? categoryId)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;
        ViewData["CategoryId"] = currentUser?.Categoryid;
        //var users = _context.GiftstoreUsers.ToList();
        //var categories = _context.GiftstoreCategories.ToList();
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
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

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
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

        //var users = _context.GiftstoreUsers.Where(obj => obj.Roleid == 2).ToList();
        //var notifications = _context.GiftstoreNotifications.ToList();

        //var model = from user in users
        //            join notification in notifications
        //            on user.Email equals notification.Email
        //            select new UsersNotifications
        //            {
        //                GiftstoreUser = user,
        //                GiftstoreNotification = notification
        //            };
        
        var request = _context.GiftstoreSenderrequests.Where(obj=> obj.Makerid == id && obj.Requeststatus.Equals("Pending")).ToList();
        
        return View(request);
    }

    public async void D1(decimal id)
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
    public IActionResult SenderRequest(decimal? Senderid, decimal Requestid, string? action)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

        GiftstoreSenderrequest? request = _context.GiftstoreSenderrequests.Where(obj => obj.Requestid == Requestid).SingleOrDefault();

        var userGift = _context.GiftstoreGifts.Where(obj=> obj.Giftname.Equals(request.Giftname)).SingleOrDefault();

        if (action.Equals("Accepted"))
        {
            request.Requeststatus = "WaitToPay";
            _context.Update(request);
            _context.SaveChangesAsync();
            
            GiftstoreUser sender = _context.GiftstoreUsers.Where(x => x.Userid == Senderid).SingleOrDefault();
            _email.SendEmail(sender.Email,sender.Username, userGift.Giftid,request.Recipientaddress);
         

            //Email for sender that maker accept his gift
        }
        else
        {
            //Email for sender that maker reject his gift
        }

        D1(Requestid);

        var makerRequest = _context.GiftstoreSenderrequests.Where(obj => obj.Makerid == id && obj.Requeststatus.Equals("Pending")).ToList();
        return View(makerRequest);
    }



    public IActionResult Profits(decimal? Categoryid)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;

        ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
        ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count(); 
        ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();

        ViewData["TotalProfits"] = (double)_context.GiftstoreOrders.Where(obj => obj.Orderstatus.Equals("Arrived")).ToList().Sum(obj => obj.Finalprice);
        return View();
    }



    private bool GiftstoreGiftExists(decimal id) => (_context.GiftstoreGifts?.Any(e => e.Giftid == id)).GetValueOrDefault();
}