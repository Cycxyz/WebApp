using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp;
using Microsoft.AspNetCore.Authorization;
namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class ConcertToBandsController : Controller
    {
        private readonly MyDbContext _context;

        public ConcertToBandsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: ConcertToBands
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.ConcertToBand.Include(c => c.Band).Include(c => c.Concert);
            return View(await myDbContext.ToListAsync());
        }

        // GET: ConcertToBands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertToBand = await _context.ConcertToBand
                .Include(c => c.Band)
                .Include(c => c.Concert)
                .FirstOrDefaultAsync(m => m.ConcertToBandId == id);
            if (concertToBand == null)
            {
                return NotFound();
            }

            return View(concertToBand);
        }

        // GET: ConcertToBands/Create
        public IActionResult Create()
        {
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name");
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "ConcertId", "ConcertId");
            return View();
        }

        // POST: ConcertToBands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertToBandId,ConcertId,BandId")] ConcertToBand concertToBand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concertToBand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", concertToBand.BandId);
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "ConcertId", "ConcertId", concertToBand.ConcertId);
            return View(concertToBand);
        }

        // GET: ConcertToBands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertToBand = await _context.ConcertToBand.FindAsync(id);
            if (concertToBand == null)
            {
                return NotFound();
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", concertToBand.BandId);
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "ConcertId", "ConcertId", concertToBand.ConcertId);
            return View(concertToBand);
        }

        // POST: ConcertToBands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertToBandId,ConcertId,BandId")] ConcertToBand concertToBand)
        {
            if (id != concertToBand.ConcertToBandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concertToBand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertToBandExists(concertToBand.ConcertToBandId))
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
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", concertToBand.BandId);
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "ConcertId", "ConcertId", concertToBand.ConcertId);
            return View(concertToBand);
        }

        // GET: ConcertToBands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertToBand = await _context.ConcertToBand
                .Include(c => c.Band)
                .Include(c => c.Concert)
                .FirstOrDefaultAsync(m => m.ConcertToBandId == id);
            if (concertToBand == null)
            {
                return NotFound();
            }

            return View(concertToBand);
        }

        // POST: ConcertToBands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concertToBand = await _context.ConcertToBand.FindAsync(id);
            _context.ConcertToBand.Remove(concertToBand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertToBandExists(int id)
        {
            return _context.ConcertToBand.Any(e => e.ConcertToBandId == id);
        }
    }
}
