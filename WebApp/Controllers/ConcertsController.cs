using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly MyDbContext _context;
        bool b = true;
        public ConcertsController(MyDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "user")]
        [HttpGet]
        public IActionResult Buy(int concertId) 
            {
            ViewBag.Tickets=new SelectList(_context.Tickets.Where(x=>x.CustomerId==null).Where(x=>x.ConcertId==concertId), "Place", "Place");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Buy(int TicketId, int ?ConcertId)
        {
            return View("Index");
        }
        [NonAction]
        void AddTickets()
        {
            foreach (var concert in _context.Concerts)
            {
                if (!_context.Tickets.Select(x => x.ConcertId).Contains(concert.ConcertId))
                {
                    var conc = _context.Concerts.Find(concert.ConcertId);
                    for (int i = 0; i < 200; i++)
                    {
                        Tickets ticket = new Tickets { Concert = conc, ConcertId = conc.ConcertId, Customer = null, CustomerId = null, Place = i + 1, Price = i < 150 ? 100 : 200 };
                        _context.Add(ticket);
                    }
                }
            }
            _context.SaveChanges();

        }
        // GET: Concerts
        public async Task<IActionResult> Index(int? BandId, int? CityId, DateTime? date)
        {
            if (b)
            {
                AddTickets();
                b = false;
            }

            var myDbContext = _context.Concerts.Where(x => true);
            if (BandId != null && BandId != 0)
            {
                var CBands = _context.ConcertToBand.Where(x => x.BandId == BandId).Select(x => x.ConcertId).ToList();
                myDbContext = myDbContext.Where(x => CBands.Contains(x.ConcertId));
            }
            if (CityId != null && CityId != 0)
            {
                var CCities = _context.Concerts
                    .Join(_context.ConcertAdresses, x => x.ConcertAdressId, y => y.ConcertAdressId, (x, y) => new { Id = x.ConcertId, CityId = y.CityId })
                    .Where(x => x.CityId == CityId).Select(x => x.Id).ToList();
                myDbContext = myDbContext.Where(x => CCities.Contains(x.ConcertId));
            }
            if (date != null)
            {
                myDbContext = myDbContext.Where(x => x.Date.Date == date.GetValueOrDefault().Date);
            }

            ViewBag.Bands = new SelectList(_context.Bands, "BandId", "Name");
            ViewBag.Cities = new SelectList(_context.Cities, "CityId", "Name");
            var lst = new List<int>();
            foreach(var it in myDbContext)
            {
                lst.Add(it.ConcertId);
            }
            ViewBag.Model = lst;
            return View(await myDbContext.Include(x => x.ConcertAdress).ToListAsync());

        }

        // GET: Concerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var concerts = await _context.Concerts
                .Include(c => c.ConcertAdress)
                .FirstOrDefaultAsync(m => m.ConcertId == id);
            if (concerts == null)
            {
                return NotFound();
            }
            ViewBag.BandsDetails = new SelectList(_context.Bands, "BandId", "Name");
            var BandsId = _context.ConcertToBand.Where(x => x.ConcertId == id).OrderBy(x => x.BandId).Select(x => x.BandId).ToList();
            var BandsName = _context.Bands.Where(x => BandsId.Contains(x.BandId)).OrderBy(x => x.BandId).Select(x => x.Name).ToList();
            ViewBag.ConcertId = id;
            ViewBag.BandsId = BandsId;
            ViewBag.BandsName = BandsName;
            ViewBag.Cities = _context.Concerts.Where(x => x.ConcertId == id)
                .Join(_context.ConcertAdresses, x => x.ConcertAdressId, y => y.ConcertAdressId, (x, y) => y.CityId)
                .Join(_context.Cities, x => x, y => y.CityId, (x, y) => y.Name).FirstOrDefault();
            ViewBag.TicketsCount = _context.Tickets.Where(x => x.Customer == null && x.ConcertId == id).Count();
            return View(concerts);
        }

        // GET: Concerts/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["ConcertAdressId"] = new SelectList(_context.ConcertAdresses, "ConcertAdressId", "Adress");
            return View();
        }

        // POST: Concerts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("ConcertId,ConcertAdressId,Date")] Concerts concerts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concerts);
                // await AddTickets();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcertAdressId"] = new SelectList(_context.ConcertAdresses, "ConcertAdressId", "Adress", concerts.ConcertAdressId);
            b = true;
            return View(concerts);
        }

        // GET: Concerts/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concerts = await _context.Concerts.FindAsync(id);
            if (concerts == null)
            {
                return NotFound();
            }
            ViewData["ConcertAdressId"] = new SelectList(_context.ConcertAdresses, "ConcertAdressId", "Adress", concerts.ConcertAdressId);
            return View(concerts);
        }

        // POST: Concerts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertId,ConcertAdressId,Date")] Concerts concerts)
        {
            if (id != concerts.ConcertId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concerts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertsExists(concerts.ConcertId))
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
            ViewData["ConcertAdressId"] = new SelectList(_context.ConcertAdresses, "ConcertAdressId", "Adress", concerts.ConcertAdressId);
            return View(concerts);
        }

        // GET: Concerts/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concerts = await _context.Concerts
                .Include(c => c.ConcertAdress)
                .FirstOrDefaultAsync(m => m.ConcertId == id);
            if (concerts == null)
            {
                return NotFound();
            }

            return View(concerts);
        }

        // POST: Concerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concerts = await _context.Concerts.FindAsync(id);
            _context.Concerts.Remove(concerts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ConcertsExists(int id)
        {
            return _context.Concerts.Any(x => x.ConcertId == id);
        }
        [NonAction]

        private void ParseLine(IXLWorksheet worksheet, Cities city)
        {
            foreach(IXLRow row in worksheet.RowsUsed().Skip(1))
            {
                DateTime date = Convert.ToDateTime(row.Cell(2).Value);
                var concert = (from conc in _context.Concerts.Include(x => x.ConcertAdress)
                               where ( date== conc.Date
                               && row.Cell(1).Value.ToString() == conc.ConcertAdress.Adress)
                               select conc).ToList();

                    Concerts newconc = new Concerts();
                if(concert.Count()==0)
                {
                    var addr1 = row.Cell(1).Value.ToString();
                    var adress = (from addr in _context.ConcertAdresses.Include(x => x.City)
                                   where addr.CityId == city.CityId && addr1==addr.Adress
                                   select addr).ToList();
                        ConcertAdresses newadress = new ConcertAdresses();
                    if(adress.Count()==0)
                    {
                        newadress.CityId = city.CityId;
                        newadress.Adress = row.Cell(1).Value.ToString();
                        _context.ConcertAdresses.Add(newadress);
                    }
                    else
                    {
                        newadress = adress[0];
                    }
                    newconc.ConcertAdress = newadress;
                    newconc.Date = date;
                    _context.Concerts.Add(newconc);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (!ModelState.IsValid || fileExcel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
            {
                await fileExcel.CopyToAsync(stream);
                using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                {
                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
                    {
                        Cities newCity;
                        var c = (from city in _context.Cities
                                 where (worksheet.Name ==city.Name )
                                 select city).ToList();
                        if (c.Count > 0)
                        {
                            newCity = c[0];
                        }
                        else
                        {
                            newCity = new Cities();
                            newCity.Name = worksheet.Name;
                            //додати в контекст
                            _context.Cities.Add(newCity);
                            await _context.SaveChangesAsync();
                        }
                        //перегляд усіх рядків                    
                        
                            try
                            {
                            ParseLine(worksheet, _context.Cities.Where(x => x.Name == newCity.Name).FirstOrDefault());
                            }
                            catch (Exception e)
                            {
                            string Path = Directory.GetCurrentDirectory();
                            FileStream file = new FileStream($"{Path}\\Log.txt", FileMode.Append);
                            byte[] array = System.Text.Encoding.Default.GetBytes($"{e.Message}\n");
                            file.Write(array, 0, array.Length);
                            file.Close();
                            return View("Error");
                            }
                        

                    }


                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

        }
        [Authorize(Roles = "admin")]
        public ActionResult Export(List<int> concerts)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var lines = (from line in concerts
                             
                             .Join(_context.Concerts, x=>x, y=>y.ConcertId, (x,y)=>new {y.Date, y.ConcertAdressId })
                             .Join(_context.ConcertAdresses, x => x.ConcertAdressId, y => y.ConcertAdressId, (x, y) => new { x.Date, y.Adress, y.Details, y.CityId })
                             .Join(_context.Cities, x => x.CityId, y => y.CityId, (x, y) => new { x.Date, x.Adress, x.Details, y.Name })   
                             
                             group new
                             {
                                 line.Adress,
                                 line.Date,
                                 line.Details,
                                 line.Name
                             }
                             by line.Name
                             );
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var l in lines)
                {
                    var worksheet =workbook.Worksheets.Add(l.Key);
                    worksheet.Cell("A1").Value = "Адрес концерта";
                    worksheet.Cell("B1").Value = "Дата концерта";
                    worksheet.Cell("C1").Value = "Детали адреса";
                    worksheet.Column(1).Width = 27;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;
                    worksheet.Row(1).Style.Font.Bold = true;
                    int it = 2;
                    foreach (var c in l)
                    {
                        worksheet.Cell(it, 1).Value = c.Adress;
                        worksheet.Cell(it, 2).Value = c.Date;
                        worksheet.Cell(it, 3).Value = c.Details;
                        it++;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
