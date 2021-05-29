using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class DiagramsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DiagramsController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var cnsrtaddr = _context.ConcertAdresses.Include(x => x.Concerts).Include(x => x.City).ToList();
            var citiesCount = new Dictionary<Cities, int>();
            List<object> concerts = new List<object>();
            foreach(var addr in cnsrtaddr)
            {
                if (!citiesCount.ContainsKey(addr.City)) 
                {
                    citiesCount[addr.City] = 0;
                }
                citiesCount[addr.City] += addr.Concerts.Count();
            }
            concerts.Add(new []{ "Город", "Количество концертов" });
            foreach(var p in citiesCount)
            {
                concerts.Add(new object[]{ p.Key.Name, p.Value });
            }
            return new JsonResult(concerts);
        }

    }
}
