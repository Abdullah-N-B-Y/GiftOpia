using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GiftStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        GiftstoreNotification notification;
        public HomeController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var users = _context.GiftstoreUsers.ToList();
            var categories = _context.GiftstoreCategories.ToList();

            var categoryUserNames = from user in users
                                    join category in categories
                                    on user.Categoryid equals category.Categoryid
                                    select new UserCagtegory
                                    {
                                        UserId = user.Userid,
                                        UserName = user.Name,
                                        UserEmail = user.Email,
                                        UserPhone = user.Phonenumber,
                                        UserImagepath = user.Imagepath,
                                        UserImage = user.UserImage,
                                        CategoryId = category.Categoryid,
                                        CategoryName = category.Categoryname,
                                        Categorydescription = category.Categorydescription,
                                        CategoryImagepath = category.Imagepath,
                                        CategoryImage = category.CategoryImage
                                    };
            return View(categories);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Categories()
        {
            var categories = _context.GiftstoreCategories.ToList();
            return View(categories);
        }

        public IActionResult Gifts(decimal? Categoryid)
        {
            var modelContext = _context.GiftstoreGifts.Where(obj => obj.Categoryid == Categoryid).ToList();

            return View(modelContext);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}