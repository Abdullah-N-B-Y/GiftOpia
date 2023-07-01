using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;

namespace GiftStoreMVC.Controllers;

public class GiftstoreBankcardsController : Controller
{
    private readonly ModelContext _context;

    public GiftstoreBankcardsController(ModelContext context) => _context = context;

    // GET: GiftstoreBankcards
    public async Task<IActionResult> Index() =>
        _context.GiftstoreBankcards != null ? 
            View(await _context.GiftstoreBankcards.ToListAsync()) :
            Problem("Entity set 'ModelContext.GiftstoreBankcards'  is null.");

    // GET: GiftstoreBankcards/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null || _context.GiftstoreBankcards == null)
        {
            return NotFound();
        }

        GiftstoreBankcard? giftstoreBankcard = await _context.GiftstoreBankcards
            .FirstOrDefaultAsync(m => m.Cardnumber == id);
        if (giftstoreBankcard == null)
        {
            return NotFound();
        }

        return View(giftstoreBankcard);
    }

    // GET: GiftstoreBankcards/Create
    public IActionResult Create() => View();

    // POST: GiftstoreBankcards/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Cardnumber,Cardholdername,Expirationdate,Cvv,Totalamount")] GiftstoreBankcard giftstoreBankcard)
    {
        if (ModelState.IsValid)
        {
            _context.Add(giftstoreBankcard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(giftstoreBankcard);
    }

    // GET: GiftstoreBankcards/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.GiftstoreBankcards == null)
        {
            return NotFound();
        }

        GiftstoreBankcard? giftstoreBankcard = await _context.GiftstoreBankcards.FindAsync(id);
        if (giftstoreBankcard == null)
        {
            return NotFound();
        }
        return View(giftstoreBankcard);
    }

    // POST: GiftstoreBankcards/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Cardnumber,Cardholdername,Expirationdate,Cvv,Totalamount")] GiftstoreBankcard giftstoreBankcard)
    {
        if (id != giftstoreBankcard.Cardnumber)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(giftstoreBankcard);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiftstoreBankcardExists(giftstoreBankcard.Cardnumber))
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
        return View(giftstoreBankcard);
    }

    // GET: GiftstoreBankcards/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.GiftstoreBankcards == null)
        {
            return NotFound();
        }

        GiftstoreBankcard? giftstoreBankcard = await _context.GiftstoreBankcards
            .FirstOrDefaultAsync(m => m.Cardnumber == id);
        if (giftstoreBankcard == null)
        {
            return NotFound();
        }

        return View(giftstoreBankcard);
    }

    // POST: GiftstoreBankcards/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.GiftstoreBankcards == null)
        {
            return Problem("Entity set 'ModelContext.GiftstoreBankcards'  is null.");
        }
        GiftstoreBankcard? giftstoreBankcard = await _context.GiftstoreBankcards.FindAsync(id);
        if (giftstoreBankcard != null)
        {
            _context.GiftstoreBankcards.Remove(giftstoreBankcard);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GiftstoreBankcardExists(string id) => (_context.GiftstoreBankcards?.Any(e => e.Cardnumber == id)).GetValueOrDefault();
}