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
    public class OrdersController : Controller
    {
        private readonly ModelContext _context;

        public OrdersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
              return _context.GiftstoreOrders != null ? 
                          View(await _context.GiftstoreOrders.ToListAsync()) :
                          Problem("Entity set 'ModelContext.GiftstoreOrders'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.GiftstoreOrders == null)
            {
                return NotFound();
            }

            var giftstoreOrder = await _context.GiftstoreOrders
                .FirstOrDefaultAsync(m => m.Orderid == id);
            if (giftstoreOrder == null)
            {
                return NotFound();
            }

            return View(giftstoreOrder);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orderid,Recipientaddress,Arrivaldate,Finalprice")] GiftstoreOrder giftstoreOrder)
        {
            // Orderdate 
          

            if (ModelState.IsValid)
            {
                giftstoreOrder.Orderstatus = "Arrived";
                giftstoreOrder.Orderdate = DateTime.UtcNow;
                _context.Add(giftstoreOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giftstoreOrder);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.GiftstoreOrders == null)
            {
                return NotFound();
            }

            var giftstoreOrder = await _context.GiftstoreOrders.FindAsync(id);
            if (giftstoreOrder == null)
            {
                return NotFound();
            }
            return View(giftstoreOrder);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Orderid,Orderdate,Orderstatus,Recipientaddress,Arrivaldate,Finalprice")] GiftstoreOrder giftstoreOrder)
        {
            if (id != giftstoreOrder.Orderid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giftstoreOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftstoreOrderExists(giftstoreOrder.Orderid))
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
            return View(giftstoreOrder);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.GiftstoreOrders == null)
            {
                return NotFound();
            }

            var giftstoreOrder = await _context.GiftstoreOrders
                .FirstOrDefaultAsync(m => m.Orderid == id);
            if (giftstoreOrder == null)
            {
                return NotFound();
            }

            return View(giftstoreOrder);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.GiftstoreOrders == null)
            {
                return Problem("Entity set 'ModelContext.GiftstoreOrders'  is null.");
            }
            var giftstoreOrder = await _context.GiftstoreOrders.FindAsync(id);
            if (giftstoreOrder != null)
            {
                _context.GiftstoreOrders.Remove(giftstoreOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftstoreOrderExists(decimal id)
        {
          return (_context.GiftstoreOrders?.Any(e => e.Orderid == id)).GetValueOrDefault();
        }
    }
}
