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
    public class ConcertMansController : Controller
    {
        private readonly MyDbContext _context;

        public ConcertMansController(MyDbContext context)
        {
            _context = context;
        }

        // GET: ConcertMans
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcertMans.ToListAsync());
        }

        // GET: ConcertMans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertMans = await _context.ConcertMans
                .FirstOrDefaultAsync(m => m.ConcertManId == id);
            if (concertMans == null)
            {
                return NotFound();
            }

            return View(concertMans);
        }

        // GET: ConcertMans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConcertMans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertManId,Name,Surname")] ConcertMans concertMans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concertMans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concertMans);
        }

        // GET: ConcertMans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertMans = await _context.ConcertMans.FindAsync(id);
            if (concertMans == null)
            {
                return NotFound();
            }
            return View(concertMans);
        }

        // POST: ConcertMans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertManId,Name,Surname")] ConcertMans concertMans)
        {
            if (id != concertMans.ConcertManId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concertMans);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertMansExists(concertMans.ConcertManId))
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
            return View(concertMans);
        }

        // GET: ConcertMans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertMans = await _context.ConcertMans
                .FirstOrDefaultAsync(m => m.ConcertManId == id);
            if (concertMans == null)
            {
                return NotFound();
            }

            return View(concertMans);
        }

        // POST: ConcertMans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concertMans = await _context.ConcertMans.FindAsync(id);
            _context.ConcertMans.Remove(concertMans);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertMansExists(int id)
        {
            return _context.ConcertMans.Any(e => e.ConcertManId == id);
        }
    }
}
