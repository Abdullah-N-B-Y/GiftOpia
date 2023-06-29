using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;

namespace GiftStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            decimal? id = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            ViewData["Username"] = currentUser.Username;
            ViewData["Password"] = currentUser.Password;
            ViewData["UserId"] = id;

            var modelContext = _context.GiftstoreCategories.ToList();
            
            return View(modelContext);
        }

        // GET: Category/Details/5
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
            return View();
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            //ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email");
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return View(giftstoreCategory);
        }

        // GET: Category/Edit/5
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
            return View(giftstoreCategory);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return View(giftstoreCategory);
        }

        // GET: Category/Delete/5
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

            return View(giftstoreCategory);
        }

        // POST: Category/Delete/5
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
            return RedirectToAction(nameof(Index));
        }

        private bool GiftstoreCategoryExists(decimal id)
        {
          return (_context.GiftstoreCategories?.Any(e => e.Categoryid == id)).GetValueOrDefault();
        }
    }
}
