using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mapa.Data;
using Mapa.Models;
using Mapa.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;

namespace Mapa.Controllers
{
	[Authorize(Roles = ApplicationRoles.Administrator)]
    public class POIsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public POIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> SheetImport(CancellationToken cancellationToken)
        {
            string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetEnvironmentVariable("LocalAppData");
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ApplicationName",
            });

            // Define request parameters.
            String spreadsheetId = "1fgP-ZxD9DCiyjL9-4__YR9ZzIRSLGaOP9sZBJRDLQxs";
            // Sheet ! A1 : I
            String range = "places!A1:I";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                int count = 0;
                foreach (var row in values.Skip(1))
                {
                    _context.POIs.Add(new POI(row));
                    count++;
                    if (count % 50 == 0)
                        await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();

            return Ok(values);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Add(POIViewModel poiVM)
        {
            try
			{
				var poi = POI.GetPOI(poiVM);
				poi.Activated = false;
				poi.CreationDate = DateTime.Now;
				AddDefinedTag(poiVM.Segments, poi);
				AddDefinedTag(poiVM.Tecnologies, poi);
				await CreateImageAsync(poi);

				_context.Add(poi);
				await _context.SaveChangesAsync();
				return Json(new { status = "OK" });
			}
			catch (Exception e){
                return Json(new { status = "BAD", message = e.Message });
            }
        }

		private void AddDefinedTag(List<int> list, POI poi)
		{
			if (list != null)
			{
				foreach (var tag in list)
				{
					if (tag > 0)
					{
						poi.DefinedTags.Add(new POITag()
						{
							TagId = tag,
							POI = poi
						});
					}
				}
			}
		}

		// GET: POIs
		public async Task<IActionResult> Index(POIsQuery query)
        {
            Func<POI, object> orderFn = null;
            switch (query.Order)
            {
            	default:
            		orderFn = (x => x.CreationDate);
            		break;
            }

            IQueryable<POI> poiQuery = _context.POIs
				.Include(x => x.DefinedTags)
				.ThenInclude(x => x.Tag)
				.AsQueryable();

            // if (!string.IsNullOrEmpty(query.Tag))
            // {
            // 	POIQuery = POIQuery
            // 		.Include(p => p.Tags)
            // 		.Where(v => v.Tags.Any(t => t.Name == query.Tag));
            // }

            if(query.HasKeywords())
                poiQuery = poiQuery.Where(w => w.Name.ToLowerInvariant().Contains(query.Keywords.ToLowerInvariant()));

            var result = poiQuery
                .OrderByDescending(orderFn)
                .Skip(query.Size * (query.Page - 1))
                .Take(query.Size);

            query.Total = await poiQuery.CountAsync();
            ViewData["Query"] = query;

            return View(result.ToList());
        }

        public async Task<IActionResult> Deactivate(int? id){
            return await changeActivation(id, false);
        }

        public async Task<IActionResult> Activate(int? id){
            return await changeActivation(id, true);
        }

        private async Task<IActionResult> changeActivation(int? id, bool v)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poi = await _context.POIs
                .SingleOrDefaultAsync(m => m.Id == id);
            poi.Activated = v;
            if(v) poi.ApprovalDate = DateTime.Now;
            _context.Update(poi);
            await _context.SaveChangesAsync();
            if (poi == null)
            {
                return NotFound();
            }
            return  RedirectToAction("Index");
        }


        // GET: POIs/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var asset = await _context.POIs
        //         .SingleOrDefaultAsync(m => m.Id == id);
        //     if (asset == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(asset);
        // }

        // GET: POIs/Create
        public async Task<IActionResult> Create()
        {
            //await LoadViewData();
            return View();
        }

        // POST: POIs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(POI poi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poi);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(poi);
        }

        // GET: POIs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.POIs
				.Include(x => x.DefinedTags)
				.ThenInclude(x => x.Tag)
				.FirstOrDefaultAsync(w => w.Id == id);
            if (asset == null)
            {
                return NotFound();
            }
            var places = _context.POIs.Where(p => p.Activated == true);
            ViewData["Categories"] = new SelectList(await _context.POIs.Select(x => x.Category).Distinct().ToListAsync());
            ViewData["Tags"] = Newtonsoft.Json.JsonConvert.SerializeObject(StringExtensions.GetStrings(places, x => !string.IsNullOrEmpty(x.Tags), x => x.Tags.Split(',')));
            return View(asset);
        }

        // POST: POIs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, POI poi)
        {
            if (id != poi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await CreateImageAsync(poi);
                    _context.Update(poi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!POIExists(poi.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            var places = _context.POIs.Where(p => p.Activated == true);
            ViewData["Categories"] = new SelectList(await _context.POIs.Select(x => x.Category).Distinct().ToListAsync());
            ViewData["Tags"] = Newtonsoft.Json.JsonConvert.SerializeObject(StringExtensions.GetStrings(places, x => !string.IsNullOrEmpty(x.Tags), x => x.Tags.Split(',')));
            return View(poi);
        }

        // GET: POIs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var poi = await _context.POIs
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.POIs.Remove(poi);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool POIExists(int id)
        {
            return _context.POIs.Any(e => e.Id == id);
        }

        private async Task CreateImageAsync(POI poi)
		{
			var file = Request.Form.Files[0];
			if (file?.Length > 0)
			{
				var fileTempPath = Path.GetTempFileName();
				var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString("N") + extension;
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatar", fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
                
				poi.Logo = Path.Combine("avatar", fileName);
			}
		}
    }
}
