using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.About;
using Microsoft.AspNetCore.Http;

namespace Business.Controllers.About
{
    public class AboutsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AboutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Abouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.About.ToListAsync());
        }

        // GET: Abouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var about = await _context.About.FirstOrDefaultAsync(m => m.Id == id);
            if (about == null) return NotFound();

            return View(about);
        }

        // GET: Abouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Abouts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subtitle,Title,Description,Feature1,Feature2,Feature3,Feature4,ContactText,PhoneNumber,QuoteButtonText,QuoteButtonUrl,ImageUrl")] AboutSection about, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/about");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await ImageFile.CopyToAsync(fileStream);

                    about.ImageUrl = "/uploads/about/" + uniqueFileName;
                }
                else
                {
                    ModelState.AddModelError("ImageFile", "Please upload an image.");
                    return View(about);
                }

                _context.Add(about);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Abouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var about = await _context.About.FindAsync(id);
            if (about == null) return NotFound();

            return View(about);
        }

        // POST: Abouts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subtitle,Title,Description,Feature1,Feature2,Feature3,Feature4,ContactText,PhoneNumber,QuoteButtonText,QuoteButtonUrl,ImageUrl")] AboutSection about, IFormFile ImageFile)
        {
            if (id != about.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.About.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    if (existing == null) return NotFound();

                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // Delete old image file
                        if (!string.IsNullOrEmpty(existing.ImageUrl))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        // Save new image
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/about");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await ImageFile.CopyToAsync(fileStream);

                        about.ImageUrl = "/uploads/about/" + uniqueFileName;
                    }
                    else
                    {
                        // Keep old image if no new uploaded
                        about.ImageUrl = existing.ImageUrl;
                    }

                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Abouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var about = await _context.About.FirstOrDefaultAsync(m => m.Id == id);
            if (about == null) return NotFound();

            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var about = await _context.About.FindAsync(id);
            if (about != null)
            {
                // Delete image file
                if (!string.IsNullOrEmpty(about.ImageUrl))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", about.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                _context.About.Remove(about);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(int id) => _context.About.Any(e => e.Id == id);
    }
}
