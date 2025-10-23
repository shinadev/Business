using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.About;

namespace Business.Controllers.About
{
    public class AboutHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutHeadersController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: AboutHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.AboutHeader.ToListAsync());
        }

        // GET: AboutHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var aboutHeader = await _context.AboutHeader.FirstOrDefaultAsync(m => m.Id == id);
            if (aboutHeader == null) return NotFound();

            return View(aboutHeader);
        }

        // GET: AboutHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AboutHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutHeader aboutHeader)
        {
            // Image required validation
            if (aboutHeader.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please upload an image.");
            }

            if (!ModelState.IsValid)
            {
                return View(aboutHeader);
            }

            if (aboutHeader.ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(aboutHeader.ImageFile.FileName);
                string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "Habout");
                Directory.CreateDirectory(uploadPath);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await aboutHeader.ImageFile.CopyToAsync(stream);
                }

                aboutHeader.ImageUrl = "/uploads/Habout/" + fileName;
            }

            _context.Add(aboutHeader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AboutHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aboutHeader = await _context.AboutHeader.FindAsync(id);
            if (aboutHeader == null) return NotFound();

            return View(aboutHeader);
        }

        // POST: AboutHeaders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutHeader aboutHeader)
        {
            if (id != aboutHeader.Id) return NotFound();

            if (!ModelState.IsValid) return View(aboutHeader);

            try
            {
                var existingHeader = await _context.AboutHeader.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (existingHeader == null) return NotFound();

                if (aboutHeader.ImageFile != null)
                {
                    // Delete old image
                    if (!string.IsNullOrEmpty(existingHeader.ImageUrl))
                    {
                        var oldPath = Path.Combine(_env.WebRootPath, existingHeader.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    // Save new image
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(aboutHeader.ImageFile.FileName);
                    string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "Habout");
                    Directory.CreateDirectory(uploadPath);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await aboutHeader.ImageFile.CopyToAsync(stream);
                    }

                    aboutHeader.ImageUrl = "/uploads/Habout/" + fileName;
                }
                else
                {
                    // Keep old image if no new upload
                    aboutHeader.ImageUrl = existingHeader.ImageUrl;
                }

                _context.Update(aboutHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.AboutHeader.Any(e => e.Id == aboutHeader.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // GET: AboutHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aboutHeader = await _context.AboutHeader.FirstOrDefaultAsync(m => m.Id == id);
            if (aboutHeader == null) return NotFound();

            return View(aboutHeader);
        }

        // POST: AboutHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutHeader = await _context.AboutHeader.FindAsync(id);
            if (aboutHeader != null)
            {
                if (!string.IsNullOrEmpty(aboutHeader.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, aboutHeader.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                _context.AboutHeader.Remove(aboutHeader);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AboutHeaderExists(int id)
        {
            return _context.AboutHeader.Any(e => e.Id == id);
        }
    }
}
