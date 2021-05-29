using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebApp;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class BandToStylesController : Controller
    {
        private readonly MyDbContext _context;

        public BandToStylesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: BandToStyles
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.BandToStyle.Include(b => b.Band).Include(b => b.Style);
            return View(await myDbContext.ToListAsync());
        }

        // GET: BandToStyles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bandToStyle = await _context.BandToStyle
                .Include(b => b.Band)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(m => m.BandToStyleId == id);
            if (bandToStyle == null)
            {
                return NotFound();
            }

            return View(bandToStyle);
        }

        // GET: BandToStyles/Create
        public IActionResult Create()
        {
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name");
            ViewData["StyleId"] = new SelectList(_context.Styles, "StyleId", "Type");
            return View();
        }

        // POST: BandToStyles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BandToStyleId,StyleId,BandId")] BandToStyle bandToStyle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bandToStyle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", bandToStyle.BandId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "StyleId", "Type", bandToStyle.StyleId);
            return View(bandToStyle);
        }

        // GET: BandToStyles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bandToStyle = await _context.BandToStyle.FindAsync(id);
            if (bandToStyle == null)
            {
                return NotFound();
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", bandToStyle.BandId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "StyleId", "Type", bandToStyle.StyleId);
            return View(bandToStyle);
        }

        // POST: BandToStyles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BandToStyleId,StyleId,BandId")] BandToStyle bandToStyle)
        {
            if (id != bandToStyle.BandToStyleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bandToStyle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandToStyleExists(bandToStyle.BandToStyleId))
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
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "Name", bandToStyle.BandId);
            ViewData["StyleId"] = new SelectList(_context.Styles, "StyleId", "Type", bandToStyle.StyleId);
            return View(bandToStyle);
        }

        // GET: BandToStyles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bandToStyle = await _context.BandToStyle
                .Include(b => b.Band)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(m => m.BandToStyleId == id);
            if (bandToStyle == null)
            {
                return NotFound();
            }

            return View(bandToStyle);
        }

        // POST: BandToStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bandToStyle = await _context.BandToStyle.FindAsync(id);
            _context.BandToStyle.Remove(bandToStyle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BandToStyleExists(int id)
        {
            return _context.BandToStyle.Any(e => e.BandToStyleId == id);
        }
    }
}
