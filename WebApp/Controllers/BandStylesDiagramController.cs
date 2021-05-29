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
    public class BandStylesDiagramController : ControllerBase
    {
        private readonly MyDbContext _context;
        public BandStylesDiagramController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var bandsStyles = _context.BandToStyle.Include(x => x.Style).Include(x => x.Band).ToList();
            var count = new Dictionary<Styles, int>();
            foreach (var pair in bandsStyles)
            {
                if (!count.ContainsKey(pair.Style)) count[pair.Style] = 0;
                count[pair.Style]++;
            }
            var res = new List<object>();
            res.Add(new object[] { "Стиль", "Количество групп" });
            foreach (var pair in count)
                res.Add(new object[] { pair.Key.Type, pair.Value });
            return new JsonResult(res);
        }

    }
}
