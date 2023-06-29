using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

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
                var categories = _context.GiftstoreCategories.ToList();
                //var model = Tuple.Create<IEnumerable<GiftstoreCategory>,>
                return View(categories);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}
