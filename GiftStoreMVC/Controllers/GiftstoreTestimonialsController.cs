using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace GiftStoreMVC.Controllers;

public class GiftstoreTestimonialsController : Controller
{
    private readonly ModelContext _context;

    public GiftstoreTestimonialsController(ModelContext context) => _context = context;

    // GET: GiftstoreTestimonials
    public async Task<IActionResult> Index()
    {
        IIncludableQueryable<GiftstoreTestimonial, GiftstoreUser?> modelContext = _context.GiftstoreTestimonials.Include(g => g.User);
        return View(await modelContext.ToListAsync());
    }

    // GET: GiftstoreTestimonials/Details/5
    public async Task<IActionResult> Details(decimal? id)
    {
        if (id == null || _context.GiftstoreTestimonials == null)
        {
            return NotFound();
        }

        GiftstoreTestimonial? giftstoreTestimonial = await _context.GiftstoreTestimonials
            .Include(g => g.User)
            .FirstOrDefaultAsync(m => m.Testimonialid == id);
        if (giftstoreTestimonial == null)
        {
            return NotFound();
        }

        return View(giftstoreTestimonial);
    }

    // GET: GiftstoreTestimonials/Create
    public IActionResult Create()
    {
        ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid");
        return View();
    }


      
    public async Task<IActionResult> UserTestimonial()
    {
        IIncludableQueryable<GiftstoreTestimonial, GiftstoreUser> modelContext = _context.GiftstoreTestimonials.Include(t => t.User);
        return View(await modelContext.ToListAsync());
    }


    // POST: GiftstoreTestimonials/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Testimonialid,Testimonialcontent,Testimonialdate,Testimonialstatus,Userid")] GiftstoreTestimonial giftstoreTestimonial)
    {
        if (ModelState.IsValid)
        {
            _context.Add(giftstoreTestimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreTestimonial.Userid);
        return View(giftstoreTestimonial);
    }

    // GET: GiftstoreTestimonials/Edit/5
    public async Task<IActionResult> Edit(decimal? id)
    {
        if (id == null || _context.GiftstoreTestimonials == null)
        {
            return NotFound();
        }

        GiftstoreTestimonial? giftstoreTestimonial = await _context.GiftstoreTestimonials.FindAsync(id);
        if (giftstoreTestimonial == null)
        {
            return NotFound();
        }
        ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreTestimonial.Userid);
        return View(giftstoreTestimonial);
    }

    // POST: GiftstoreTestimonials/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Testimonialcontent,Testimonialdate,Testimonialstatus,Userid")] GiftstoreTestimonial giftstoreTestimonial)
    {
        if (id != giftstoreTestimonial.Testimonialid)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(giftstoreTestimonial);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiftstoreTestimonialExists(giftstoreTestimonial.Testimonialid))
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
        ViewData["Userid"] = new SelectList(_context.GiftstoreUsers, "Userid", "Userid", giftstoreTestimonial.Userid);
        return View(giftstoreTestimonial);
    }

    // GET: GiftstoreTestimonials/Delete/5
    public async Task<IActionResult> Delete(decimal? id)
    {
        if (id == null || _context.GiftstoreTestimonials == null)
        {
            return NotFound();
        }

        GiftstoreTestimonial? giftstoreTestimonial = await _context.GiftstoreTestimonials
            .Include(g => g.User)
            .FirstOrDefaultAsync(m => m.Testimonialid == id);
        if (giftstoreTestimonial == null)
        {
            return NotFound();
        }

        return View(giftstoreTestimonial);
    }

    // POST: GiftstoreTestimonials/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(decimal id)
    {
        if (_context.GiftstoreTestimonials == null)
        {
            return Problem("Entity set 'ModelContext.GiftstoreTestimonials'  is null.");
        }
        GiftstoreTestimonial? giftstoreTestimonial = await _context.GiftstoreTestimonials.FindAsync(id);
        if (giftstoreTestimonial != null)
        {
            _context.GiftstoreTestimonials.Remove(giftstoreTestimonial);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GiftstoreTestimonialExists(decimal id) => (_context.GiftstoreTestimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
}