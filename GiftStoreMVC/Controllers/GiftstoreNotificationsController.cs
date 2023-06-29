using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;

namespace GiftStore.Controllers
{
    public class GiftstoreNotificationsController : Controller
    {
        private readonly ModelContext _context;

        public GiftstoreNotificationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: GiftstoreNotifications
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftstoreNotifications.ToList();
            return View(modelContext);
        }

        // GET: GiftstoreNotifications/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftstoreNotifications == null)
            {
                return NotFound();
            }

            var giftstoreNotification = await _context.GiftstoreNotifications
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Notificationlid == id);
            if (giftstoreNotification == null)
            {
                return NotFound();
            }

            return View(giftstoreNotification);
        }

        // GET: GiftstoreNotifications/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email");
            return View();
        }

        // POST: GiftstoreNotifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Notificationlid,Notificationcontent,Notificationdate,Isread,Userid")] GiftstoreNotification giftstoreNotification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftstoreNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email", giftstoreNotification.Userid);
            return View(giftstoreNotification);
        }

        // GET: GiftstoreNotifications/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftstoreNotifications == null)
            {
                return NotFound();
            }

            var giftstoreNotification = await _context.GiftstoreNotifications.FindAsync(id);
            if (giftstoreNotification == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email", giftstoreNotification.Userid);
            return View(giftstoreNotification);
        }

        // POST: GiftstoreNotifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Notificationlid,Notificationcontent,Notificationdate,Isread,Userid")] GiftstoreNotification giftstoreNotification)
        {
            if (id != giftstoreNotification.Notificationlid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftstoreNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftstoreNotificationExists(giftstoreNotification.Notificationlid))
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
            ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Email", giftstoreNotification.Userid);
            return View(giftstoreNotification);
        }

        // GET: GiftstoreNotifications/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftstoreNotifications == null)
            {
                return NotFound();
            }

            var giftstoreNotification = await _context.GiftstoreNotifications
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Notificationlid == id);
            if (giftstoreNotification == null)
            {
                return NotFound();
            }

            return View(giftstoreNotification);
        }

        // POST: GiftstoreNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftstoreNotifications == null)
            {
                return Problem("Entity set 'ModelContext.GiftstoreNotifications'  is null.");
            }
            var giftstoreNotification = await _context.GiftstoreNotifications.FindAsync(id);
            if (giftstoreNotification != null)
            {
                _context.GiftstoreNotifications.Remove(giftstoreNotification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftstoreNotificationExists(decimal id)
        {
          return (_context.GiftstoreNotifications?.Any(e => e.Notificationlid == id)).GetValueOrDefault();
        }
    }
}
