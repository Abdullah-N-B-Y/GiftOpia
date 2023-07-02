using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;

        ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
        ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count();
        ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();

        // AHmad
        return View(currentUser);
    }


    public async Task<IActionResult> Category()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        var modelContext = _context.GiftstoreCategories.ToList();

        return View("~/Views/Admin/Categories/Category.cshtml", modelContext);
    }
    
    private bool GiftstoreCategoryExists(decimal id) => (_context.GiftstoreCategories?.Any(e => e.Categoryid == id)).GetValueOrDefault();

    //End category Section


    public IActionResult AllCategories()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;

        var modelContext = _context.GiftstoreCategories.ToList();

        return View(modelContext);
    }

    public IActionResult CategoryGifts(decimal? Categoryid)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        var modelContext = _context.GiftstoreGifts.Where(obj=> obj.Categoryid == Categoryid).ToList();

        return View(modelContext);
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

        var users = _context.GiftstoreUsers.Where(obj => obj.Roleid == 2).ToList();
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

    public async void D1(decimal id)
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
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

        GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == Userid).SingleOrDefault();
        user.Approvalstatus = action;
        _context.Update(user);
        _context.SaveChangesAsync();
        
        _email.SendEmailToUser(user.Email,user.Username,action);
        
        //D1(Notificationlid);

        var users = _context.GiftstoreUsers.ToList();
        var notifications = _context.GiftstoreNotifications.ToList();

        IEnumerable<UsersNotifications>? model = from u in users
            join notification in notifications
                on u.Email equals notification.Email
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
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;

        ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
        ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count();  //Anas Majdoub new work
        ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();

        double profits = (double) _context.GiftstoreOrders.Where(obj=> obj.Orderstatus.Equals("Arrived")).ToList().Sum(obj => obj.Finalprice);
        ViewData["TotalProfits"] = profits * 0.05;
        return View();

    }


    public IActionResult Reportes(decimal? Categoryid, DateTime period)
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
            totalPrice = user.Profits,
            createDate = order.Arrivaldate,
        };

        return View(report.ToList());
    }

    public IActionResult UsersIndex()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Password"] = currentUser.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

        var users = _context.GiftstoreUsers.ToList();

        return View("~/Views/Admin/Users/UsersIndex.cshtml", users);

    }


}