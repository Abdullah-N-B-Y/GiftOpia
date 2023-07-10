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
    public class SenderrequestsController : Controller
    {
        private readonly ModelContext _context;

        public SenderrequestsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Senderrequests
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.GiftstoreSenderrequests.Include(g => g.Maker).Include(g => g.Sender);
            return View(await modelContext.ToListAsync());
        }

        // GET: Senderrequests/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftstoreSenderrequests == null)
            {
                return NotFound();
            }

            var giftstoreSenderrequest = await _context.GiftstoreSenderrequests
                .Include(g => g.Maker)
                .Include(g => g.Sender)
                .FirstOrDefaultAsync(m => m.Requestid == id);
            if (giftstoreSenderrequest == null)
            {
                return NotFound();
            }

            return View(giftstoreSenderrequest);
        }

        // GET: Senderrequests/Create
        public IActionResult Create()
        {
            ViewData["Makerid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid");
            ViewData["Senderid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid");
            return View();
        }

        // POST: Senderrequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Requestid,Recipientname,Recipientaddress,Requeststatus,Requestdate,Senderid,Makerid,Sendername,Giftname,Giftprice")] GiftstoreSenderrequest giftstoreSenderrequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giftstoreSenderrequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Makerid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreSenderrequest.Makerid);
            ViewData["Senderid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreSenderrequest.Senderid);
            return View(giftstoreSenderrequest);
        }

        // GET: Senderrequests/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftstoreSenderrequests == null)
            {
                return NotFound();
            }

            var giftstoreSenderrequest = await _context.GiftstoreSenderrequests.FindAsync(id);
            if (giftstoreSenderrequest == null)
            {
                return NotFound();
            }
            ViewData["Makerid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreSenderrequest.Makerid);
            ViewData["Senderid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreSenderrequest.Senderid);
            return View(giftstoreSenderrequest);
        }

        // POST: Senderrequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Requestid,Recipientname,Recipientaddress,Requeststatus,Requestdate,Senderid,Makerid,Sendername,Giftname,Giftprice")] GiftstoreSenderrequest giftstoreSenderrequest)
        {
            if (id != giftstoreSenderrequest.Requestid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftstoreSenderrequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftstoreSenderrequestExists(giftstoreSenderrequest.Requestid))
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
            ViewData["Makerid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreSenderrequest.Makerid);
            ViewData["Senderid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreSenderrequest.Senderid);
            return View(giftstoreSenderrequest);
        }

        // GET: Senderrequests/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftstoreSenderrequests == null)
            {
                return NotFound();
            }

            var giftstoreSenderrequest = await _context.GiftstoreSenderrequests
                .Include(g => g.Maker)
                .Include(g => g.Sender)
                .FirstOrDefaultAsync(m => m.Requestid == id);
            if (giftstoreSenderrequest == null)
            {
                return NotFound();
            }

            return View(giftstoreSenderrequest);
        }

        // POST: Senderrequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftstoreSenderrequests == null)
            {
                return Problem("Entity set 'ModelContext.GiftstoreSenderrequests'  is null.");
            }
            var giftstoreSenderrequest = await _context.GiftstoreSenderrequests.FindAsync(id);
            if (giftstoreSenderrequest != null)
            {
                _context.GiftstoreSenderrequests.Remove(giftstoreSenderrequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftstoreSenderrequestExists(decimal id)
        {
          return (_context.GiftstoreSenderrequests?.Any(e => e.Requestid == id)).GetValueOrDefault();
        }
    }
}
