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
    public class GiftstorePagesController : Controller
    {
        private readonly ModelContext _context;

        public GiftstorePagesController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftstorePages
        public async Task<IActionResult> Index()
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

            var modelContext = _context.GiftstorePages.Include(g => g.Admin);
            return View(await modelContext.ToListAsync());
        }

        // GET: GiftstorePages/Details/5
        public async Task<IActionResult> Details(decimal? id)
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

            if (id == null || _context.GiftstorePages == null)
            {
                return NotFound();
            }

            var giftstorePage = await _context.GiftstorePages
                .Include(g => g.Admin)
                .FirstOrDefaultAsync(m => m.Pageid == id);
            if (giftstorePage == null)
            {
                return NotFound();
            }

            return View(giftstorePage);
        }

        // GET: GiftstorePages/Create
        public IActionResult Create()
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

            ViewData["Adminid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid");
            return View();
        }

        // POST: GiftstorePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pageid,Pagetitle,Pagecontent,Adminid")] GiftstorePage giftstorePage)
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

            if (ModelState.IsValid)
            {
                _context.Add(giftstorePage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Adminid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstorePage.Adminid);
            return View(giftstorePage);
        }

        // GET: GiftstorePages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
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

            if (id == null || _context.GiftstorePages == null)
            {
                return NotFound();
            }

            var giftstorePage = await _context.GiftstorePages.FindAsync(id);
            if (giftstorePage == null)
            {
                return NotFound();
            }
            ViewData["Adminid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstorePage.Adminid);
            return View(giftstorePage);
        }

        // POST: GiftstorePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Pageid,Pagetitle,Pagecontent,Adminid")] GiftstorePage giftstorePage)
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

            if (id != giftstorePage.Pageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftstorePage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftstorePageExists(giftstorePage.Pageid))
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
            ViewData["Adminid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstorePage.Adminid);
            return View(giftstorePage);
        }

        // GET: GiftstorePages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
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

            if (id == null || _context.GiftstorePages == null)
            {
                return NotFound();
            }

            var giftstorePage = await _context.GiftstorePages
                .Include(g => g.Admin)
                .FirstOrDefaultAsync(m => m.Pageid == id);
            if (giftstorePage == null)
            {
                return NotFound();
            }

            return View(giftstorePage);
        }

        // POST: GiftstorePages/Delete/5
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
            ViewData["UserId"] = id2;
            ViewData["RoleId"] = currentUser.Roleid;
            ViewData["ImagePath"] = currentUser.Imagepath;
            if (_context.GiftstorePages == null)
            {
                return Problem("Entity set 'ModelContext.GiftstorePages'  is null.");
            }
            var giftstorePage = await _context.GiftstorePages.FindAsync(id);
            if (giftstorePage != null)
            {
                _context.GiftstorePages.Remove(giftstorePage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftstorePageExists(decimal id)
        {
          return (_context.GiftstorePages?.Any(e => e.Pageid == id)).GetValueOrDefault();
        }
    }
}
