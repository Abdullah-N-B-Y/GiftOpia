using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftStoreMVC.Controllers
{
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
            decimal? id = HttpContext.Session.GetInt32("UserId");
            GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
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
    }
}
