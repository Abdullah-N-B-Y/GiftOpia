using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace GiftStoreMVC.Controllers;

public class AdminController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IEmail _email;

    //GiftstoreNotification notification;
    public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment,IEmail email)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _email = email;
    }
    public IActionResult Index()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == id).SingleOrDefault();
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


        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj=> obj.Isread == false);

        List<DataPoint> dataPoints = new List<DataPoint>();
        dataPoints.Add(new DataPoint("Admin",_context.GiftstoreUsers.Count(x=>x.Roleid==1)));
        dataPoints.Add(new DataPoint("Maker",_context.GiftstoreUsers.Count(x=>x.Roleid==2)));
        dataPoints.Add(new DataPoint("Sender",  _context.GiftstoreUsers.Count(x => x.Roleid == 3)));
        dataPoints.Add(new DataPoint("Driver",  _context.GiftstoreUsers.Count(x => x.Roleid == 4)));
        dataPoints.Add(new DataPoint("Category", _context.GiftstoreCategories.Count()));
        dataPoints.Add(new DataPoint("Gift", _context.GiftstoreGifts.Count()));
        dataPoints.Add(new DataPoint("Arrived gift", _context.GiftstoreOrders.Where(obj=>obj.Orderstatus.Equals("Arrived")).Count()));

        ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);


        var user = _context.GiftstoreUsers.ToList().Take(5);
        var Testimonials = _context.GiftstoreTestimonials.ToList().Take(5);
        var modle = Tuple.Create<IEnumerable<GiftstoreUser>, IEnumerable<GiftstoreTestimonial>>(user, Testimonials);





        return View(modle);
    }


    public async Task<IActionResult> Category()
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
        var modelContext = _context.GiftstoreCategories.ToList();

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        return View("~/Views/Admin/Categories/Category.cshtml", modelContext);
    }
    
    private bool GiftstoreCategoryExists(decimal id) => (_context.GiftstoreCategories?.Any(e => e.Categoryid == id)).GetValueOrDefault();

    //End category Section


    public IActionResult AllCategories()
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
        var modelContext = _context.GiftstoreCategories.ToList();

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        return View(modelContext);
    }

    public IActionResult CategoryGifts(decimal? Categoryid)
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
        var modelContext = _context.GiftstoreGifts.Where(obj=> obj.Categoryid == Categoryid).ToList();

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        return View(modelContext);
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

        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        var users = _context.GiftstoreUsers.Where(obj => obj.Roleid == 2 || obj.Roleid == 4).ToList();
        var notifications = _context.GiftstoreNotifications.ToList();

        IEnumerable<UsersNotifications>? model = from u in users
                                                 join notification in notifications
                                                 on u.Email equals notification.Email
                                                 where u.Approvalstatus.Equals("Pending")
                                                 select new UsersNotifications
                                                 {
                                                     GiftstoreUser = u,
                                                     GiftstoreNotification = notification
                                                 };
        return View(model);
    }

    public async void DeleteNotification(decimal id)
    {
        GiftstoreNotification? giftStoreNotification = await _context.GiftstoreNotifications.FindAsync(id);
        if (giftStoreNotification != null)
        {
            _context.GiftstoreNotifications.Remove(giftStoreNotification);
        }
        await _context.SaveChangesAsync();
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Notification(decimal? Userid, decimal Notificationlid, string? action)
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

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == Userid).SingleOrDefault();
        user.Approvalstatus = action;
        _context.Update(user);
        _context.SaveChangesAsync();
        
        _email.SendEmailToUser(user.Email,user.Username,action);
        
        DeleteNotification(Notificationlid);

        var users = _context.GiftstoreUsers.ToList();
        var notifications = _context.GiftstoreNotifications.ToList();

        IEnumerable<UsersNotifications>? model = from u in users
                                                 join notification in notifications
                                                 on u.Email equals notification.Email
                                                 where u.Approvalstatus.Equals("Pending")
                                                 select new UsersNotifications
                                                 {
                                                     GiftstoreUser = u,
                                                     GiftstoreNotification = notification
                                                 };
        return View(model);
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

        //ViewBag.Notifications = _context.GiftstoreNotifications.ToList();
        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        var allPaidRequest = _context.GiftstoreSenderrequests.Count(obj=>obj.Requeststatus.Equals("Paid"));
        

        ViewData["PaidGifts"] = allPaidRequest;
        var allArrivedOrders = _context.GiftstoreOrders.Where(obj => obj.Orderstatus.Equals("Arrived")).ToList();
        ViewData["ArrivedGifts"] = allArrivedOrders.Count();


        double profits = (double) allArrivedOrders.Sum(obj => obj.Finalprice);
        currentUser.Profits = (decimal)(profits * 0.05);
        _context.Update(currentUser);
        _context.SaveChanges();

        ViewData["TotalProfits"] = profits * 0.05;


        return View();

    }


    public IActionResult SearchInterval(DateTime? startDate, DateTime? endDate)
    {
        var users = _context.GiftstoreUsers.ToList();
        var requests = _context.GiftstoreSenderrequests.ToList();
        var gifts = _context.GiftstoreGifts.ToList();
        
        IEnumerable<GiftstoreOrder>orders = null;

        if (startDate == null && endDate == null)
        { 
            orders = _context.GiftstoreOrders.ToList();
        }
        else if (startDate == null && endDate != null)
        {
            orders = _context.GiftstoreOrders.Where(x => x.Arrivaldate <= endDate).ToList();
        }
        else if (startDate != null && endDate == null)
        {
            orders = _context.GiftstoreOrders.Where(x => x.Arrivaldate >= startDate).ToList();
        }
        else
        {
            orders = _context.GiftstoreOrders.Where(x => x.Arrivaldate >= startDate && x.Arrivaldate <= endDate).ToList();
        }

        IEnumerable<Reprotes>? report =
        from user in users
        join request in requests on user.Userid equals request.Senderid
        join order in orders on request.Requestid equals order.Requestid
        //where order.Orderstatus.Equals("Arrived") && order.Arrivaldate >= period
        select new Reprotes
        {
            userName = user.Name,
            totalPrice = order.Finalprice,
            createDate = order.Arrivaldate,
        };

        return RedirectToAction("Reportes", report.ToList());
    }
    public IActionResult Reportes(decimal? Categoryid, DateTime period)
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

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Userid == id && obj.Isread == false);

        var users = _context.GiftstoreUsers.ToList();
        var requests = _context.GiftstoreSenderrequests.ToList();
        var gifts = _context.GiftstoreGifts.ToList();
        var orders = _context.GiftstoreOrders.ToList();


        IEnumerable<Reprotes>? report = 
        from user in users
        join request in requests on user.Userid equals request.Senderid
        join order in orders on request.Requestid equals order.Requestid
        //where order.Orderstatus.Equals("Arrived") && order.Arrivaldate >= period
        select new Reprotes
        {
            userName=user.Name,
            totalPrice = order.Finalprice,
            createDate = order.Arrivaldate,
        };

        return View(report.ToList());
    }

    public IActionResult ManagePages()
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

        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName");

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Userid == id && obj.Isread == false);

        return RedirectToAction("Index","GiftstorePages");
    }

    public IActionResult BanUnbanUsers()
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

        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName");

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Userid == id && obj.Isread == false);

        IEnumerable<GiftstoreUser> banUsers = _context.GiftstoreUsers.Where(obj => obj.Approvalstatus.Equals("Banned")).ToList();
        return View(banUsers);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BanUnbanUsers(decimal Userid, string action)
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

        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName");

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Userid == id && obj.Isread == false);

        var bannededUser = _context.GiftstoreUsers.Where(obj => obj.Userid == Userid).SingleOrDefault();
        bannededUser.Approvalstatus = action;
        _context.Update(bannededUser);
        _context.SaveChanges();
        IEnumerable<GiftstoreUser> banUsers = _context.GiftstoreUsers.Where(obj => obj.Approvalstatus.Equals("Banned")).ToList();
        return View(banUsers);
    }
    public IActionResult UsersIndex()
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

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Userid == id && obj.Isread == false);

        var users = _context.GiftstoreUsers.ToList();

        return View("~/Views/Admin/Users/UsersIndex.cshtml", users);

    }


}