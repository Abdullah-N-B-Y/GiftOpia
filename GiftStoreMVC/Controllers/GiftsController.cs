using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Query;

namespace GiftStoreMVC.Controllers;

public class GiftsController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GiftsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;

        IIncludableQueryable<GiftstoreGift, GiftstoreUser>? modelContext = _context.GiftstoreGifts.Include(g => g.Category).Include(g => g.User);
        return View(await modelContext.ToListAsync());
    }

    public async Task<IActionResult> Details(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser?.Roleid;

        if (id == null || _context.GiftstoreGifts == null)

        {
            return NotFound();
        }

        GiftstoreGift? giftstoreGift = await _context.GiftstoreGifts
            .Include(g => g.Category)
            .Include(g => g.User)
            .FirstOrDefaultAsync(m => m.Giftid == id);
        if (giftstoreGift == null)
        {
            return NotFound();
        }

        return View(giftstoreGift);
    }

    public IActionResult Create()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;

        ViewData["Categoryid"] = currentUser.Categoryid;
        ViewData["Userid"] = id;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Giftid,Giftname,Giftprice,GiftImage,Giftdescription,Categoryid,Userid")] GiftstoreGift giftstoreGift, string? Giftavailability)
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser?.Roleid;

        if (ModelState.IsValid)
        {
            if (giftstoreGift.GiftImage != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + giftstoreGift.GiftImage.FileName;
                string path = Path.Combine(wwwRootPath, "GiftsImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await giftstoreGift.GiftImage.CopyToAsync(fileStream);
                }
                giftstoreGift.Imagepath = fileName;
            }

            giftstoreGift.Giftavailability = Giftavailability == "yes" ? 1 : 0; ;
            _context.Add(giftstoreGift);
            await _context.SaveChangesAsync();
            return RedirectToAction("Gifts", "Maker", giftstoreGift);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser?.Roleid;

        ViewData["Categoryid"] = currentUser.Categoryid;
        ViewData["Userid"] = id2;

        if (id == null || _context.GiftstoreGifts == null)
        {
            return NotFound();
        }

        GiftstoreGift? giftstoreGift = await _context.GiftstoreGifts.FindAsync(id);
        if (giftstoreGift == null)
        {
            return NotFound();
        }
        return View(giftstoreGift);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(decimal id, [Bind("Giftid,Giftname,Giftprice,GiftImage,Giftdescription,Categoryid,Userid")] GiftstoreGift giftstoreGift, string? Giftavailability)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser?.Roleid;

        if (id != giftstoreGift.Giftid)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (giftstoreGift.GiftImage != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + giftstoreGift.GiftImage.FileName;
                    string path = Path.Combine(wwwRootPath, "GiftsImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await giftstoreGift.GiftImage.CopyToAsync(fileStream);
                    }
                    giftstoreGift.Imagepath = fileName;
                }

                giftstoreGift.Giftavailability = Giftavailability == "yes" ? 1 : 0;
                _context.Update(giftstoreGift);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiftstoreGiftExists(giftstoreGift.Giftid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Gifts", "Maker", giftstoreGift);
        }
        ViewData["Categoryid"] = new SelectList(_context.GiftstoreCategories, "Categoryid", "Categoryid", giftstoreGift.Categoryid);
        ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreGift.Userid);
        return View(giftstoreGift);
    }

    public async Task<IActionResult> Delete(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser?.Roleid;

        if (id == null || _context.GiftstoreGifts == null)
        {
            return NotFound();
        }

        GiftstoreGift? giftstoreGift = await _context.GiftstoreGifts
            .Include(g => g.Category)
            .Include(g => g.User)
            .FirstOrDefaultAsync(m => m.Giftid == id);
        if (giftstoreGift == null)
        {
            return NotFound();
        }

        return View(giftstoreGift);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(decimal id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser?.Username;
        ViewData["Password"] = currentUser?.Password;
        ViewData["UserId"] = id2;
        ViewData["RoleId"] = currentUser?.Roleid;

        if (_context.GiftstoreGifts == null)
        {
            return Problem("Entity set 'ModelContext.GiftstoreGifts'  is null.");
        }
        GiftstoreGift? giftstoreGift = await _context.GiftstoreGifts.FindAsync(id);
        if (giftstoreGift != null)
        {
            _context.GiftstoreGifts.Remove(giftstoreGift);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction("Gifts", "Maker", giftstoreGift);
    }

    private bool GiftstoreGiftExists(decimal id) => (_context.GiftstoreGifts?.Any(e => e.Giftid == id)).GetValueOrDefault();
}