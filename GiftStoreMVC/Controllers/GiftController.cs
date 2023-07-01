using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;

namespace GiftStoreMVC.Controllers
{
    public class GiftController : Controller
    {
        private readonly ModelContext _context;

        public GiftController(ModelContext context)
        {
            _context = context;
        }

        // GET: Gift
        public async Task<IActionResult> Index()
        {
             
            //decimal? id = HttpContext.Session.GetInt32("UserId");
            //var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
            //ViewData["Username"] = currentUser.Username;
            //ViewData["Password"] = currentUser.Password;
            //ViewData["UserId"] = id;
            //ViewData["RoleId"] = currentUser.Roleid;
            //ViewData["CategoryId"] = currentUser.Categoryid;

            var modelContext = _context.GiftstoreGifts.Include(g => g.Category).Include(g => g.Order);
            return View(await modelContext.ToListAsync());
        }

        // GET: Gift/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftstoreGifts == null)
            {
                return NotFound();
            }

            var giftstoreGift = await _context.GiftstoreGifts
                .Include(g => g.Category)
                .Include(g => g.Order)
                .FirstOrDefaultAsync(m => m.Giftid == id);
            if (giftstoreGift == null)
            {
                return NotFound();
            }

            return View(giftstoreGift);
        }

        // GET: Gift/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.GiftstoreCategories, "Categoryid", "Categoryid");
            ViewData["Orderid"] = new SelectList(_context.GiftstoreOrders, "Orderid", "Orderid");
            return View();
        }

        // POST: Gift/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Giftid,Giftname,Giftprice,Imagepath,Giftavailability,Giftdescription,Categoryid,Orderid")] GiftstoreGift giftstoreGift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftstoreGift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.GiftstoreCategories, "Categoryid", "Categoryid", giftstoreGift.Categoryid);
            ViewData["Orderid"] = new SelectList(_context.GiftstoreOrders, "Orderid", "Orderid", giftstoreGift.Orderid);
            return View(giftstoreGift);
        }

        // GET: Gift/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftstoreGifts == null)
            {
                return NotFound();
            }

            var giftstoreGift = await _context.GiftstoreGifts.FindAsync(id);
            if (giftstoreGift == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.GiftstoreCategories, "Categoryid", "Categoryid", giftstoreGift.Categoryid);
            ViewData["Orderid"] = new SelectList(_context.GiftstoreOrders, "Orderid", "Orderid", giftstoreGift.Orderid);
            return View(giftstoreGift);
        }

        // POST: Gift/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Giftid,Giftname,Giftprice,Imagepath,Giftavailability,Giftdescription,Categoryid,Orderid")] GiftstoreGift giftstoreGift)
        {
            if (id != giftstoreGift.Giftid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.GiftstoreCategories, "Categoryid", "Categoryid", giftstoreGift.Categoryid);
            ViewData["Orderid"] = new SelectList(_context.GiftstoreOrders, "Orderid", "Orderid", giftstoreGift.Orderid);
            return View(giftstoreGift);
        }

        // GET: Gift/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            //decimal? id2 = HttpContext.Session.GetInt32("UserId");
            //var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
            //ViewData["Username"] = currentUser.Username;
            //ViewData["Password"] = currentUser.Password;
            //ViewData["UserId"] = id2;
            //ViewData["RoleId"] = currentUser.Roleid;
            //ViewData["CategoryId"] = currentUser.Categoryid;

            if (id == null || _context.GiftstoreGifts == null)
            {
                return NotFound();
            }

            var giftstoreGift = await _context.GiftstoreGifts
                .Include(g => g.Category)
                .Include(g => g.Order)
                .FirstOrDefaultAsync(m => m.Giftid == id);
            if (giftstoreGift == null)
            {
                return NotFound();
            }

            return View(giftstoreGift);
        }

        // POST: Gift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            //decimal? id2 = HttpContext.Session.GetInt32("UserId");
            //var currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
            //ViewData["Username"] = currentUser.Username;
            //ViewData["Password"] = currentUser.Password;
            //ViewData["UserId"] = id2;
            //ViewData["RoleId"] = currentUser.Roleid;
            //ViewData["CategoryId"] = currentUser.Categoryid;

            if (_context.GiftstoreGifts == null)
            {
                return Problem("Entity set 'ModelContext.GiftstoreGifts'  is null.");
            }
            var giftstoreGift = await _context.GiftstoreGifts.FindAsync(id);
            if (giftstoreGift != null)
            {
                _context.GiftstoreGifts.Remove(giftstoreGift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftstoreGiftExists(decimal id)
        {
          return (_context.GiftstoreGifts?.Any(e => e.Giftid == id)).GetValueOrDefault();
        }
    }
}
