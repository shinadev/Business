using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Data;
using Business.Models.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers.Home
{
    public class HomeSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeSectionsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: HomeSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeSection.ToListAsync());
        }

        // GET: HomeSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var homeSection = await _context.HomeSection.FirstOrDefaultAsync(m => m.Id == id);
            if (homeSection == null) return NotFound();

            return View(homeSection);
        }

        // GET: HomeSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomeSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,CaptionSmall,CaptionLarge,Button1Text,Button1Url,Button2Text,Button2Url,DisplayOrder,IsActive")] HomeSection homeSection,
            IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImageUrl", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/home");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                homeSection.ImageUrl = "/uploads/home/" + uniqueFileName;

                _context.Add(homeSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeSection);
        }

        // GET: HomeSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var homeSection = await _context.HomeSection.FindAsync(id);
            if (homeSection == null) return NotFound();

            return View(homeSection);
        }

        // POST: HomeSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,CaptionSmall,CaptionLarge,Button1Text,Button1Url,Button2Text,Button2Url,DisplayOrder,IsActive")] HomeSection homeSection,
            IFormFile imageFile)
        {
            if (id != homeSection.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.HomeSection.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (existing == null) return NotFound();

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/home");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Delete old image file if exists
                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var oldImageFullPath = Path.Combine(_webHostEnvironment.WebRootPath, existing.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                        if (System.IO.File.Exists(oldImageFullPath))
                            System.IO.File.Delete(oldImageFullPath);
                    }

                    homeSection.ImageUrl = "/uploads/home/" + uniqueFileName;
                }
                else
                {
                    homeSection.ImageUrl = existing.ImageUrl;
                }

                try
                {
                    _context.Update(homeSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeSectionExists(homeSection.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(homeSection);
        }

        // GET: HomeSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var homeSection = await _context.HomeSection.FirstOrDefaultAsync(m => m.Id == id);
            if (homeSection == null) return NotFound();

            return View(homeSection);
        }

        // POST: HomeSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeSection = await _context.HomeSection.FindAsync(id);
            if (homeSection != null)
            {
                // Delete image file
                if (!string.IsNullOrEmpty(homeSection.ImageUrl))
                {
                    var imageFullPath = Path.Combine(_webHostEnvironment.WebRootPath, homeSection.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(imageFullPath))
                        System.IO.File.Delete(imageFullPath);
                }

                _context.HomeSection.Remove(homeSection);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool HomeSectionExists(int id)
        {
            return _context.HomeSection.Any(e => e.Id == id);
        }
    }
}
