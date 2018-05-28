using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mapa.Models;
using Mapa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mapa.Extensions;

namespace Mapa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {

			IQueryable<POI> places = _context.POIs.Where(p => p.Activated == true);
            ViewData["Places"] = Newtonsoft.Json.JsonConvert.SerializeObject(places.ToList());
            ViewData["Categories"] = new SelectList(await _context.POIs.Select(x => x.Category).Distinct().ToListAsync());
            ViewData["Tags"] = Newtonsoft.Json.JsonConvert.SerializeObject(StringExtensions.GetStrings(places, x => !string.IsNullOrEmpty(x.Tags), x => x.Tags.Split(',')));
			ViewData["Segments"] = Newtonsoft.Json.JsonConvert.SerializeObject(_context.Tags.Where(x => x.Key.Equals("Segmento")).Select(x => new { value = x.Id, text = x.Value }));
			ViewData["Tecnologies"] = Newtonsoft.Json.JsonConvert.SerializeObject(_context.Tags.Where(x => x.Key.Equals("Tecnologia")).Select(x => new { value = x.Id, text = x.Value }));
			LoadData();
            return View();
        }

        [ResponseCache(Duration = 60)]
        public IActionResult Shuffle(){
            var places = _context.POIs.Where(p => p.Activated == true).ToList();
            ViewData["Places"] = Newtonsoft.Json.JsonConvert.SerializeObject(places);
            ViewData["Categories"] = Newtonsoft.Json.JsonConvert.SerializeObject(places.Select(x => x.Category).Distinct());
            LoadData();
            return View();
        }

        private void LoadData(){
            var places = _context.POIs.Where(p => p.Activated == true);
            ViewData["PerCategory"] = Newtonsoft.Json.JsonConvert.SerializeObject(
                places
                .GroupBy(x => x.Category)
                .Select(k => new { Category = k.Key, Value = k.Count() }));
            ViewData["TotalTags"] = StringExtensions.GetStrings(places, x => !string.IsNullOrEmpty(x.Tags), x => x.Tags.Split(',')).Count();
            ViewData["TotalCategories"] = places.Select(x => x.Category).Distinct().Count();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
