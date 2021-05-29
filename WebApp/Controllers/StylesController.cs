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
    public class StylesController : Controller
    {
        private readonly MyDbContext _context;

        public StylesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Styles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Styles.ToListAsync());
        }

        // GET: Styles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styles = await _context.Styles
                .FirstOrDefaultAsync(m => m.StyleId == id);
            if (styles == null)
            {
                return NotFound();
            }

            return View(styles);
        }

        // GET: Styles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Styles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StyleId,Type")] Styles styles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(styles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(styles);
        }

        // GET: Styles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styles = await _context.Styles.FindAsync(id);
            if (styles == null)
            {
                return NotFound();
            }
            return View(styles);
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StyleId,Type")] Styles styles)
        {
            if (id != styles.StyleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(styles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StylesExists(styles.StyleId))
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
            return View(styles);
        }

        // GET: Styles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styles = await _context.Styles
                .FirstOrDefaultAsync(m => m.StyleId == id);
            if (styles == null)
            {
                return NotFound();
            }

            return View(styles);
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var styles = await _context.Styles.FindAsync(id);
            _context.Styles.Remove(styles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StylesExists(int id)
        {
            return _context.Styles.Any(e => e.StyleId == id);
        }
    }
}
