using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Site;

namespace Business.Controllers.Site
{
    public class FaviconsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FaviconsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Favicons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Favicons.ToListAsync());
        }

        // GET: Favicons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var favicon = await _context.Favicons.FirstOrDefaultAsync(f => f.Id == id);
            if (favicon == null) return NotFound();

            return View(favicon);
        }

        // GET: Favicons/Create
        public IActionResult Create() => View();

        // POST: Favicons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Favicon favicon)
        {
            if (ModelState.IsValid)
            {
                if (favicon.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/favicon");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid() + "_" + favicon.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await favicon.ImageFile.CopyToAsync(fileStream);
                    }

                    favicon.ImageUrl = $"/uploads/favicon/{uniqueFileName}";
                }

                _context.Add(favicon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(favicon);
        }

        // GET: Favicons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var favicon = await _context.Favicons.FindAsync(id);
            if (favicon == null) return NotFound();

            return View(favicon);
        }

        // POST: Favicons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Favicon favicon)
        {
            if (id != favicon.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingFavicon = await _context.Favicons.FindAsync(id);

                    if (favicon.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/favicon");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid() + "_" + favicon.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await favicon.ImageFile.CopyToAsync(fileStream);
                        }

                        existingFavicon.ImageUrl = $"/uploads/favicon/{uniqueFileName}";
                    }

                    existingFavicon.Title = favicon.Title;
                    existingFavicon.Name = favicon.Name;

                    _context.Update(existingFavicon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaviconExists(favicon.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(favicon);
        }

        // GET: Favicons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var favicon = await _context.Favicons.FirstOrDefaultAsync(f => f.Id == id);
            if (favicon == null) return NotFound();

            return View(favicon);
        }

        // POST: Favicons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favicon = await _context.Favicons.FindAsync(id);
            if (favicon != null)
            {
                _context.Favicons.Remove(favicon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FaviconExists(int id) => _context.Favicons.Any(f => f.Id == id);
    }
}
