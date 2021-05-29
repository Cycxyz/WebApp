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
    public class ConcertManToBandsController : Controller
    {
        private readonly MyDbContext _context;

        public ConcertManToBandsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: ConcertManToBands
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.ConcertManToBand.Include(c => c.Band).Include(c => c.ConcertMan);
            return View(await myDbContext.ToListAsync());
        }

        // GET: ConcertManToBands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertManToBand = await _context.ConcertManToBand
                .Include(c => c.Band)
                .Include(c => c.ConcertMan)
                .FirstOrDefaultAsync(m => m.ConcertManToBandId == id);
            if (concertManToBand == null)
            {
                return NotFound();
            }

            return View(concertManToBand);
        }

        // GET: ConcertManToBands/Create
        public IActionResult Create()
        {
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name" );
            ViewData["ConcertManId"] = new SelectList(_context.ConcertMans, "ConcertManId", "Name");
            return View();
        }

        // POST: ConcertManToBands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertManToBandId,ConcertManId,BandId")] ConcertManToBand concertManToBand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concertManToBand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", concertManToBand.BandId);
            ViewData["ConcertManId"] = new SelectList(_context.ConcertMans, "ConcertManId", "Name", concertManToBand.ConcertManId);
            return View(concertManToBand);
        }

        // GET: ConcertManToBands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertManToBand = await _context.ConcertManToBand.FindAsync(id);
            if (concertManToBand == null)
            {
                return NotFound();
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", concertManToBand.BandId);
            ViewData["ConcertManId"] = new SelectList(_context.ConcertMans, "ConcertManId", "Name", concertManToBand.ConcertManId);
            return View(concertManToBand);
        }

        // POST: ConcertManToBands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertManToBandId,ConcertManId,BandId")] ConcertManToBand concertManToBand)
        {
            if (id != concertManToBand.ConcertManToBandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concertManToBand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertManToBandExists(concertManToBand.ConcertManToBandId))
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
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", concertManToBand.BandId);
            ViewData["ConcertManId"] = new SelectList(_context.ConcertMans, "ConcertManId", "Name", concertManToBand.ConcertManId);
            return View(concertManToBand);
        }

        // GET: ConcertManToBands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertManToBand = await _context.ConcertManToBand
                .Include(c => c.Band)
                .Include(c => c.ConcertMan)
                .FirstOrDefaultAsync(m => m.ConcertManToBandId == id);
            if (concertManToBand == null)
            {
                return NotFound();
            }

            return View(concertManToBand);
        }

        // POST: ConcertManToBands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concertManToBand = await _context.ConcertManToBand.FindAsync(id);
            _context.ConcertManToBand.Remove(concertManToBand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertManToBandExists(int id)
        {
            return _context.ConcertManToBand.Any(e => e.ConcertManToBandId == id);
        }
    }
}
