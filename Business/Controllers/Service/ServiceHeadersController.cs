using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Service;

namespace Business.Controllers.Service
{
    public class ServiceHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceHeadersController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: ServiceHeaders
        public async Task<IActionResult> Index()
        {
            var headers = await _context.ServiceHeader.ToListAsync();
            return View(headers);
        }

        // GET: ServiceHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var serviceHeader = await _context.ServiceHeader.FirstOrDefaultAsync(h => h.Id == id);
            if (serviceHeader == null) return NotFound();

            return View(serviceHeader);
        }

        // GET: ServiceHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceHeader serviceHeader)
        {
            if (ModelState.IsValid)
            {
                if (serviceHeader.ImageFile != null && serviceHeader.ImageFile.Length > 0)
                {
                    serviceHeader.ImageUrl = await SaveImageAsync(serviceHeader.ImageFile);
                }

                _context.Add(serviceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceHeader);
        }

        // GET: ServiceHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var serviceHeader = await _context.ServiceHeader.FindAsync(id);
            if (serviceHeader == null) return NotFound();

            return View(serviceHeader);
        }

        // POST: ServiceHeaders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceHeader serviceHeader)
        {
            if (id != serviceHeader.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.ServiceHeader.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                    if (existing == null) return NotFound();

                    if (serviceHeader.ImageFile != null && serviceHeader.ImageFile.Length > 0)
                    {
                        DeleteImageIfExists(existing.ImageUrl);
                        serviceHeader.ImageUrl = await SaveImageAsync(serviceHeader.ImageFile);
                    }
                    else
                    {
                        serviceHeader.ImageUrl = existing.ImageUrl;
                    }

                    _context.Update(serviceHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceHeaderExists(serviceHeader.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceHeader);
        }

        // GET: ServiceHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var serviceHeader = await _context.ServiceHeader.FirstOrDefaultAsync(h => h.Id == id);
            if (serviceHeader == null) return NotFound();

            return View(serviceHeader);
        }

        // POST: ServiceHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceHeader = await _context.ServiceHeader.FindAsync(id);
            if (serviceHeader != null)
            {
                DeleteImageIfExists(serviceHeader.ImageUrl);
                _context.ServiceHeader.Remove(serviceHeader);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceHeaderExists(int id)
        {
            return _context.ServiceHeader.Any(e => e.Id == id);
        }

        // Save uploaded image to wwwroot/uploads/service and return relative URL
        private async Task<string> SaveImageAsync(Microsoft.AspNetCore.Http.IFormFile imageFile)
        {
            string uploadDir = Path.Combine(_env.WebRootPath, "uploads", "service");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/uploads/service/{fileName}";
        }

        // Delete image file if exists
        private void DeleteImageIfExists(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string filePath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
