using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Site;

namespace Business.Controllers.Site
{
    public class LogoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LogoesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Logoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Logos.ToListAsync());
        }

        // GET: Logoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var logo = await _context.Logos.FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null) return NotFound();

            return View(logo);
        }

        // GET: Logoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Logoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Logo logo)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (logo.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/logo");
                    Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await logo.ImageFile.CopyToAsync(fileStream);
                    }

                    logo.ImageUrl = "/uploads/logo/" + uniqueFileName;
                }

                _context.Add(logo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(logo);
        }

        // GET: Logoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var logo = await _context.Logos.FindAsync(id);
            if (logo == null) return NotFound();

            return View(logo);
        }

        // POST: Logoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Logo logo)
        {
            if (id != logo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (logo.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/logo");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await logo.ImageFile.CopyToAsync(fileStream);
                        }

                        logo.ImageUrl = "/uploads/logo/" + uniqueFileName;
                    }

                    _context.Update(logo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogoExists(logo.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(logo);
        }

        // GET: Logoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var logo = await _context.Logos.FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null) return NotFound();

            return View(logo);
        }

        // POST: Logoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logo = await _context.Logos.FindAsync(id);
            if (logo != null)
            {
                // Optionally delete the image file
                if (!string.IsNullOrEmpty(logo.ImageUrl))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, logo.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Logos.Remove(logo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LogoExists(int id)
        {
            return _context.Logos.Any(e => e.Id == id);
        }
    }
}
