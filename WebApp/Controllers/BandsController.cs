using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp;

namespace WebApp.Controllers
{
    public class BandsController : Controller
    {
        private readonly MyDbContext _context;

        public BandsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Bands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bands.ToListAsync());
        }

        // GET: Bands/Details/5
        public async Task<IActionResult> Details(int? id, int? ConcertId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bands = await _context.Bands
                .FirstOrDefaultAsync(m => m.BandId == id);
            if (bands == null)
            {
                return NotFound();
            }
            var StyleId  = _context.BandToStyle.Where(x => x.BandId == id).OrderBy(x=>x.StyleId).Select(x => x.StyleId).ToList();
            var StyleName = _context.Styles.Where(x => StyleId.Contains(x.StyleId)).OrderBy(x=>x.StyleId).Select(x => x.Type).ToList();
            var ArtistId = _context.ConcertManToBand.Where(x => x.BandId == id).OrderBy(x=>x.ConcertManId).Select(x => x.ConcertManId).ToList();
            var ArtistName = _context.ConcertMans.Where(x => ArtistId.Contains(x.ConcertManId)).OrderBy(x=>x.ConcertManId).Select(x => x.Name).ToList();
            var ArtistSurName = _context.ConcertMans.Where(x => ArtistId.Contains(x.ConcertManId)).OrderBy(x=>x.ConcertManId).Select(x => x.Surname).ToList();
            ViewBag.StyleId = StyleId;
            ViewBag.StyleName = StyleName;
            ViewBag.ArtistId = ArtistId;
            ViewBag.ArtistName = ArtistName;
            ViewBag.ArtistSurName = ArtistSurName;
            if (ConcertId != null) ViewBag.ConcertId = ConcertId;
            else ViewBag.ConcertId = -1;
            return View(bands);
        }

        // GET: Bands/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("BandId,Name")] Bands bands)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bands);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bands);
        }

        // GET: Bands/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bands = await _context.Bands.FindAsync(id);
            if (bands == null)
            {
                return NotFound();
            }
            return View(bands);
        }

        // POST: Bands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("BandId,Name")] Bands bands)
        {
            if (id != bands.BandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bands);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandsExists(bands.BandId))
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
            return View(bands);
        }

        // GET: Bands/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bands = await _context.Bands
                .FirstOrDefaultAsync(m => m.BandId == id);
            if (bands == null)
            {
                return NotFound();
            }

            return View(bands);
        }

        // POST: Bands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bands = await _context.Bands.FindAsync(id);
            _context.Bands.Remove(bands);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        private bool BandsExists(int id)
        {
            return _context.Bands.Any(e => e.BandId == id);
        }
    }
}
