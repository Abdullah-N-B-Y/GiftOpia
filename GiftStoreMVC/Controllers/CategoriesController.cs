using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Hosting;

namespace GiftStoreMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CategoriesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Categories
    public async Task<IActionResult> Index()
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        return _context.GiftstoreCategories != null ? 
            View(await _context.GiftstoreCategories.ToListAsync()) :
            Problem("Entity set 'ModelContext.GiftstoreCategories'  is null.");
    }

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        if (id == null || _context.GiftstoreCategories == null)
        {
            return NotFound();
        }

        GiftstoreCategory? giftstoreCategory = await _context.GiftstoreCategories
            .FirstOrDefaultAsync(m => m.Categoryid == id);
        if (giftstoreCategory == null)
        {
            return NotFound();
        }

        return View(giftstoreCategory);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Categoryid,Categoryname,CategoryImage,Categorydescription")] GiftstoreCategory giftstoreCategory)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

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

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        if (id == null || _context.GiftstoreCategories == null)
        {
            return NotFound();
        }

        GiftstoreCategory? giftstoreCategory = await _context.GiftstoreCategories.FindAsync(id);
        if (giftstoreCategory == null)
        {
            return NotFound();
        }
        return View(giftstoreCategory);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(decimal id, [Bind("Categoryid,Categoryname,CategoryImage,Categorydescription")] GiftstoreCategory giftstoreCategory)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        if (id != giftstoreCategory.Categoryid)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (giftstoreCategory.CategoryImage != null)
            {
                string fileName = Guid.NewGuid().ToString() + giftstoreCategory.CategoryImage.FileName;
                string path = Path.Combine(wwwRootPath, "CategoriesImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await giftstoreCategory.CategoryImage.CopyToAsync(fileStream);
                }
                giftstoreCategory.Imagepath = fileName;
            }
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
        return View(giftstoreCategory);
    }

    // GET: Categories/Delete/5
    public async Task<IActionResult> Delete(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        if (id == null || _context.GiftstoreCategories == null)
        {
            return NotFound();
        }

        GiftstoreCategory? giftstoreCategory = await _context.GiftstoreCategories
            .FirstOrDefaultAsync(m => m.Categoryid == id);
        if (giftstoreCategory == null)
        {
            return NotFound();
        }

        return View(giftstoreCategory);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(decimal id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        if (_context.GiftstoreCategories == null)
        {
            return Problem("Entity set 'ModelContext.GiftstoreCategories'  is null.");
        }
        GiftstoreCategory? giftstoreCategory = await _context.GiftstoreCategories.FindAsync(id);
        if (giftstoreCategory != null)
        {
            _context.GiftstoreCategories.Remove(giftstoreCategory);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GiftstoreCategoryExists(decimal id) => (_context.GiftstoreCategories?.Any(e => e.Categoryid == id)).GetValueOrDefault();
}