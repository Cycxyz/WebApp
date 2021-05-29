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
    public class ConcertAdressesController : Controller
    {
        private readonly MyDbContext _context;

        public ConcertAdressesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: ConcertAdresses
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.ConcertAdresses.Include(c => c.City);
            return View(await myDbContext.ToListAsync());
        }

        // GET: ConcertAdresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertAdresses = await _context.ConcertAdresses
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.ConcertAdressId == id);
            if (concertAdresses == null)
            {
                return NotFound();
            }

            return View(concertAdresses);
        }

        // GET: ConcertAdresses/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name");
            return View();
        }

        // POST: ConcertAdresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertAdressId,Adress,CityId,Details")] ConcertAdresses concertAdresses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concertAdresses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", concertAdresses.CityId);
            return View(concertAdresses);
        }

        // GET: ConcertAdresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertAdresses = await _context.ConcertAdresses.FindAsync(id);
            if (concertAdresses == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", concertAdresses.CityId);
            return View(concertAdresses);
        }

        // POST: ConcertAdresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertAdressId,Adress,CityId,Details")] ConcertAdresses concertAdresses)
        {
            if (id != concertAdresses.ConcertAdressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concertAdresses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertAdressesExists(concertAdresses.ConcertAdressId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name", concertAdresses.CityId);
            return View(concertAdresses);
        }

        // GET: ConcertAdresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertAdresses = await _context.ConcertAdresses
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.ConcertAdressId == id);
            if (concertAdresses == null)
            {
                return NotFound();
            }

            return View(concertAdresses);
        }

        // POST: ConcertAdresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concertAdresses = await _context.ConcertAdresses.FindAsync(id);
            _context.ConcertAdresses.Remove(concertAdresses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertAdressesExists(int id)
        {
            return _context.ConcertAdresses.Any(e => e.ConcertAdressId == id);
        }
    }
}
