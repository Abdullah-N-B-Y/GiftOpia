using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiftStoreMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //GiftstoreNotification notification;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;
            ViewData["RoleId"] = currentUser.Roleid;

            ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
            ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count();
            ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();


            return View(currentUser);
        }


        public async Task<IActionResult> Category()
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;
            ViewData["RoleId"] = currentUser.Roleid;
            var modelContext = _context.GiftstoreCategories.ToList();

            return View("~/Views/Admin/Categories/Category.cshtml", modelContext);
        }


        public async Task<IActionResult> Details(decimal? id)
        {
            //if (id == null || _context.GiftstoreCategories == null)
            //{
            //    return NotFound();
            //}

            //var giftstoreCategory = await _context.GiftstoreCategories
            //    .Include(g => g.User)
            //    .FirstOrDefaultAsync(m => m.Categoryid == id);
            //if (giftstoreCategory == null)
            //{
            //    return NotFound();
            //}

            //return View(giftstoreCategory);
            return View("~/Views/Admin/Categories/Details.cshtml");
        }


        public IActionResult Create()
        {
            //ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email");
            return View("~/Views/Admin/Categories/Create.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Categoryid,Categoryname,Categorydescription,CategoryImage")] GiftstoreCategory giftstoreCategory)
        {
            if (ModelState.IsValid)
            {
                if (giftstoreCategory.CategoryImage != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + giftstoreCategory.CategoryImage.FileName;
                    string path = Path.Combine(wwwRootPath, "CategoriesImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await giftstoreCategory.CategoryImage.CopyToAsync(fileStream);
                    }
                    giftstoreCategory.Imagepath = fileName;
                }

                _context.Add(giftstoreCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email");
            return View("~/Views/Admin/Categories/Create.cshtml", giftstoreCategory);
        }


        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftstoreCategories == null)
            {
                return NotFound();
            }

            var giftstoreCategory = await _context.GiftstoreCategories.FindAsync(id);
            if (giftstoreCategory == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email");
            return View("~/Views/Admin/Categories/Edit.cshtml", giftstoreCategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Categoryid,Categoryname,Imagepath,Categorydescription,Userid")] GiftstoreCategory giftstoreCategory)
        {
            if (id != giftstoreCategory.Categoryid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftstoreCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftstoreCategoryExists(giftstoreCategory.Categoryid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email");
            return View("~/Views/Admin/Categories/Edit.cshtml", giftstoreCategory);
        }


        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftstoreCategories == null)
            {
                return NotFound();
            }

            var giftstoreCategory = await _context.GiftstoreCategories.FirstOrDefaultAsync(m => m.Categoryid == id);
            if (giftstoreCategory == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Categories/Delete.cshtml", giftstoreCategory);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftstoreCategories == null)
            {
                return Problem("Entity set 'ModelContext.GiftstoreCategories'  is null.");
            }
            var giftstoreCategory = await _context.GiftstoreCategories.FindAsync(id);
            if (giftstoreCategory != null)
            {
                _context.GiftstoreCategories.Remove(giftstoreCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("~/Views/Admin/Categories/Delete.cshtml", nameof(Index));
        }

        public async void D1(decimal id)
        {
            var giftstoreNotification = await _context.GiftstoreNotifications.FindAsync(id);
            if (giftstoreNotification != null)
            {
                _context.GiftstoreNotifications.Remove(giftstoreNotification);
            }
            await _context.SaveChangesAsync();
        }

        private bool GiftstoreCategoryExists(decimal id)
        {
            return (_context.GiftstoreCategories?.Any(e => e.Categoryid == id)).GetValueOrDefault();
        }

        //End category Section

        
        public IActionResult AllCategories()
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
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
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
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
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;
            ViewData["RoleId"] = currentUser.Roleid;
            ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

            var users = _context.GiftstoreUsers.Where(obj => obj.Roleid == 2).ToList();
            var notifications = _context.GiftstoreNotifications.ToList();

            var model = from user in users
                        join notification in notifications
                        on user.Email equals notification.Email
                        select new UsersNotifications
                        {
                            GiftstoreUser = user,
                            GiftstoreNotification = notification
                        };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Notification(decimal? Userid, decimal Notificationlid, string? action)
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;
            ViewData["RoleId"] = currentUser.Roleid;
            ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

            var user = _context.GiftstoreUsers.Where(obj => obj.Userid == Userid).SingleOrDefault();
            user.Approvalstatus = action;
            _context.Update(user);
            _context.SaveChangesAsync();

            D1(Notificationlid);

            var users = _context.GiftstoreUsers.ToList();
            var notifications = _context.GiftstoreNotifications.ToList();

            var model = from u in users
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
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;
            ViewData["RoleId"] = currentUser.Roleid;

            ViewData["NumberOfUsers"] = _context.GiftstoreUsers.Count();
            ViewData["NumberOfGifts"] = _context.GiftstoreGifts.Count();  //Anas Majdoub new work
            ViewData["NumberOfCategories"] = _context.GiftstoreCategories.Count();

            var profits = (double) _context.GiftstoreOrders.Where(obj=> obj.Orderstatus.Equals("Arrived")).ToList().Sum(obj => obj.Finalprice);
            ViewData["TotalProfits"] = profits * 0.05;
            return View();
        }



        public IActionResult Reportes(decimal? Categoryid, DateTime period)
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
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


            var report = 
                (
                from user in users
                join request in requests on user.Userid equals request.Senderid
                join gift in gifts on request.Giftid equals gift.Giftid
                join order in orders on gift.Orderid equals order.Orderid
                where order.Orderstatus.Equals("Arrived") && order.Arrivaldate >= period
                select new Reprotes
                {
                    userName=user.Name,
                    totalPrice = user.Profits,
                    createDate = order.Arrivaldate,
                }
                );

    

            return View(report.ToList());
        }

        public IActionResult UsersIndex()
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;
            ViewData["RoleId"] = currentUser.Roleid;
            ViewData["RoleName"] = HttpContext.Session.GetString("RoleName"); ;

            var users = _context.GiftstoreUsers.ToList();

            return View("~/Views/Admin/Users/UsersIndex.cshtml", users);

        }


    }
}
