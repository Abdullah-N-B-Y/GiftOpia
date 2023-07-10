using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftStoreMVC.Controllers
{
    public class TestController : Controller
    {
        private readonly ModelContext _context;
        public TestController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Test()
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

            var v = _context.GiftstoreOrders.ToList();
            return View(v);
        }

        [HttpPost]
        public IActionResult Test(DateTime? startDate, DateTime? endDate)
        {
            var model = _context.GiftstoreOrders.ToList();
            if (startDate == null && endDate == null)
                return View(model);
            else if (startDate == null && endDate != null)
            {
                var res = model.Where(x => x.Orderdate <= endDate).ToList();
                return View(res);
            }
            else if (startDate != null && endDate == null)
            {
                var res = model.Where(x => x.Arrivaldate >= startDate).ToList();
                return View(res);
            }
            else
            {
                var res = model.Where(x => x.Arrivaldate >= startDate && x.Orderdate <= endDate).ToList();
                return View(res);
            }
        }
    }
}
